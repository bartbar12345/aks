using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.ConcreteServices;

public class GradeService : BaseService, IGradeService
{
    private readonly UserManager<User> _userManager;

    public GradeService(
        ApplicationDbContext dbContext,
        IMapper mapper,
        ILogger logger,
        UserManager<User> userManager)
        : base(dbContext, mapper, logger)
    {
        _userManager = userManager;
    }

    public GradeVm AddGradeToStudent(AddGradeToStudentVm addGradeToStudentVm)
    {
        var teacher = DbContext.Users
            .OfType<Teacher>()
            .FirstOrDefault(t => t.Id == addGradeToStudentVm.TeacherId);

        if (teacher == null)
            throw new Exception("Teacher not found.");

        var isTeacher = _userManager.IsInRoleAsync(teacher, "Teacher").Result;

        if (!isTeacher)
            throw new UnauthorizedAccessException("User is not a teacher.");

        var grade = Mapper.Map<Grade>(addGradeToStudentVm);
        grade.DateOfIssue = DateTime.Now;

        DbContext.Grades.Add(grade);
        DbContext.SaveChanges();

        return Mapper.Map<GradeVm>(grade);
    }

    public GradesReportVm GetGradesReportForStudent(GetGradesReportVm getGradesVm)
    {
        var student = DbContext.Users
            .OfType<Student>()
            .FirstOrDefault(s => s.Id == getGradesVm.StudentId);

        if (student == null)
            throw new Exception("Student not found.");

        var getter = DbContext.Users.FirstOrDefault(u => u.Id == getGradesVm.GetterUserId);

        if (getter == null)
            throw new Exception("Getter user not found.");

        var isTeacher = _userManager.IsInRoleAsync(getter, "Teacher").Result;
        var isStudent = _userManager.IsInRoleAsync(getter, "Student").Result;
        var isParent = _userManager.IsInRoleAsync(getter, "Parent").Result;

        var canSee =
            isTeacher ||
            isStudent && getter.Id == student.Id ||
            isParent && student.ParentId == getter.Id;

        if (!canSee)
            throw new UnauthorizedAccessException("User cannot see this grades report.");

        return new GradesReportVm
        {
            StudentId = student.Id,
            StudentName = $"{student.FirstName} {student.LastName}",
            AverageGrade = student.AverageGrade,
            AverageGradePerSubject = student.AverageGradePerSubject,
            GradesPerSubject = student.GradesPerSubject
        };
    }
}