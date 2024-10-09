using ToiletFinderServer.Models;

namespace ToiletFinderServer.DTO
{
    public class CurrentToiletDTO
    {
      public int  ToiletId {  get; set; }
      public string? Tlocation { get; set; }
      public bool? Accessibility { get; set; }
      public double? Price { get; set; }
      public Rate? Rate { get; set; }
      public Review? Review { get; set; }

        //public CurrentToiletDTO() { }
        //public CurrentToiletDTO(Models.CurrentToilet modelUser)
        //{
        //    this.ToiletId = modelUser.ToiletId;
        //    this.Tlocation = modelUser.Tlocation;
        //    this.Accessibility = modelUser.Accessibility;
        //    this.Price = modelUser.Price;
        //    this.Rate = modelUser.Rate;
        //    this.Review = modelUser.Review;
        //}
    }
}
 