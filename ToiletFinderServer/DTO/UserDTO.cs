namespace ToiletFinderServer.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int? UserType { get; set; }

        public UserDTO() { }
        public UserDTO(Models.User modelUser)
        {
            this.Username = modelUser.Username;
            this.Password = modelUser.Pass;
            this.Email = modelUser.Email;
            this.UserType = modelUser.UserType;
            this.UserId = modelUser.UserId;

        }
        public Models.User GetModels()
        {
            Models.User modelsUser = new Models.User()
            {
                UserId = this.UserId,
                Username = this.Username,
                Email = this.Email,
                Pass = this.Password,
                UserType = this.UserType
            };

            return modelsUser;
        }

    }
}
