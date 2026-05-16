using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System.Linq.Expressions;

namespace SchoolRegister.Services.ConcreteServices;

public class TeacherService : BaseService, ITeacherService
{
    private readonly UserManager<User> _userManager;

    public TeacherService(
        ApplicationDbContext dbContext,
        IMapper mapper,
        ILogger logger,
        UserManager<User> userManager)
        : base(dbContext, mapper, logger)
    {
        _userManager = userManager;
    }

    public TeacherVm GetTeacher(Expression<Func<Teacher, bool>> filterPredicate)
    {
        var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(filterPredicate);
        return Mapper.Map<TeacherVm>(teacher);
    }

    public IEnumerable<TeacherVm> GetTeachers(Expression<Func<Teacher, bool>>? filterPredicate = null)
    {
        var teachers = DbContext.Users.OfType<Teacher>().AsQueryable();

        if (filterPredicate != null)
            teachers = teachers.Where(filterPredicate);

        return Mapper.Map<IEnumerable<TeacherVm>>(teachers);
    }

    public IEnumerable<GroupVm> GetTeachersGroups(TeachersGroupsVm getTeachersGroups)
    {
        var groups = DbContext.SubjectGroups
            .Where(sg => sg.Subject.TeacherId == getTeachersGroups.TeacherId)
            .Select(sg => sg.Group)
            .ToList();

        return Mapper.Map<IEnumerable<GroupVm>>(groups);
    }
}