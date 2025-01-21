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
                //Check if user is logged int
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

        
}
    }














