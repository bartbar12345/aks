using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Model.Enums;

namespace SchoolRegister.Tests;

public static class Extensions
{
    public static async void SeedData(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

        if (dbContext.Users.Any())
            return;

        var teacherRole = new Role { Id = 3, Name = "Teacher", NormalizedName = "TEACHER", RoleValue = RoleValue.Teacher };
        var studentRole = new Role { Id = 1, Name = "Student", NormalizedName = "STUDENT", RoleValue = RoleValue.Student };
        var parentRole = new Role { Id = 2, Name = "Parent", NormalizedName = "PARENT", RoleValue = RoleValue.Parent };
        var adminRole = new Role { Id = 4, Name = "Admin", NormalizedName = "ADMIN", RoleValue = RoleValue.Admin };

        await roleManager.CreateAsync(teacherRole);
        await roleManager.CreateAsync(studentRole);
        await roleManager.CreateAsync(parentRole);
        await roleManager.CreateAsync(adminRole);

        await dbContext.Groups.AddRangeAsync(
            new Group { Id = 1, Name = "IO" },
            new Group { Id = 2, Name = "PAI" },
            new Group { Id = 3, Name = "AIP Erasmus" }
        );

        var password = "User1234";

        var t1 = new Teacher { Id = 1, FirstName = "Adam", LastName = "Bednarski", UserName = "t1@eg.eg", Email = "t1@eg.eg", Title = "mgr inż." };
        var t2 = new Teacher { Id = 2, FirstName = "Jan", LastName = "Nowak", UserName = "t2@eg.eg", Email = "t2@eg.eg", Title = "mgr" };
        var t3 = new Teacher { Id = 12, FirstName = "Stanisław", LastName = "Nowakowski", UserName = "t11@eg.eg", Email = "t11@eg.eg", Title = "mgr inż." };

        await userManager.CreateAsync(t1, password);
        await userManager.CreateAsync(t2, password);
        await userManager.CreateAsync(t3, password);
        await userManager.AddToRoleAsync(t1, "Teacher");
        await userManager.AddToRoleAsync(t2, "Teacher");
        await userManager.AddToRoleAsync(t3, "Teacher");

        var p1 = new Parent { Id = 3, FirstName = "Zbigniew", LastName = "Kowalski", UserName = "p1@eg.eg", Email = "p1@eg.eg" };
        var p2 = new Parent { Id = 4, FirstName = "Anna", LastName = "Nowakowska", UserName = "p2@eg.eg", Email = "p2@eg.eg" };

        await userManager.CreateAsync(p1, password);
        await userManager.CreateAsync(p2, password);
        await userManager.AddToRoleAsync(p1, "Parent");
        await userManager.AddToRoleAsync(p2, "Parent");

        var s1 = new Student { Id = 5, FirstName = "Tomasz", LastName = "Kowalski", UserName = "s1@eg.eg", Email = "s1@eg.eg", GroupId = 1, ParentId = 3 };
        var s2 = new Student { Id = 6, FirstName = "Krzysztof", LastName = "Kowalski", UserName = "s2@eg.eg", Email = "s2@eg.eg", GroupId = 1, ParentId = 3 };
        var s3 = new Student { Id = 7, FirstName = "Natalia", LastName = "Kowalska", UserName = "s3@eg.eg", Email = "s3@eg.eg", GroupId = 2, ParentId = 3 };
        var s4 = new Student { Id = 8, FirstName = "Magdalena", LastName = "Wiśniewska", UserName = "s4@eg.eg", Email = "s4@eg.eg", GroupId = 2, ParentId = 4 };
        var s5 = new Student { Id = 9, FirstName = "Jan", LastName = "Wiśniewski", UserName = "s5@eg.eg", Email = "s5@eg.eg", GroupId = 3, ParentId = 4 };
        var s6 = new Student { Id = 10, FirstName = "Krystian", LastName = "Wiśniewski", UserName = "s6@eg.eg", Email = "s6@eg.eg", GroupId = 3, ParentId = 4 };

        foreach (var student in new[] { s1, s2, s3, s4, s5, s6 })
        {
            await userManager.CreateAsync(student, password);
            await userManager.AddToRoleAsync(student, "Student");
        }

        var admin = new User { Id = 11, FirstName = "Jacek", LastName = "Kowalczyk", UserName = "a1@eg.eg", Email = "a1@eg.eg" };
        await userManager.CreateAsync(admin, password);
        await userManager.AddToRoleAsync(admin, "Admin");

        await dbContext.Subjects.AddRangeAsync(
            new Subject { Id = 1, Name = "Aplikacje WWW", Description = "Aplikacje webowe", TeacherId = 1 },
            new Subject { Id = 2, Name = "Programowanie obiektowe", Description = "Programowanie obiektowe", TeacherId = 1 },
            new Subject { Id = 3, Name = "Advanced Internet Programming", Description = "AIP", TeacherId = 2 },
            new Subject { Id = 4, Name = "Administracja Intenetowymi Systemami Baz Danych", Description = "AISBD", TeacherId = 2 },
            new Subject { Id = 5, Name = "Programowanie interaktywnej grafiki dla stron WWW", Description = "Grafika WWW", TeacherId = 12 }
        );

        await dbContext.SubjectGroups.AddRangeAsync(
            new SubjectGroup { SubjectId = 1, GroupId = 1 },
            new SubjectGroup { SubjectId = 1, GroupId = 2 },
            new SubjectGroup { SubjectId = 2, GroupId = 1 },
            new SubjectGroup { SubjectId = 2, GroupId = 2 },
            new SubjectGroup { SubjectId = 2, GroupId = 3 },
            new SubjectGroup { SubjectId = 3, GroupId = 3 },
            new SubjectGroup { SubjectId = 4, GroupId = 2 },
            new SubjectGroup { SubjectId = 4, GroupId = 3 }
        );

        await dbContext.Grades.AddAsync(new Grade
        {
            Id = 1,
            DateOfIssue = new DateTime(2019, 03, 21, 17, 46, 38),
            StudentId = 5,
            SubjectId = 1,
            GradeValue = GradeScale.DB
        });

        await dbContext.SaveChangesAsync();
    }
}