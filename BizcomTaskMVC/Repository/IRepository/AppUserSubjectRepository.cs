using BizcomTaskMVC.Data;
using BizcomTaskMVC.Entities;

namespace BizcomTaskMVC.Repository.IRepository;

public class AppUserSubjectRepository : Repository<AppUserSubject>, IAppUserSubjectRepository
{
    public AppUserSubjectRepository(AppDbContext context) : base(context)
    {
    }
}