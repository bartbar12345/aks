using SchoolRegister.DAL.EF;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Tests.UnitTests;

public class SubjectServiceUnitTests : BaseUnitTests
{
    private readonly ISubjectService _subjectService;

    public SubjectServiceUnitTests(ISubjectService subjectService, ApplicationDbContext dbContext)
        : base(dbContext)
    {
        _subjectService = subjectService;
    }

    [Fact]
    public void GetSubject()
    {
        var subject = _subjectService.GetSubject(x => x.Name == "Programowanie obiektowe");

        Assert.NotNull(subject);
    }

    [Fact]
    public void GetSubjects()
    {
        var subjects = _subjectService.GetSubjects(x => x.Id > 2 && x.Id <= 4).ToList();

        Assert.NotNull(subjects);
        Assert.NotEmpty(subjects);
        Assert.Equal(2, subjects.Count);
    }

    [Fact]
    public void GetAllSubjects()
    {
        var subjects = _subjectService.GetSubjects().ToList();

        Assert.NotNull(subjects);
        Assert.NotEmpty(subjects);
        Assert.Equal(DbContext.Subjects.Count(), subjects.Count);
    }

    [Fact]
    public void AddNewSubject()
    {
        var vm = new AddOrUpdateSubjectVm
        {
            Name = "Zaawansowane programowanie internetowe",
            Description = "W ramach przedmiotu studenci tworzą rozwiazania w bibliotekach SPA",
            TeacherId = 1
        };

        var createdSubject = _subjectService.AddOrUpdateSubject(vm);

        Assert.NotNull(createdSubject);
        Assert.Equal("Zaawansowane programowanie internetowe", createdSubject.Name);
    }

    [Fact]
    public void EditSubject()
    {
        var vm = new AddOrUpdateSubjectVm
        {
            Id = 1,
            Name = "Aplikacje webowe",
            Description = null,
            TeacherId = 1
        };

        var editedSubject = _subjectService.AddOrUpdateSubject(vm);

        Assert.NotNull(editedSubject);
        Assert.Equal("Aplikacje webowe", editedSubject.Name);
        Assert.Null(editedSubject.Description);
    }
}