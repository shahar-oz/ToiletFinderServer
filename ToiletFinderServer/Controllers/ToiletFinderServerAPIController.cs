using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Login([FromBody] DTO.UserDTO loginDto)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                Models.User? modelsUser = context.GetUser(loginDto.Email);

                //Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
                if (modelsUser == null || modelsUser.Pass != loginDto.Password || modelsUser.Email!=loginDto.Email)
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


    }
}
//public partial class ToiletDBContext : DbContext
//{
//    public UserDTO? GetUser(string email)
//    {
//        return this.AppUsers.Where(u => u.UserEmail == email)
//                            .Include(u => u.UserTasks)
//                            .ThenInclude(t => t.TaskComments)
//                            .FirstOrDefault();
//    }
//}








//[Route("api/[controller]")]
//[ApiController]
//public class ToiletFinderServerAPIController : ControllerBase
//{
//       //a variable to hold a reference to the db context!
//    private ToiletDBContext context;
//    //a variable that hold a reference to web hosting interface (that provide information like the folder on which the server runs etc...)
//    private IWebHostEnvironment webHostEnvironment;
//    //Use dependency injection to get the db context and web host into the constructor
//    public ToiletFinderServerAPIController(ToiletDBContext context, IWebHostEnvironment env)
//    {
//        this.context = context;
//        this.webHostEnvironment = env;
//    }

//    [HttpPost("register")]
//    public IActionResult Register([FromBody] DTO.UserDTO userDTO)
//    {
//        try
//        {
//            HttpContext.Session.Clear();
//            Models.User newUser = new User()
//            {
//                Username = userDTO.Username,
//                Email = userDTO.Email,
//                Pass = userDTO.Password,
//                PhoneNumber = userDTO.PhoneNumber,
//                DateOfBirth = userDTO.DateOfBirth
//            };

//            context.Users.Add(newUser);
//            context.SaveChanges();

//            DTO.UserDTO dtoUser = new DTO.UserDTO(newUser);
//            return Ok(dtoUser);
//        }

//        catch (Exception ex)
//        {
//            return BadRequest(ex.Message);
//        }

//    }
//}





