using ToiletFinderServer.Models;

namespace ToiletFinderServer.DTO
{
    public class RateDTO
    {
        public int? Rate1 {  get; set; }
        public int ToiletId {  get; set; }
        public virtual CurrentToilet Toilet { get; set; } = null!;

        public RateDTO() { }
        public RateDTO(Models.Rate modelUser)
        {
            this.Rate1 = modelUser.Rate1;
            this.ToiletId = modelUser.ToiletId;
        }
    }
}
