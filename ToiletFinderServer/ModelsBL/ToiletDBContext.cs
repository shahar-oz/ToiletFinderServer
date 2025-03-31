using Microsoft.EntityFrameworkCore;
using ToiletFinderServer.Models;

namespace ToiletFinderServer.Models
{
    public partial class ToiletDBContext : DbContext
    {
        public User? GetUser(string email)
        {
            return this.Users.Where(u => u.Email == email).FirstOrDefault();
        }

        public CurrentToilet? GetToilet(int id)
        {
            return this.CurrentToilets.Where(t => t.ToiletId == id)
                .Include(t => t.CurrentToiletsPhotos)
                .Include(t => t.Rates)
                .Include(t=>t.Reviews).FirstOrDefault();
        }

        //gets all toilets by status

        public List<CurrentToilet>? GetAllToiletsByStatus(int i)
        {
            return this.CurrentToilets.Include(t => t.CurrentToiletsPhotos).Include(t => t.Rates)
                .Include(t => t.Reviews).Include(t=>t.User).Where(u => u.StatusId == i).ToList();
        }
        //gets all approved toilets
        public List<CurrentToilet>? GetAllApprovedToilets()
        {
            return this.CurrentToilets.Include(t => t.CurrentToiletsPhotos).Include(t => t.Rates)
                .Include(t => t.Reviews).Include(t => t.User).Where(u => u.StatusId == 1).ToList();
        }
        public List<CurrentToilet>? GetAllPendingToilets()
        {
            return this.CurrentToilets.Include(t => t.CurrentToiletsPhotos).Include(t => t.Rates)
                .Include(t => t.Reviews).Include(t => t.User).Where(u => u.StatusId == 2).ToList();
        }
        public List<CurrentToilet>? GetAllDeclinedToilets()
        {
            return this.CurrentToilets.Include(t => t.CurrentToiletsPhotos).Include(t => t.Rates)
                .Include(t => t.Reviews).Include(t => t.User).Where(u => u.StatusId == 3).ToList();
        }
        public bool SetStatus(int toiletId, int statusId)
        {
            try
            {
                CurrentToilet? t = this.CurrentToilets.Where(t => t.ToiletId == toiletId).FirstOrDefault();
                if (t != null)
                {
                    t.StatusId = statusId;
                    this.Update(t);
                    this.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public List<CurrentToilet?> GetToiletByUser(int userID)
        {
            return this.CurrentToilets.Include(t => t.CurrentToiletsPhotos).Include(t => t.Rates)
                .Include(t => t.Reviews).Include(t => t.User).Where(r => r.UserId == userID).ToList();
        }

        //gets all toilets
        public List<CurrentToilet>? GetAllToilets()
        {
            return this.CurrentToilets.Include(t => t.CurrentToiletsPhotos).Include(t => t.Rates)
                .Include(t => t.Reviews).Include(t => t.User).ToList();
        }

        
    }
}

