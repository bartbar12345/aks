using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Tests.UnitTests;

public class GroupServiceUnitTests : BaseUnitTests
{
    private readonly IGroupService _groupService;

    public GroupServiceUnitTests(ApplicationDbContext dbContext, IGroupService groupService)
        : base(dbContext)
    {
        _groupService = groupService;
    }

    [Fact]
    public void GetGroup()
    {
        var group = _groupService.GetGroup(x => x.Name == "PAI");
        Assert.NotNull(group);
    }

    [Fact]
    public void GetGroups()
    {
        var groups = _groupService.GetGroups(x => x.Id >= 1 && x.Id <= 2).ToList();

        Assert.NotNull(groups);
        Assert.NotEmpty(groups);
        Assert.Equal(2, groups.Count);
    }

    [Fact]
    public void GetAllGroups()
    {
        var groups = _groupService.GetGroups().ToList();

        Assert.NotNull(groups);
        Assert.NotEmpty(groups);
        Assert.Equal(3, groups.Count);
    }

    [Fact]
    public void AddGroup()
    {
        var vm = new AddOrUpdateGroupVm { Name = "SK" };

        _groupService.AddOrUpdateGroup(vm);

        Assert.Equal(4, DbContext.Groups.Count());
        Assert.NotNull(_groupService.GetGroup(x => x.Name == "SK"));
    }

    [Fact]
    public void UpdateGroup()
    {
        var vm = new AddOrUpdateGroupVm { Name = "SIiDM", Id = 3 };

        _groupService.AddOrUpdateGroup(vm);

        Assert.NotNull(_groupService.GetGroup(x => x.Name == "SIiDM"));
    }

    [Fact]
    public void AttachStudentToGroup()
    {
        var vm = new AttachDetachStudentToGroupVm { GroupId = 1, StudentId = 7 };

        var student = _groupService.AttachStudentToGroup(vm);

        Assert.True(student.GroupName == "IO");

        var group = _groupService.GetGroup(g => g.Id == vm.GroupId);

        Assert.NotNull(group);
        Assert.NotNull(group.Students.FirstOrDefault(x => x.Id == 7));
    }

    [Fact]
    public void DetachStudentFromGroup()
    {
        var vm = new AttachDetachStudentToGroupVm { GroupId = 1, StudentId = 7 };

        var student = _groupService.DetachStudentFromGroup(vm);

        Assert.NotNull(student);
        Assert.Null(student.GroupName);
    }

    [Fact]
    public void AttachSubjectToGroup()
    {
        var vm = new AttachDetachSubjectGroupVm { GroupId = 1, SubjectId = 4 };

        _groupService.AttachSubjectToGroup(vm);

        var group = _groupService.GetGroup(g => g.Id == vm.GroupId);

        Assert.NotNull(group);
        Assert.NotNull(group.Subjects.FirstOrDefault(s => s.Name == "Administracja Intenetowymi Systemami Baz Danych"));
    }

    [Fact]
    public void DetachSubjectFromGroup()
    {
        var vm = new AttachDetachSubjectGroupVm { GroupId = 2, SubjectId = 4 };

        var group = _groupService.DetachSubjectFromGroup(vm);

        Assert.NotNull(group);
        Assert.Null(group.Subjects.FirstOrDefault(s => s.Name == "Administracja Intenetowymi Systemami Baz Danych"));
    }

    [Fact]
    public void AttachTeacherToSubject()
    {
        var vm = new AttachDetachSubjectToTeacherVm { SubjectId = 5, TeacherId = 2 };

        var subject = _groupService.AttachTeacherToSubject(vm);

        Assert.NotNull(subject);
        Assert.True(subject.TeacherId == vm.TeacherId);
    }

    [Fact]
    public void DetachTeacherToSubject()
    {
        var vm = new AttachDetachSubjectToTeacherVm { SubjectId = 3, TeacherId = 2 };

        var subject = _groupService.DetachTeacherFromSubject(vm);

        Assert.NotNull(subject);
        Assert.Null(subject.TeacherId);
        Assert.Null(subject.TeacherName);
    }
}