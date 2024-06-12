using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homework.Data;
using Microsoft.EntityFrameworkCore;
namespace Homework.Models.Repositories
{
    public interface IAssignmentRepository
    {
        Assignment GetAssignment(int ID);
        IEnumerable<Assignment> GetClassAssignments(int ClassId);
        Assignment Add(Assignment Assignment);
        Assignment Update(Assignment AssignmentChanges);
        Assignment Delete(int ID);
    }
    public class SQLAssignmentRepository : IAssignmentRepository
    {
        private readonly ApplicationDbContext context;
        public SQLAssignmentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        Assignment IAssignmentRepository.Add(Assignment Assignment)
        {
            context.Assignments.Add(Assignment);
            context.SaveChanges();
            return Assignment;
        }
        Assignment IAssignmentRepository.Delete(int Id)
        {
            Assignment Assignment = context.Assignments.Find(Id);
            if (Assignment != null)
            {
                context.Assignments.Remove(Assignment);
                context.SaveChanges();
            }
            return Assignment;
        }
        IEnumerable<Assignment> IAssignmentRepository.GetClassAssignments(int ClassId)
        {
            return context.Assignments.Where(a => a.ClassroomID == ClassId).Include(c => c.Classroom);
        }
        Assignment IAssignmentRepository.GetAssignment(int Id)
        {
            return context.Assignments.Find(Id);
        }
        Assignment IAssignmentRepository.Update(Assignment AssignmentChanges)
        {
            var Assignment = context.Assignments.Attach(AssignmentChanges);
            Assignment.State = EntityState.Modified;
            context.SaveChanges();
            return AssignmentChanges;
        }
    }
}
