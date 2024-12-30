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
                .Include(t=>t.CurrentToiletsPhotos).FirstOrDefault();
        }
    }
    }

