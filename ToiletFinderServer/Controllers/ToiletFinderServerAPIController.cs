using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToiletFinderServer.Models;

namespace ToiletFinderServer.Controllers
{
    [Route("api/[controller]")]
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
    }
}
