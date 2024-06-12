using Homework.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Models.Repositories
{
    public interface IInviteRepository
    {
        Invite Add(Invite invite);
        IEnumerable<Invite> GetAllInvites(int ClassroomId);
        IEnumerable<Invite> GetUserInvites(string email);
        Invite Delete(int Classid, string email);        
    }
    public class SQLInviteRepository : IInviteRepository
    {
        private readonly ApplicationDbContext context;
        public SQLInviteRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        Invite IInviteRepository.Add(Invite invite)
        {
            context.Invites.Add(invite);
            context.SaveChanges();
            return invite;
        }
        IEnumerable<Invite> IInviteRepository.GetAllInvites(int ClassroomId)
        {
            return context.Invites.Where(i => i.ClassroomId == ClassroomId).Include(c => c.Classroom);
        }
        IEnumerable<Invite> IInviteRepository.GetUserInvites(string email)
        {
            return context.Invites.Where(invite => invite.Email == email).Include(c => c.Classroom);
        }

        Invite IInviteRepository.Delete(int Classid, string email)
        {
            Invite invite = context.Invites.Where(i => i.Email == email && i.ClassroomId == Classid).FirstOrDefault();
            if (invite != null)
            {
                context.Invites.Remove(invite);
                context.SaveChanges();
            }
            return invite;
        }
    }
}
