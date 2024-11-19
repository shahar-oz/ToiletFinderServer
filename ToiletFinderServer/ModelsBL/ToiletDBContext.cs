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
        }
    }

