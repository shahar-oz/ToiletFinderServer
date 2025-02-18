using ToiletFinderServer.Models;

namespace ToiletFinderServer.DTO
{
    public class ReviewDTO
    {
        public string? Review { get; set; }
        public int ToiletId { get; set; }
        public virtual CurrentToilet Toilet { get; set; } = null!;

        public ReviewDTO() { }
        public ReviewDTO(Models.Review model)
        {
            this.Review = model.Review1;
            this.ToiletId = model.ToiletId;
        }
    }
}
