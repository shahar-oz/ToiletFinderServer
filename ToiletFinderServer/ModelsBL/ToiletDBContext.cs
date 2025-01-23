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
                .Include(t => t.CurrentToiletsPhotos).FirstOrDefault();
        }

        //gets all toilets by status

        public List<CurrentToilet>? GetAllToiletsByStatus(int i)
        {
            return this.CurrentToilets.Where(u => u.StatusId == i).ToList();
        }
        //gets all approved toilets
        public List<CurrentToilet>? GetAllApprovedToilets()
        {
            return this.CurrentToilets.Where(u => u.StatusId == 1).ToList();
        }

        //gets all toilets
        public List<CurrentToilet>? GetAllToilets()
        {
            return this.CurrentToilets.ToList();
        }
    }
    }

