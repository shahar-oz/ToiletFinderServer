using Microsoft.EntityFrameworkCore;
using ToiletFinderServer.Models;

namespace ToiletFinderServer.ModelsBL
{
    public partial class ToiletDBContext : DbContext
    {
        public User? GetUser(string email)
        {
            return this.Users.Where(u => u.UserEmail == email)
                                .Include(u => u.UserTasks)
                                .ThenInclude(t => t.TaskComments)
                                .FirstOrDefault();
        }
    }
}
