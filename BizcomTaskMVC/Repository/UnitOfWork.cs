using BizcomTaskMVC.Data;
using BizcomTaskMVC.Repository.IRepository;

namespace BizcomTaskMVC.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IAppUserRepository AppUserRepository { get; }
    public ISubjectRepository SubjectRepository { get; }
    public IAppUserSubjectRepository AppUserSubjectRepository { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        AppUserRepository ??= new AppUserRepository(_context);
        SubjectRepository ??= new SubjectRepository(_context);
        AppUserSubjectRepository ??= new AppUserSubjectRepository(_context);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}