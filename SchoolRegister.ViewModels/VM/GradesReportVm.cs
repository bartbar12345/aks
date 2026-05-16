using SchoolRegister.Model.Enums;

namespace SchoolRegister.ViewModels.VM;

public class GradesReportVm
{
    public int StudentId { get; set; }

    public string StudentName { get; set; } = null!;

    public double AverageGrade { get; set; }

    public IDictionary<string, double> AverageGradePerSubject { get; set; } = new Dictionary<string, double>();

    public IDictionary<string, List<GradeScale>> GradesPerSubject { get; set; } = new Dictionary<string, List<GradeScale>>();
}