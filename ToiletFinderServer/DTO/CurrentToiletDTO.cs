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

        public List<CurrentToiletPhotoDTO> Photos { get; set; }

        public CurrentToiletDTO() { }

        
        public Models.CurrentToilet GetModels()
        {
            Models.CurrentToilet m = new Models.CurrentToilet();
            m.ToiletId = ToiletId;
            m.Tlocation = Tlocation;
            m.Accessibility = Accessibility;
            m.Price = Price;
            m.Rate = Rate;
            m.Review = Review;
            m.CurrentToiletsPhotos = new List<Models.CurrentToiletsPhoto>();
            if (this.Photos != null)
            {
                foreach(var photo in this.Photos)
                {
                    m.CurrentToiletsPhotos.Add(new Models.CurrentToiletsPhoto()
                    {
                        PhotoId = photo.PhotoId,
                        ToiletId = m.ToiletId
                    });
                }
            }
            return m;
        }
        public CurrentToiletDTO(Models.CurrentToilet modelToilet, string photoBasePath)
        {
            this.ToiletId = modelToilet.ToiletId;
            this.Tlocation = modelToilet.Tlocation;
            this.Accessibility = modelToilet.Accessibility;
            this.Price = modelToilet.Price;
            this.Rate = modelToilet.Rate;
            this.Review = modelToilet.Review;
            this.Photos = new List<CurrentToiletPhotoDTO>();
            if (modelToilet.CurrentToiletsPhotos != null)
            {
                foreach (CurrentToiletsPhoto p in modelToilet.CurrentToiletsPhotos)
                {
                    this.Photos.Add(new CurrentToiletPhotoDTO()
                    {
                        PhotoId = p.PhotoId,
                        PhotoUrlPath = GetToiletPhotoPath(p.PhotoId, photoBasePath)
                    });
                }
            }
        }
        private string GetToiletPhotoPath(int photoId, string photoBasePath)
        {
            string virtualPath = $"/toilets/{photoId}";
            string path = $"{photoBasePath}\\toilets\\{photoId}.png";
            if (System.IO.File.Exists(path))
            {
                virtualPath += ".png";
            }
            else
            {
                path = $"{photoBasePath}\\toilets\\{photoId}.jpg";
                if (System.IO.File.Exists(path))
                {
                    virtualPath += ".jpg";
                }
                else
                {
                    virtualPath = $"/toilets/default.png";
                }
            }

            return virtualPath;
        }
    }

    public class CurrentToiletPhotoDTO
    {
        public int PhotoId { get; set; }
        public string PhotoUrlPath { get; set; }

        public CurrentToiletPhotoDTO() { }
        
    }
}
 