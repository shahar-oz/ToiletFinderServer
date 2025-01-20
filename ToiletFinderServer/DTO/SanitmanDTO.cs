namespace ToiletFinderServer.DTO
{
    public class SanitmanDTO
    {
        public string Servicezone { get; set; }
        public string Username { get; set; }
        public string Pass { get; set; }
        public string Email { get; set; }
        public SanitmanDTO() { }
        public SanitmanDTO(Models.Sanitman sanitman)
        {
            this.Servicezone = sanitman.Servicezone;
            this.Username = sanitman.Username;
            this.Pass = sanitman.Pass;
            this.Email = sanitman.Email;    

        }
        public Models.Sanitman GetModels()
        {
            Models.Sanitman modelsSanit = new Models.Sanitman()
            {
                Servicezone = this.Servicezone,
                Username = this.Username,
                Pass = this.Pass,
                Email = this.Email
            };

            return modelsSanit;
        }
    }
}
