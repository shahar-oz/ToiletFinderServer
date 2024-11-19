using ToiletFinderServer.Models;

namespace ToiletFinderServer.DTO
{
    public class ReviewDTO
    {
        public string? Review1 { get; set; }
        public int ToiletId { get; set; }
        public virtual CurrentToilet Toilet { get; set; } = null!;
    }
}
