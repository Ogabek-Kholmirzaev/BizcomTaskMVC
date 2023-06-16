using AspNetCoreHero.ToastNotification.Abstractions;
using BizcomTaskMVC.Entities;
using BizcomTaskMVC.Repository.IRepository;
using BizcomTaskMVC.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BizcomTaskMVC.Controllers;

public class StatisticsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserService _userService;
    private readonly UserManager<AppUser> _userManager;
    private readonly INotyfService _notyfService;

    public StatisticsController(
        IUnitOfWork unitOfWork,
        UserService userService,
        UserManager<AppUser> userManager,
        INotyfService notyfService
        )
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
        _userManager = userManager;
        _notyfService = notyfService;
    }

    public IActionResult Index() => View();

    public async Task<IActionResult> GetStudentsUnder20()
    {
        var currentYear = DateTime.UtcNow.Year;
        var studentsUnder20 = (await _userService.GetAllStudentsAsync())
            .Where(s => currentYear - s.BirthDate.Year < 20).ToList();

        return View(studentsUnder20);
    }

    public async Task<IActionResult> GetStudentsBirthdayBetween12Aug18Sep()
    {
        var august = 8;
        var augustDay = 12;
        var september = 9;
        var septemberDay = 18;

        var studentsInRange = (await _userService.GetAllStudentsAsync())
            .Where(s => (s.BirthDate.Month >= august && s.BirthDate.Month <= september)
                        && (s.BirthDate.Day >= augustDay && s.BirthDate.Day <= septemberDay)).ToList();

        return View(studentsInRange);
    }

    public async Task<IActionResult> GetTeachersOver55()
    {
        var currentYear = DateTime.UtcNow.Year;
        var teachers = (await _userService.GetAllTeachersAsync())
            .Where(t => currentYear - t.BirthDate.Year > 55).ToList();

        return View(teachers);
    }

    public async Task<IActionResult> GetBeelineUsers()
    {
        var beelineUsers = (await _userService.GetAllUsersAsync())
            .Where(u => u.PhoneNumber[5] == '0' || u.PhoneNumber[5] == '1').ToList();

        return View(beelineUsers);
    }

    public async Task<IActionResult> GetStudentsFilterByPhrase(string phrase)
    {
        if (string.IsNullOrWhiteSpace(phrase))
        {
            _notyfService.Error("Entered empty value or white spaces");

            return RedirectToAction(nameof(Index));
        }

        var trim = phrase.Trim().ToLower();
        var studentsFilter = (await _userService.GetAllStudentsAsync())
            .Where(s => s.FirstName.ToLower().Contains(trim) || s.LastName.ToLower().Contains(trim))
            .ToList();

        ViewData["phrase"] = trim;

        return View(studentsFilter);
    }

    public async Task<IActionResult> GetUserSubjectMaxScore(Guid userId)
    {
        var student = await _userManager.FindByIdAsync(userId.ToString());

        if (student == null)
        {
            _notyfService.Error("User not found");

            return RedirectToAction(nameof(Index));
        }

        if (student.Subjects == null || student.Subjects.Count == 0)
        {
            _notyfService.Error("User does not have any subjects");

            return RedirectToAction(nameof(Index));
        }

        var subjects = student.Subjects.OrderByDescending(s => s.Grade).ToList();

        return View(subjects[0]);
    }

    public async Task<IActionResult> GetTeacherSubjectStudentScoreOver80AndCountOver10(Guid teacherId)
    {
        var teacher = (await _userService.GetAllTeachersAsync())
            .FirstOrDefault(t => t.Id == teacherId);

        if (teacher == null)
        {
            _notyfService.Error("Teacher not found");

            RedirectToAction(nameof(Index));
        }

        foreach (var subject in teacher?.Subjects ?? new List<AppUserSubject>())
        {
            var subjectStudents = _unitOfWork.AppUserSubjectRepository.GetAll()
                .Where(us => us.SubjectId == subject.SubjectId && us.IsTeacher == false && us.Grade > 80)
                .ToList();

            if (subjectStudents.Count < 10)
                continue;

            return View(subject);
        }

        _notyfService.Error("Subject not found");

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> GetTeachersByStudentsHighScoreOver97()
    {
        var teachersResult = new List<AppUser>();
        var teachers = await _userService.GetAllTeachersAsync();

        foreach (var teacher in teachers)
        {
            if (teacher.Subjects == null)
                continue;

            foreach (var teacherSubject in teacher.Subjects)
            {
                var subject = _unitOfWork.SubjectRepository.Get(s => s.Id == teacherSubject.SubjectId);
                var isTrue = subject!.AppUsers?.Any(us => us.Grade > 97 && us.IsTeacher == false);

                if (isTrue == true)
                {
                    teachersResult.Add(teacher);
                    break;
                }
            }
        }

        return View(teachersResult);
    }

    public IActionResult GetSubjectHighestAverageScore()
    {
        Guid maxAverageSubjectId = default;
        double maxAverageScore = -1;

        var subjects = _unitOfWork.SubjectRepository.GetAll().ToList();

        if (subjects.Count == 0)
        {
            _notyfService.Error("There are not any subjects");

            return RedirectToAction(nameof(Index));
        }

        foreach (var subject in subjects)
        {
            if (subject.AppUsers == null || subject.AppUsers.Count == 0)
                continue;

            var overAllScore = subject.AppUsers.Where(u => u.IsTeacher == false)
                .Sum(s => s.Grade);

            var studentsCount = subject.AppUsers.Count(u => u.IsTeacher == false);
            var average = (double)overAllScore / studentsCount;

            if (average > maxAverageScore)
            {
                maxAverageSubjectId = subject.Id;
                maxAverageScore = average;
            }
        }

        return View(_unitOfWork.SubjectRepository.Get(s => s.Id == maxAverageSubjectId));
    }
}