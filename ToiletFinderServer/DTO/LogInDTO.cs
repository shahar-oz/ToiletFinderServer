namespace ToiletFinderServer.DTO
{
    public class LogInDTO
    {
        public string Password { get; set; }
        public string Email { get; set; }

        public LogInDTO() { }
        public LogInDTO(Models.User modelUser)
        {
           
            this.Password = modelUser.Pass;
            this.Email = modelUser.Email;


        }
        public Models.User GetModels()
        {
            Models.User modelsUser = new Models.User()
            {


                Email = this.Email,
                Pass = this.Password,

            };

            return modelsUser;
        }
    }
}
