using BizcomTaskMVC.Entities;
using BizcomTaskMVC.Repository.IRepository;
using BizcomTaskMVC.Statics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BizcomTaskMVC.Services;

public class UserService
{
    private readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<AppUser>> GetAllUsersAsync() => await _userManager.Users.ToListAsync();

    public async Task<List<AppUser>> GetAllStudentsAsync()
    {
        var students = new List<AppUser>();
        var users = await _userManager.Users.ToListAsync();

        foreach (var user in users)
        {
            if(await _userManager.IsInRoleAsync(user, UserRoles.Student))
                students.Add(user);
        }

        return students;
    }

    public async Task<List<AppUser>> GetAllTeachersAsync()
    {
        var teachers = new List<AppUser>();
        var users = await _userManager.Users.ToListAsync();

        foreach (var user in users)
        {
            if (await _userManager.IsInRoleAsync(user, UserRoles.Teacher))
                teachers.Add(user);
        }

        return teachers;
    }
}