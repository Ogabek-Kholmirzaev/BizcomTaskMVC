using AspNetCoreHero.ToastNotification.Abstractions;
using BizcomTaskMVC.Entities;
using BizcomTaskMVC.Models.ViewModels;
using BizcomTaskMVC.Repository.IRepository;
using BizcomTaskMVC.Statics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BizcomTaskMVC.Controllers;

[Authorize]
public class SubjectsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly INotyfService _notyfService;

    public SubjectsController(IUnitOfWork unitOfWork, INotyfService notyfService, UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _notyfService = notyfService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var userSubjects = user?.Subjects ?? new List<AppUserSubject>();
        
        return View(userSubjects);
    }

    [Authorize(Roles = UserRoles.Teacher)]
    public IActionResult Create() => View(new CreateSubjectVM());

    [HttpPost]
    [Authorize(Roles = UserRoles.Teacher)]
    public async Task<IActionResult> Create(CreateSubjectVM createSubjectVM)
    {
        if (!ModelState.IsValid) return View(createSubjectVM);

        var user = await _userManager.GetUserAsync(User);
        var subject = new Subject
        {
            Name = createSubjectVM.Name,
            Key = Guid.NewGuid().ToString("N"),
            AppUsers = new List<AppUserSubject>()
            {
                new AppUserSubject
                {
                    IsTeacher = true,
                    IsAdmin = true,
                    AppUserId = user!.Id
                }
            }
        };

        _unitOfWork.SubjectRepository.Add(subject);
        await _unitOfWork.SaveChangesAsync();

        _notyfService.Success("Subject created successfully");

        return RedirectToAction(nameof(Details), new { subjectId = subject.Id });
    }
    
    public IActionResult Join() => View(new JoinSubjectVM());

    [HttpPost]
    public async Task<IActionResult> Join(JoinSubjectVM joinSubjectVM)
    {
        if(!ModelState.IsValid) return View(joinSubjectVM);

        var subject = _unitOfWork.SubjectRepository.Get(s => s.Key == joinSubjectVM.Key);

        if(subject == null)
        {
            _notyfService.Error("Key is not suitable to any subject");

            return View(joinSubjectVM);
        }

        var user = await _userManager.GetUserAsync(User);

        if (user.Subjects.Any(s => s.SubjectId == subject.Id))
        {
            _notyfService.Information("You already joined to this course");

            return View(joinSubjectVM);
        }

        _unitOfWork.AppUserSubjectRepository.Add(new AppUserSubject
        {
            Grade = 0,
            IsTeacher = User.IsInRole(UserRoles.Teacher),
            AppUserId = user.Id,
            SubjectId = subject.Id,
        });
        await _unitOfWork.SaveChangesAsync();

        _notyfService.Success("Joined successfully");

        return RedirectToAction(nameof(Details), new { subjectId = subject.Id });
    }
    
    public IActionResult Details(Guid subjectId)
    {
        var subject = _unitOfWork.SubjectRepository.Get(s => s.Id == subjectId);

        if (subject == null)
        {
            _notyfService.Error("Subject not found");

            return RedirectToAction(nameof(Index));
        }

        return View(subject);
    }
    
    [Authorize(Roles = UserRoles.Teacher)]
    public async Task<IActionResult> Mark(Guid studentId, Guid userSubjectId)
    {
        var student = await _userManager.FindByIdAsync(studentId.ToString());

        if (student == null)
        {
            _notyfService.Error("Student not found");
            
            return RedirectToAction(nameof(Index));
        }

        var studentSubject = student.Subjects?.FirstOrDefault(s => s.Id == userSubjectId);

        if (studentSubject == null)
        {
            _notyfService.Error("Student doesn't have this subject");

            return RedirectToAction(nameof(Index));
        }

        var teacher = await _userManager.GetUserAsync(User);

        if (teacher?.Subjects?.FirstOrDefault(s => s.SubjectId == studentSubject.SubjectId) == null)
        {
            _notyfService.Error("Unauthorized");

            return RedirectToAction(nameof(Index));
        }

        return View(studentSubject);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Teacher)]
    public async Task<IActionResult> Mark(AppUserSubject appUserSubject)
    {
        var studentSubject = _unitOfWork.AppUserSubjectRepository
            .Get(s => s.Id == appUserSubject.Id);

        if (studentSubject == null)
        {
            _notyfService.Error("Student doesn't have this subject");

            return RedirectToAction(nameof(Index));
        }

        var teacher = await _userManager.GetUserAsync(User);

        if (teacher?.Subjects?.FirstOrDefault(s => s.SubjectId == appUserSubject.SubjectId) == null)
        {
            _notyfService.Error("Unauthorized");

            return RedirectToAction(nameof(Index));
        }

        _notyfService.Success("Marked successfully");

        studentSubject.Grade = appUserSubject.Grade;
        _unitOfWork.AppUserSubjectRepository.Update(studentSubject);
        await _unitOfWork.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { subjectId = appUserSubject.SubjectId });
    }

    [Authorize(Roles = UserRoles.Teacher)]
    public async Task<IActionResult> Edit(Guid userSubjectId)
    {
        var userSubject = _unitOfWork.AppUserSubjectRepository.Get(us => us.Id == userSubjectId);

        if (userSubject == null)
        {
            _notyfService.Error("Subject not found");

            return RedirectToAction(nameof(Index));
        }

        var teacher = await _userManager.GetUserAsync(User);

        if (userSubject.AppUserId != teacher.Id || !userSubject.IsAdmin)
        {
            _notyfService.Error("You aren't admin to this subject");

            return RedirectToAction(nameof(Index));
        }

        return View(userSubject.Subject);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Teacher)]
    public async Task<IActionResult> Edit(Subject subject)
    {
        if (!ModelState.IsValid) return View(subject);

        var subjectFromDb = _unitOfWork.SubjectRepository.Get(s => s.Id == subject.Id);

        if (subjectFromDb == null)
        {
            _notyfService.Error("Subject not found");

            return RedirectToAction(nameof(Index));
        }

        var teacher = await _userManager.GetUserAsync(User);

        if (teacher.Subjects.Any(s => s.IsAdmin && s.SubjectId == subjectFromDb.Id) == false)
        {
            _notyfService.Error("You aren't admin");

            return RedirectToAction(nameof(Index));
        }

        _notyfService.Success("Subject edited successfully");
        subjectFromDb.Name = subject.Name;
        await _unitOfWork.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = UserRoles.Teacher)]
    public async Task<IActionResult> Delete(Guid subjectId) 
    {
        var subject = _unitOfWork.SubjectRepository.Get(s => s.Id == subjectId);

        if (subject == null)
        {
            _notyfService.Error("Subject not found");

            return RedirectToAction(nameof(Index));
        }

        var teacher = await _userManager.GetUserAsync(User);
        var teacherSubject = subject.AppUsers?.FirstOrDefault(u => u.AppUserId == teacher?.Id);

        if (teacherSubject == null || teacherSubject.IsAdmin == false)
        {
            _notyfService.Error("You are not an admin to this course");

            return RedirectToAction(nameof(Index));
        }

        return View(subject);
    }

    [HttpPost, ActionName(nameof(Delete))]
    [Authorize(Roles = UserRoles.Teacher)]
    public async Task<IActionResult> DeleteSubjectById(Guid id)
    {
        var teacherSubject = _unitOfWork.AppUserSubjectRepository.Get(us => us.SubjectId == id);

        if (teacherSubject == null)
        {
            _notyfService.Error("Subject not found");

            return RedirectToAction(nameof(Index));
        }

        if (!teacherSubject.IsAdmin)
        {
            _notyfService.Error("You must be an admin of this subject");

            return RedirectToAction(nameof(Index));
        }

        _unitOfWork.SubjectRepository.Remove(teacherSubject.Subject!);
        await _unitOfWork.SaveChangesAsync();

        _notyfService.Success("Deleted Successfully");

        return RedirectToAction(nameof(Index));
    }
}