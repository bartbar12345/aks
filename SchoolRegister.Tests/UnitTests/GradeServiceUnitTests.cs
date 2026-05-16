using SchoolRegister.DAL.EF;
using SchoolRegister.Model.Enums;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Tests.UnitTests;

public class GradeServiceUnitTests : BaseUnitTests
{
    private readonly IGradeService _gradeService;

    public GradeServiceUnitTests(ApplicationDbContext dbContext, IGradeService gradeService)
        : base(dbContext)
    {
        _gradeService = gradeService;
    }

    [Fact]
    public void AddGradeToStudent()
    {
        var gradeVm = new AddGradeToStudentVm
        {
            StudentId = 5,
            SubjectId = 1,
            GradeValue = GradeScale.DB,
            TeacherId = 1
        };

        var grade = _gradeService.AddGradeToStudent(gradeVm);

        Assert.NotNull(grade);
        Assert.Equal(2, DbContext.Grades.Count());
    }

    [Fact]
    public void GetGradesReportForStudentByTeacher()
    {
        var vm = new GetGradesReportVm { StudentId = 5, GetterUserId = 1 };

        var report = _gradeService.GetGradesReportForStudent(vm);

        Assert.NotNull(report);
    }

    [Fact]
    public void GetGradesReportForStudentByStudent()
    {
        var vm = new GetGradesReportVm { StudentId = 5, GetterUserId = 5 };

        var report = _gradeService.GetGradesReportForStudent(vm);

        Assert.NotNull(report);
    }

    [Fact]
    public void GetGradesReportForStudentByParent()
    {
        var vm = new GetGradesReportVm { StudentId = 5, GetterUserId = 3 };

        var report = _gradeService.GetGradesReportForStudent(vm);

        Assert.NotNull(report);
    }
}