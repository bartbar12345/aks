using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System.Linq.Expressions;

namespace SchoolRegister.Services.ConcreteServices;

public class GroupService : BaseService, IGroupService
{
    private readonly UserManager<User> _userManager;

    public GroupService(
        ApplicationDbContext dbContext,
        IMapper mapper,
        ILogger logger,
        UserManager<User> userManager)
        : base(dbContext, mapper, logger)
    {
        _userManager = userManager;
    }

    public GroupVm AddOrUpdateGroup(AddOrUpdateGroupVm addOrUpdateGroupVm)
    {
        var group = Mapper.Map<Group>(addOrUpdateGroupVm);

        if (!addOrUpdateGroupVm.Id.HasValue || addOrUpdateGroupVm.Id == 0)
            DbContext.Groups.Add(group);
        else
            DbContext.Groups.Update(group);

        DbContext.SaveChanges();

        return Mapper.Map<GroupVm>(group);
    }

    public StudentVm AttachStudentToGroup(AttachDetachStudentToGroupVm attachStudentToGroupVm)
    {
        var student = DbContext.Users
            .OfType<Student>()
            .FirstOrDefault(s => s.Id == attachStudentToGroupVm.StudentId);

        if (student == null)
            throw new Exception("Student not found.");

        student.GroupId = attachStudentToGroupVm.GroupId;

        DbContext.SaveChanges();

        return Mapper.Map<StudentVm>(student);
    }

    public StudentVm DetachStudentFromGroup(AttachDetachStudentToGroupVm detachStudentToGroupVm)
    {
        var student = DbContext.Users
            .OfType<Student>()
            .FirstOrDefault(s => s.Id == detachStudentToGroupVm.StudentId);

        if (student == null)
            throw new Exception("Student not found.");

        student.GroupId = null;

        DbContext.SaveChanges();

        return Mapper.Map<StudentVm>(student);
    }

    public GroupVm AttachSubjectToGroup(AttachDetachSubjectGroupVm attachSubjectGroupVm)
    {
        var exists = DbContext.SubjectGroups.Any(sg =>
            sg.SubjectId == attachSubjectGroupVm.SubjectId &&
            sg.GroupId == attachSubjectGroupVm.GroupId);

        if (!exists)
        {
            DbContext.SubjectGroups.Add(new SubjectGroup
            {
                SubjectId = attachSubjectGroupVm.SubjectId,
                GroupId = attachSubjectGroupVm.GroupId
            });

            DbContext.SaveChanges();
        }

        var group = DbContext.Groups.First(g => g.Id == attachSubjectGroupVm.GroupId);
        return Mapper.Map<GroupVm>(group);
    }

    public GroupVm DetachSubjectFromGroup(AttachDetachSubjectGroupVm detachSubjectGroupVm)
    {
        var subjectGroup = DbContext.SubjectGroups.FirstOrDefault(sg =>
            sg.SubjectId == detachSubjectGroupVm.SubjectId &&
            sg.GroupId == detachSubjectGroupVm.GroupId);

        if (subjectGroup != null)
        {
            DbContext.SubjectGroups.Remove(subjectGroup);
            DbContext.SaveChanges();
        }

        var group = DbContext.Groups.First(g => g.Id == detachSubjectGroupVm.GroupId);
        return Mapper.Map<GroupVm>(group);
    }

    public SubjectVm AttachTeacherToSubject(AttachDetachSubjectToTeacherVm attachSubjectToTeacherVm)
    {
        var subject = DbContext.Subjects.FirstOrDefault(s => s.Id == attachSubjectToTeacherVm.SubjectId);

        if (subject == null)
            throw new Exception("Subject not found.");

        subject.TeacherId = attachSubjectToTeacherVm.TeacherId;

        DbContext.SaveChanges();

        return Mapper.Map<SubjectVm>(subject);
    }

    public SubjectVm DetachTeacherFromSubject(AttachDetachSubjectToTeacherVm detachSubjectToTeacherVm)
    {
        var subject = DbContext.Subjects.FirstOrDefault(s => s.Id == detachSubjectToTeacherVm.SubjectId);

        if (subject == null)
            throw new Exception("Subject not found.");

        subject.TeacherId = null;

        DbContext.SaveChanges();

        return Mapper.Map<SubjectVm>(subject);
    }

    public GroupVm GetGroup(Expression<Func<Group, bool>> filterPredicate)
    {
        var group = DbContext.Groups.FirstOrDefault(filterPredicate);
        return Mapper.Map<GroupVm>(group);
    }

    public IEnumerable<GroupVm> GetGroups(Expression<Func<Group, bool>>? filterPredicate = null)
    {
        var groups = DbContext.Groups.AsQueryable();

        if (filterPredicate != null)
            groups = groups.Where(filterPredicate);

        return Mapper.Map<IEnumerable<GroupVm>>(groups);
    }
}