using SchoolRegister.Model.Enums;

namespace SchoolRegister.ViewModels.VM;

public class GradeVm
{
    public int Id { get; set; }

    public DateTime DateOfIssue { get; set; }

    public GradeScale GradeValue { get; set; }

    public int StudentId { get; set; }

    public int SubjectId { get; set; }

    public string? SubjectName { get; set; }

    public string? StudentName { get; set; }
}