namespace SchoolRegister.Model.DataModels;

public class Student : User
{
    public Group Group { get; set; } = null!;

    public int GroupId { get; set; }

    public List<Grade> Grades { get; set; } = new();

    public Parent Parent { get; set; } = null!;

    public int ParentId { get; set; }

    public double AverageGrade
    {
        get
        {
            if (Grades.Count == 0)
                return 0;

            return Grades.Average(g => (int)g.GradeValue);
        }
    }

    public Dictionary<string, double> AverageGradePerSubject
    {
        get
        {
            return Grades
                .GroupBy(g => g.Subject.Name)
                .ToDictionary(
                    g => g.Key,
                    g => g.Average(x => (int)x.GradeValue)
                );
        }
    }

    public Dictionary<string, List<Grade>> GradesPerSubject
    {
        get
        {
            return Grades
                .GroupBy(g => g.Subject.Name)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToList()
                );
        }
    }
}