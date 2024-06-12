using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homework.Data;
using Microsoft.EntityFrameworkCore;
namespace Homework.Models.Repositories
{
    public interface ISubmittedAssignmentRepository
    {
        SubmittedAssignment GetSubmittedAssignment(int ID);
        IEnumerable<SubmittedAssignment> GetSubmittedAssignments(int assignmentId);
        IEnumerable<SubmittedAssignment> GetUserSubmittedAssignments(int assignmentId, string userId);
        IEnumerable<ClassroomUser> GetPeopleNotSubmitted(int assignmentId);
        SubmittedAssignment Add(SubmittedAssignment SubmittedAssignment);
        SubmittedAssignment Update(SubmittedAssignment SubmittedAssignmentChanges);
        SubmittedAssignment Delete(int ID);
    }
    public class SQLSubmittedAssignmentRepository : ISubmittedAssignmentRepository
    {
        private readonly ApplicationDbContext context;
        public SQLSubmittedAssignmentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        SubmittedAssignment ISubmittedAssignmentRepository.Add(SubmittedAssignment SubmittedAssignment)
        {
            context.SubmittedAssignments.Add(SubmittedAssignment);
            context.SaveChanges();
            return SubmittedAssignment;
        }
        SubmittedAssignment ISubmittedAssignmentRepository.Delete(int Id)
        {
            SubmittedAssignment SubmittedAssignment = context.SubmittedAssignments.Find(Id);
            if (SubmittedAssignment != null)
            {
                context.SubmittedAssignments.Remove(SubmittedAssignment);
                context.SaveChanges();
            }
            return SubmittedAssignment;
        }
        IEnumerable<SubmittedAssignment> ISubmittedAssignmentRepository.GetSubmittedAssignments(int assignmentId)
        {
            return context.SubmittedAssignments.Where(a => a.AssignmentID == assignmentId).Include(a => a.Assignment);
        }

        IEnumerable<SubmittedAssignment> ISubmittedAssignmentRepository.GetUserSubmittedAssignments(int assignmentId, string userId)
        { 
            return context.SubmittedAssignments.Where(a => a.AssignmentID == assignmentId && a.AppUserID == userId).Include(a => a.Assignment);
        }
        IEnumerable<ClassroomUser> ISubmittedAssignmentRepository.GetPeopleNotSubmitted(int assignmentId)
        {
            Assignment assignment = context.Assignments.Find(assignmentId);
            List<string> submittedStudentsIDs = context.SubmittedAssignments.Where(c => c.AssignmentID == assignmentId)
                .Select(c => c.AppUserID)
                .ToList();
            IEnumerable<ClassroomUser> notSubmitted = context.ClassroomUsers
                .Where(cu => cu.Role=="Student" 
                && cu.ClassroomId == assignment.ClassroomID 
                && !submittedStudentsIDs.Contains(cu.AppUserId))
                .Include(au => au.AppUser); 
            return notSubmitted;
        }

        SubmittedAssignment ISubmittedAssignmentRepository.GetSubmittedAssignment(int Id)
        {
            return context.SubmittedAssignments.Find(Id);
        }
        SubmittedAssignment ISubmittedAssignmentRepository.Update(SubmittedAssignment SubmittedAssignmentChanges)
        {
            var SubmittedAssignment = context.SubmittedAssignments.Attach(SubmittedAssignmentChanges);
            SubmittedAssignment.State = EntityState.Modified;
            context.SaveChanges();
            return SubmittedAssignmentChanges;
        }
    }
}
