using BizcomTaskMVC.Data;
using BizcomTaskMVC.Entities;
using BizcomTaskMVC.Repository.IRepository;

namespace BizcomTaskMVC.Repository;

public class SubjectRepository : Repository<Subject>, ISubjectRepository
{
    public SubjectRepository(AppDbContext context) : base(context)
    {
    }
}