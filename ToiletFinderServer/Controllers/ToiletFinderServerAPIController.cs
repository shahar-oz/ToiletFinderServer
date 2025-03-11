using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ToiletFinderServer.DTO;
using ToiletFinderServer.Models;

namespace ToiletFinderServer.Controllers
{
    [Route("api")]
    [ApiController]
    public class ToiletFinderServerAPIController : ControllerBase
    {

        //a variable to hold a reference to the db context!
        private ToiletDBContext context;
        //a variable that hold a reference to web hosting interface (that provide information like the folder on which the server runs etc...)
        private IWebHostEnvironment webHostEnvironment;
        //Use dependency injection to get the db context and web host into the constructor
        public ToiletFinderServerAPIController(ToiletDBContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.webHostEnvironment = env;
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] DTO.LogInDTO loginDto)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                Models.User? modelsUser = context.GetUser(loginDto.Email);

                //Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
                if (modelsUser == null || modelsUser.Pass != loginDto.Password || modelsUser.Email != loginDto.Email)
                {
                    return Unauthorized();
                }

                //Login suceed! now mark login in session memory!
                HttpContext.Session.SetString("loggedInUser", modelsUser.Email);

                DTO.UserDTO dtoUser = new DTO.UserDTO(modelsUser);
                return Ok(dtoUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] DTO.UserDTO userDto)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Create model user class
                Models.User modelsUser = userDto.GetModels();

                context.Users.Add(modelsUser);
                context.SaveChanges();

                //User was added!
                DTO.UserDTO dtoUser = new DTO.UserDTO(modelsUser);
                return Ok(dtoUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("addToilet")]
        public IActionResult AddToilet([FromBody] DTO.CurrentToiletDTO toiletDto)
        {
            try
            {
                //Check if user is logged in
                string? email = HttpContext.Session.GetString("loggedInUser");

                if (email == null || email == "")
                {
                    return Unauthorized();
                }

                //Create model user class
                Models.CurrentToilet modelsToilet = toiletDto.GetModels();

                modelsToilet.StatusId = 2;

                context.CurrentToilets.Add(modelsToilet);
                context.SaveChanges();

                //Toilet was added!

                string photosLocalPath = webHostEnvironment.WebRootPath;
                DTO.CurrentToiletDTO dtoToilet = new DTO.CurrentToiletDTO(modelsToilet, photosLocalPath);
                string json = JsonSerializer.Serialize(dtoToilet);
                return Ok(dtoToilet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



        [HttpPost("UploadToiletImage")]
        public async Task<IActionResult> UploadToiletImageAsync(IFormFile file, [FromQuery] int toiletId)
        {
            //Check if who is logged in
            string? userEmail = HttpContext.Session.GetString("loggedInUser");
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User is not logged in");
            }

            //Get model toilet class from DB with matching id. 
            Models.CurrentToilet? toilet = context.GetToilet(toiletId);
            //Clear the tracking of all objects to avoid double tracking
            context.ChangeTracker.Clear();

            if (toilet == null)
            {
                return Unauthorized("Toilet is not found in the database");
            }

            //Add photo to database (only the record)
            CurrentToiletsPhoto photoRecord = new CurrentToiletsPhoto() { ToiletId = toiletId };
            context.CurrentToiletsPhotos.Add(photoRecord);
            context.SaveChanges();

            //Read all files sent
            long imagesSize = 0;

            if (file.Length > 0)
            {
                //Check the file extention!
                string[] allowedExtentions = { ".png", ".jpg" };
                string extention = "";
                if (file.FileName.LastIndexOf(".") > 0)
                {
                    extention = file.FileName.Substring(file.FileName.LastIndexOf(".")).ToLower();
                }
                if (!allowedExtentions.Where(e => e == extention).Any())
                {
                    //Extention is not supported
                    return BadRequest("File sent with non supported extention");
                }

                //Build path in the web root (better to a specific folder under the web root
                string filePath = $"{this.webHostEnvironment.WebRootPath}\\toilets\\{photoRecord.PhotoId}{extention}";

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);

                    if (IsImage(stream))
                    {
                        imagesSize += stream.Length;
                    }
                    else
                    {
                        //Delete the file if it is not supported!
                        System.IO.File.Delete(filePath);
                    }

                }

            }

            toilet.CurrentToiletsPhotos.Add(photoRecord);
            DTO.CurrentToiletDTO dtoToilet = new DTO.CurrentToiletDTO(toilet, this.webHostEnvironment.WebRootPath);
            return Ok(dtoToilet);
        }

        //Helper functions

        //this function gets a file stream and check if it is an image
        private static bool IsImage(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            List<string> jpg = new List<string> { "FF", "D8" };
            List<string> bmp = new List<string> { "42", "4D" };
            List<string> gif = new List<string> { "47", "49", "46" };
            List<string> png = new List<string> { "89", "50", "4E", "47", "0D", "0A", "1A", "0A" };
            List<List<string>> imgTypes = new List<List<string>> { jpg, bmp, gif, png };

            List<string> bytesIterated = new List<string>();

            for (int i = 0; i < 8; i++)
            {
                string bit = stream.ReadByte().ToString("X2");
                bytesIterated.Add(bit);

                bool isImage = imgTypes.Any(img => !img.Except(bytesIterated).Any());
                if (isImage)
                {
                    return true;
                }
            }

            return false;
        }

       
        [HttpGet("GetAllApprovedToilets")]
        public IActionResult GetAllApprovedToilets()
        {
            try
            {
                List<Models.CurrentToilet> listApprovedToilets = context.GetAllApprovedToilets();
                List<CurrentToiletDTO> output = new List<CurrentToiletDTO>();
                foreach (Models.CurrentToilet t in listApprovedToilets)
                {
                    output.Add(new CurrentToiletDTO(t, this.webHostEnvironment.WebRootPath));
                }
                return Ok(output);
            
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllPendingToilets")]
        public IActionResult GetAllPendingToilets()
        {
            try
            {
                List<Models.CurrentToilet> listApprovedToilets = context.GetAllPendingToilets();
                List<CurrentToiletDTO> output = new List<CurrentToiletDTO>();
                foreach(Models.CurrentToilet t in  listApprovedToilets)
                {
                    output.Add(new CurrentToiletDTO(t, this.webHostEnvironment.WebRootPath));
                }
                return Ok(output);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllDeclinedToilets")]
        public IActionResult GetAllDeclinedToilets()
        {
            try
            {
                List<Models.CurrentToilet> listApprovedToilets = context.GetAllDeclinedToilets();
                List<CurrentToiletDTO> output = new List<CurrentToiletDTO>();
                foreach (Models.CurrentToilet t in listApprovedToilets)
                {
                    output.Add(new CurrentToiletDTO(t, this.webHostEnvironment.WebRootPath));
                }
                return Ok(output);
            }
               
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetToiletByUser")]
        public IActionResult GetToiletByUser([FromQuery] int userID)
        {
            try
            {
                //validate its a service provider 
                string? email = HttpContext.Session.GetString("loggedInUser");
                if (email == null)
                    return Unauthorized();
                User? u = context.GetUser(email);
                if (u == null || u.UserType != 2)
                    return Unauthorized();
                List<Models.CurrentToilet> listToilets = context.GetToiletByUser(userID);
                List<DTO.CurrentToiletDTO> final = new List<CurrentToiletDTO>();
                foreach(CurrentToilet t in listToilets)
                {
                    final.Add(new DTO.CurrentToiletDTO(t, this.webHostEnvironment.WebRootPath));
                }

                return Ok(final);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("addRateandReview")]
        public IActionResult AddRateandReview([FromQuery] int toiletId, [FromQuery] int rate, [FromBody] string reviewText)
        {
            try
            {
                // Check if user is logged in
                //validate its a service provider 
                string? email = HttpContext.Session.GetString("loggedInUser");
                if (email == null)
                    return Unauthorized();
                User? u = context.GetUser(email);
                if (u == null || u.UserType != 1)
                    return Unauthorized();


                // Get toilet from database
                Models.CurrentToilet? toilet = context.GetToilet(toiletId);
                if (toilet == null)
                {
                    return NotFound("Toilet not found in the database");
                }

                // Add new review to database
                Models.Review newReview = new Models.Review()
                {
                    ToiletId = toiletId,
                    Review1 = reviewText
                };
                context.Reviews.Add(newReview);

                // Add new rating to database
                Models.Rate newRate = new Models.Rate()
                {
                    ToiletId = toiletId,
                    Rate1 = rate
                };
                context.Rates.Add(newRate);

                context.SaveChanges();

                // Return updated toilet with new reviews and rates
                DTO.CurrentToiletDTO updatedToilet = new DTO.CurrentToiletDTO(toilet, webHostEnvironment.WebRootPath);
                return Ok(updatedToilet);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #region Change Status 
        //change status
        [HttpPost("ChangeStatusToApprove")]
        public IActionResult ChangeStatusToApprove(DTO.CurrentToiletDTO toiletDTO)
        {
            try
            {
                //validate its an admin
                string? email = HttpContext.Session.GetString("loggedInUser");

                if (email == null || email == "")
                {
                    return Unauthorized();
                }
                User? u = context.GetUser(email);
                if (u == null || u.UserType != 3)
                    return Unauthorized();

                bool success = context.SetStatus(toiletDTO.ToiletId, 1);
                if (success)
                    return Ok(success);
                else
                    return BadRequest("Either toiletID not found or DB connection problem!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ChangStatusToDecline")]
        public IActionResult ChangStatusToDecline(DTO.CurrentToiletDTO toiletDTO)
        {
            try
            {
                //validate its an admin
                string? email = HttpContext.Session.GetString("loggedInUser");

                if (email == null || email == "")
                {
                    return Unauthorized();
                }
                User? u = context.GetUser(email);
                if (u == null || u.UserType != 3)
                    return Unauthorized();

                bool success = context.SetStatus(toiletDTO.ToiletId, 3);
                if (success)
                    return Ok(success);
                else
                    return BadRequest("Either toiletID not found or DB connection problem!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


    }
}














