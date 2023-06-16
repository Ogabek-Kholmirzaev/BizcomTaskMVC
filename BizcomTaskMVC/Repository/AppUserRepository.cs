using BizcomTaskMVC.Data;
using BizcomTaskMVC.Entities;
using BizcomTaskMVC.Repository.IRepository;

namespace BizcomTaskMVC.Repository;

public class AppUserRepository : Repository<AppUser>, IAppUserRepository
{
    public AppUserRepository(AppDbContext context) : base(context)
    {
    }
}