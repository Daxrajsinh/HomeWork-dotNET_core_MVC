using Homework.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Models.Repositories
{
    public interface IClassroomUserRepository
    {
        ClassroomUser Add(ClassroomUser classroomUser);
        ClassroomUser GetClassroomUser(int classId, string userId);
        IEnumerable<ClassroomUser> GetClassroomMentors(int id);
        IEnumerable<ClassroomUser> GetClassroomStudents(int id);
        IEnumerable<ClassroomUser> GetUserClassrooms(string userId);
        ClassroomUser Delete(int classId, string userId);
    }
    public class SQLClassroomUserRepository : IClassroomUserRepository
    {
        private readonly ApplicationDbContext context;
        public SQLClassroomUserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        ClassroomUser IClassroomUserRepository.Add(ClassroomUser classroomUser)
        {
            context.ClassroomUsers.Add(classroomUser);
            context.SaveChanges();
            return classroomUser;
        }
        ClassroomUser IClassroomUserRepository.GetClassroomUser(int classId, string userId)
        {
            return context.ClassroomUsers.Find(classId, userId);
        }
        IEnumerable<ClassroomUser> IClassroomUserRepository.GetClassroomMentors(int id)
        {
            return context.ClassroomUsers.Where(cu => cu.Role == "Mentor" && cu.ClassroomId == id).Include(au => au.AppUser);
        }
        IEnumerable<ClassroomUser> IClassroomUserRepository.GetClassroomStudents(int id)
        {
            return context.ClassroomUsers.Where(cu => cu.Role == "Student" && cu.ClassroomId == id).Include(au => au.AppUser);
        }
        IEnumerable<ClassroomUser> IClassroomUserRepository.GetUserClassrooms(string userId)
        {
            return context.ClassroomUsers.Where(cu => cu.AppUserId == userId).Include(cu => cu.Classroom).Include(cu => cu.AppUser);
        }
        ClassroomUser IClassroomUserRepository.Delete(int classId, string userId)
        {
            ClassroomUser ClassroomUser = context.ClassroomUsers.Find(classId, userId);
            if (ClassroomUser != null)
            {
                context.ClassroomUsers.Remove(ClassroomUser);
                context.SaveChanges();
            }
            return ClassroomUser;
        }
    }
}
