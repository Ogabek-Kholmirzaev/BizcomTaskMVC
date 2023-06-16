using BizcomTaskMVC.Entities;
using BizcomTaskMVC.Statics;
using Microsoft.AspNetCore.Identity;

namespace BizcomTaskMVC.Data;

public class DbInitializer
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly AppDbContext _context;

    public DbInitializer(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        AppDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public void Initialize()
    {
        if (_userManager.Users.Any())
            return;

        #region Seed_Students_And_Teachers

        if(_roleManager.RoleExistsAsync(UserRoles.Student).GetAwaiter().GetResult() == false)
            _roleManager.CreateAsync(new AppRole() { Name = UserRoles.Student }).GetAwaiter().GetResult();

        if (_roleManager.RoleExistsAsync(UserRoles.Teacher).GetAwaiter().GetResult() == false)
            _roleManager.CreateAsync(new AppRole() { Name = UserRoles.Teacher }).GetAwaiter().GetResult();

        var password = "12345678";
        var student1 = new AppUser
        {
            UserName = Guid.NewGuid().ToString(),
            Email = "student1@email.com",
            PhoneNumber = "+998901234567",
            FirstName = "FName1",
            LastName = "LName1",
            BirthDate = DateTime.UtcNow.Date.AddYears(-18).AddMonths(2),
            StudentRegNumber = 1,
        };

        var student2 = new AppUser
        {
            UserName = Guid.NewGuid().ToString(),
            Email = "student2@email.com",
            PhoneNumber = "+998911234567",
            FirstName = "FName2",
            LastName = "LName2",
            BirthDate = DateTime.UtcNow.Date.AddYears(-17),
            StudentRegNumber = 2,
        };

        var student3 = new AppUser
        {
            UserName = Guid.NewGuid().ToString(),
            Email = "student3@email.com",
            PhoneNumber = "+998931234567",
            FirstName = "FName3",
            LastName = "LName3",
            BirthDate = DateTime.UtcNow.Date.AddYears(-19).AddMonths(2),
            StudentRegNumber = 3
        };

        var student4 = new AppUser
        {
            UserName = Guid.NewGuid().ToString(),
            Email = "student4@email.com",
            PhoneNumber = "+998941234567",
            FirstName = "FName4",
            LastName = "LName4",
            BirthDate = DateTime.UtcNow.Date.AddYears(-21),
            StudentRegNumber = 4
        };

        var student5 = new AppUser
        {
            UserName = Guid.NewGuid().ToString(),
            Email = "student5@email.com",
            PhoneNumber = "+998951234567",
            FirstName = "FName5",
            LastName = "LName5",
            BirthDate = DateTime.UtcNow.Date.AddYears(-20),
            StudentRegNumber = 5
        };

        var student6 = new AppUser
        {
            UserName = Guid.NewGuid().ToString(),
            Email = "student6@email.com",
            PhoneNumber = "+998971234567",
            FirstName = "FName6",
            LastName = "LName6",
            BirthDate = DateTime.UtcNow.Date.AddYears(-20).AddMonths(2),
            StudentRegNumber = 6
        };

        var student7 = new AppUser
        {
            UserName = Guid.NewGuid().ToString(),
            Email = "student7@email.com",
            PhoneNumber = "+998991234567",
            FirstName = "FName7",
            LastName = "LName7",
            BirthDate = DateTime.UtcNow.Date.AddYears(-25),
            StudentRegNumber = 7
        };

        var student8 = new AppUser
        {
            UserName = Guid.NewGuid().ToString(),
            Email = "student8@email.com",
            PhoneNumber = "+998900234567",
            FirstName = "FName8",
            LastName = "LName8",
            BirthDate = DateTime.UtcNow.Date.AddYears(-17),
            StudentRegNumber = 8
        };

        var student9 = new AppUser
        {
            UserName = Guid.NewGuid().ToString(),
            Email = "student9@email.com",
            PhoneNumber = "+998900134567",
            FirstName = "FName9",
            LastName = "LName9",
            BirthDate = DateTime.UtcNow.Date.AddYears(-29),
            StudentRegNumber = 9
        };

        var student10 = new AppUser
        {
            UserName = Guid.NewGuid().ToString(),
            Email = "student10@email.com",
            PhoneNumber = "+998910124567",
            FirstName = "FName10",
            LastName = "LName10",
            BirthDate = DateTime.UtcNow.Date.AddYears(-23).AddMonths(2),
            StudentRegNumber = 10
        };

        var student11 = new AppUser
        {
            UserName = Guid.NewGuid().ToString(),
            Email = "student11@email.com",
            PhoneNumber = "+998910114567",
            FirstName = "FName11",
            LastName = "LName11",
            BirthDate = DateTime.UtcNow.Date.AddYears(-23),
            StudentRegNumber = 11
        };

        var student12 = new AppUser
        {
            UserName = Guid.NewGuid().ToString(),
            Email = "student12@email.com",
            PhoneNumber = "+998910114667",
            FirstName = "FName12",
            LastName = "LName12",
            BirthDate = DateTime.UtcNow.Date.AddYears(-21),
            StudentRegNumber = 12
        };

        var teacher1 = new AppUser
        {
            UserName = Guid.NewGuid().ToString(),
            Email = "teacher1@email.com",
            PhoneNumber = "+998919876543",
            FirstName = "FNameT1",
            LastName = "LNameT1",
            BirthDate = DateTime.UtcNow.Date.AddYears(-56)
        };

        var teacher2 = new AppUser
        {
            UserName = Guid.NewGuid().ToString(),
            Email = "teacher2@email.com",
            PhoneNumber = "+998909876543",
            FirstName = "FNameT2",
            LastName = "LNameT2",
            BirthDate = DateTime.UtcNow.Date.AddYears(-60)
        };

        var teacher3 = new AppUser
        {
            UserName = Guid.NewGuid().ToString(),
            Email = "teacher3@email.com",
            PhoneNumber = "+998949876543",
            FirstName = "FNameT3",
            LastName = "LNameT3",
            BirthDate = DateTime.UtcNow.Date.AddYears(-45)
        };

        _userManager.CreateAsync(student1, password).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(student1, UserRoles.Student);

        _userManager.CreateAsync(student2, password).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(student2, UserRoles.Student);

        _userManager.CreateAsync(student3, password).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(student3, UserRoles.Student);

        _userManager.CreateAsync(student4, password).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(student4, UserRoles.Student);

        _userManager.CreateAsync(student5, password).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(student5, UserRoles.Student);

        _userManager.CreateAsync(student6, password).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(student6, UserRoles.Student);

        _userManager.CreateAsync(student7, password).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(student7, UserRoles.Student);

        _userManager.CreateAsync(student8, password).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(student8, UserRoles.Student);

        _userManager.CreateAsync(student9, password).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(student9, UserRoles.Student);

        _userManager.CreateAsync(student10, password).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(student10, UserRoles.Student);

        _userManager.CreateAsync(student11, password).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(student11, UserRoles.Student);

        _userManager.CreateAsync(student12, password).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(student12, UserRoles.Student);

        _userManager.CreateAsync(teacher1, password).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(teacher1, UserRoles.Teacher);

        _userManager.CreateAsync(teacher2, password).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(teacher2, UserRoles.Teacher);

        _userManager.CreateAsync(teacher3, password).GetAwaiter().GetResult();
        _userManager.AddToRoleAsync(teacher3, UserRoles.Teacher);

        #endregion

        #region Seed_Subjects

        var subject1 = new Subject
        {
            Name = "SubjectName1",
            Key = Guid.NewGuid().ToString("N"),
            AppUsers = new List<AppUserSubject>()
            {
                new AppUserSubject
                {
                    IsTeacher = true,
                    IsAdmin = true,
                    AppUserId = teacher1.Id
                },
                new AppUserSubject
                {
                    IsTeacher = true,
                    IsAdmin = false,
                    AppUserId = teacher2.Id
                }
            }
        };

        var subject2 = new Subject
        {
            Name = "SubjectName2",
            Key = Guid.NewGuid().ToString("N"),
            AppUsers = new List<AppUserSubject>()
            {
                new AppUserSubject
                {
                    IsTeacher = true,
                    IsAdmin = true,
                    AppUserId = teacher2.Id
                }
            }
        };

        var subject3 = new Subject
        {
            Name = "SubjectName3",
            Key = Guid.NewGuid().ToString("N"),
            AppUsers = new List<AppUserSubject>()
            {
                new AppUserSubject
                {
                    IsTeacher = true,
                    IsAdmin = true,
                    AppUserId = teacher3.Id
                }
            }
        };

        _context.Subjects.Add(subject1);
        _context.Subjects.Add(subject2);
        _context.Subjects.Add(subject3);
        _context.SaveChanges();

        #endregion

        #region Seed_StudentSubjects

        //subject1
        _context.AppUserSubjects.AddRange(new List<AppUserSubject>()
        {
            new() { Grade = 99, AppUserId = student1.Id, SubjectId = subject1.Id },
            new() { Grade = 88, AppUserId = student2.Id, SubjectId = subject1.Id },
            new() { Grade = 87, AppUserId = student3.Id, SubjectId = subject1.Id },
            new() { Grade = 88, AppUserId = student4.Id, SubjectId = subject1.Id },
            new() { Grade = 85, AppUserId = student5.Id, SubjectId = subject1.Id },
            new() { Grade = 84, AppUserId = student6.Id, SubjectId = subject1.Id },
            new() { Grade = 83, AppUserId = student7.Id, SubjectId = subject1.Id },
            new() { Grade = 98, AppUserId = student8.Id, SubjectId = subject1.Id },
            new() { Grade = 82, AppUserId = student9.Id, SubjectId = subject1.Id },
            new() { Grade = 81, AppUserId = student10.Id, SubjectId = subject1.Id },
            new() { Grade = 99, AppUserId = student11.Id, SubjectId = subject1.Id },
            new() { Grade = 100, AppUserId = student12.Id, SubjectId = subject1.Id }
        });

        _context.AppUserSubjects.AddRange(new List<AppUserSubject>()
        {
            new() { Grade = 65, AppUserId = student1.Id, SubjectId = subject2.Id },
            new() { Grade = 98, AppUserId = student2.Id, SubjectId = subject2.Id },
            new() { Grade = 77, AppUserId = student3.Id, SubjectId = subject2.Id },
            new() { Grade = 64, AppUserId = student4.Id, SubjectId = subject2.Id },
            new() { Grade = 65, AppUserId = student5.Id, SubjectId = subject2.Id },
            new() { Grade = 34, AppUserId = student6.Id, SubjectId = subject2.Id },
            new() { Grade = 88, AppUserId = student7.Id, SubjectId = subject2.Id },
            new() { Grade = 67, AppUserId = student8.Id, SubjectId = subject2.Id },
            new() { Grade = 59, AppUserId = student9.Id, SubjectId = subject2.Id },
            new() { Grade = 100, AppUserId = student10.Id, SubjectId = subject2.Id }
        });

        _context.AppUserSubjects.AddRange(new List<AppUserSubject>()
        {
            new() { Grade = 76, AppUserId = student5.Id, SubjectId = subject3.Id },
            new() { Grade = 87, AppUserId = student6.Id, SubjectId = subject3.Id },
            new() { Grade = 67, AppUserId = student7.Id, SubjectId = subject3.Id },
            new() { Grade = 76, AppUserId = student8.Id, SubjectId = subject3.Id },
            new() { Grade = 65, AppUserId = student9.Id, SubjectId = subject3.Id },
            new() { Grade = 45, AppUserId = student10.Id, SubjectId = subject3.Id },
            new() { Grade = 87, AppUserId = student11.Id, SubjectId = subject3.Id },
            new() { Grade = 55, AppUserId = student12.Id, SubjectId = subject3.Id }
        });

        _context.SaveChanges();

        #endregion

        return;
    }
}