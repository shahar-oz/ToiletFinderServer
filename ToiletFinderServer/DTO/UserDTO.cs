namespace ToiletFinderServer.DTO
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly? DateOfBirth { get; set; }

        public UserDTO() { }
        public UserDTO(Models.User modelUser)
        {
            this.Username = modelUser.Username;
            this.Password = modelUser.Pass;
            this.Email = modelUser.Email;
            this.PhoneNumber = modelUser.PhoneNumber;
            this.DateOfBirth = modelUser.DateOfBirth;
        }

    }
}
