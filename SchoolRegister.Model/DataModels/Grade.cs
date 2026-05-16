using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolRegister.Model.Enums;

namespace SchoolRegister.Model.DataModels;

public class Grade
{
    [Key]
    public int Id { get; set; }

    public DateTime DateOfIssue { get; set; } = DateTime.Now;

    public GradeScale GradeValue { get; set; }

    public virtual Subject Subject { get; set; } = null!;

    [ForeignKey("Subject")]
    public int SubjectId { get; set; }

    public virtual Student Student { get; set; } = null!;

    [ForeignKey("Student")]
    public int StudentId { get; set; }
}