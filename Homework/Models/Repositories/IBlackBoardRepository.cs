using Homework.Data;
using Homework.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homework.Models.Repositories
{
    public interface IBlackBoardRepository
    {
        BlackBoard GetBlackBoard(int Id);
        BlackBoard Add(BlackBoard blackBoard);
        IEnumerable<BlackBoard> GetAllBlackBoards();
        IEnumerable<BlackBoard> GetClassBlackBoards(int Id);
        BlackBoard Delete(int Id);
    }
    public class SQLBlackBoardRepository : IBlackBoardRepository
    {
        private readonly ApplicationDbContext context;
        public SQLBlackBoardRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        BlackBoard IBlackBoardRepository.GetBlackBoard(int Id)
        {
            return context.BlackBoards.Find(Id);
        }
        BlackBoard IBlackBoardRepository.Add(BlackBoard blackBoard)
        {
            context.BlackBoards.Add(blackBoard);
            context.SaveChanges();
            return blackBoard;
        }
        IEnumerable<BlackBoard> IBlackBoardRepository.GetAllBlackBoards()
        {
            return context.BlackBoards.Include(s => s.ClassroomUser);
        }
        IEnumerable<BlackBoard> IBlackBoardRepository.GetClassBlackBoards(int Id)
        {
            return context.BlackBoards.Where(bb => bb.ClassroomId == Id).Include(s => s.ClassroomUser);
        }
        BlackBoard IBlackBoardRepository.Delete(int Id)
        {
            BlackBoard BlackBoard = context.BlackBoards.Find(Id);
            if (BlackBoard != null)
            {
                context.BlackBoards.Remove(BlackBoard);
                context.SaveChanges();
            }
            return BlackBoard;
        }
    }
}