using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiTesi.Models;

namespace WebApiTesi.Data
{
    public class Context : IdentityDbContext<User>
    {
        public Context(DbContextOptions<Context> options): base(options)
        {
            
        }
    }
}
