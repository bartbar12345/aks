using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Tests.UnitTests;

public class TeacherServiceUnitTests : BaseUnitTests
{
    private readonly ITeacherService _teacherService;

    public TeacherServiceUnitTests(ApplicationDbContext dbContext, ITeacherService teacherService)
        : base(dbContext)
    {
        _teacherService = teacherService;
    }

    [Fact]
    public void GetTeacher()
    {
        var teacher = _teacherService.GetTeacher(x => x.UserName == "t1@eg.eg");
        Assert.NotNull(teacher);
    }

    [Fact]
    public void GetTeachers()
    {
        var teachers = _teacherService.GetTeachers(x => x.UserName!.Contains("@eg.eg")).ToList();

        Assert.NotNull(teachers);
        Assert.NotEmpty(teachers);
        Assert.Equal(3, teachers.Count);
    }

    [Fact]
    public void GetAllTeachers()
    {
        var teachers = _teacherService.GetTeachers().ToList();

        Assert.NotNull(teachers);
        Assert.NotEmpty(teachers);
        Assert.Equal(3, teachers.Count);
    }

    [Fact]
    public void GetTeachersGroups()
    {
        var vm = new TeachersGroupsVm { TeacherId = 1 };

        var groups = _teacherService.GetTeachersGroups(vm).ToList();

        Assert.NotNull(groups);
        Assert.NotEmpty(groups);
        Assert.Equal(5, groups.Count);
    }
}