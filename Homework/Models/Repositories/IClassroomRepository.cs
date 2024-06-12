using Homework.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Models.Repositories
{
    public interface IClassroomRepository
    {
        Classroom GetClassroom(int ID);
        IEnumerable<Classroom> GetAllClassrooms();
        Classroom Add(Classroom classroom);
        Classroom Update(Classroom classroomChanges);
        Classroom Delete(int ID);
    }
    public class SQLClassroomRepository : IClassroomRepository
    {
        private readonly ApplicationDbContext context;
        public SQLClassroomRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        Classroom IClassroomRepository.Add(Classroom classroom)
        {
            context.Classrooms.Add(classroom);
            context.SaveChanges();
            return classroom;
        }
        Classroom IClassroomRepository.Delete(int Id)
        {
            Classroom Classroom = context.Classrooms.Find(Id);
            if (Classroom != null)
            {
                context.Classrooms.Remove(Classroom);
                context.SaveChanges();
            }
            return Classroom;
        }
        IEnumerable<Classroom> IClassroomRepository.GetAllClassrooms()
        {
            return context.Classrooms.Include(s => s.AppUser);
        }
        Classroom IClassroomRepository.GetClassroom(int Id)
        {
            return context.Classrooms.Find(Id);
        }
        Classroom IClassroomRepository.Update(Classroom classroomChanges)
        {
            var Classroom = context.Classrooms.Attach(classroomChanges);
            Classroom.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return classroomChanges;
        }
    }
}
