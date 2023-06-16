namespace BizcomTaskMVC.Repository.IRepository;

public interface IUnitOfWork
{
    IAppUserRepository AppUserRepository { get; }
    ISubjectRepository SubjectRepository { get; }
    IAppUserSubjectRepository AppUserSubjectRepository { get; }

    Task SaveChangesAsync();
}