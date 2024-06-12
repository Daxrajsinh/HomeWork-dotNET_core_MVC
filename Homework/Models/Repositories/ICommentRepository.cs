using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homework.Data;
using Homework.Models;
using Microsoft.EntityFrameworkCore;
namespace Homework.Models.Repositories
{
    public interface ICommentRepository
    {
        IEnumerable<Comment> GetBlackBoardComments(int BlackBoardId);
        Comment Add(Comment Comment);
        Comment Update(Comment CommentChanges);
        Comment Delete(int ID);
    }
    public class SQLCommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext context;
        public SQLCommentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        Comment ICommentRepository.Add(Comment Comment)
        {
            context.Comments.Add(Comment);
            context.SaveChanges();
            return Comment;
        }
        Comment ICommentRepository.Delete(int Id)
        {
            Comment Comment = context.Comments.Find(Id);
            if (Comment != null)
            {
                context.Comments.Remove(Comment);
                context.SaveChanges();
            }
            return Comment;
        }
        IEnumerable<Comment> ICommentRepository.GetBlackBoardComments(int BlackBoardId)
        {
            return context.Comments.Where(c => c.BlackBoardId == BlackBoardId).Include(s => s.AppUser).Include(s => s.BlackBoard);
        }
        Comment ICommentRepository.Update(Comment CommentChanges)
        {
            var Comment = context.Comments.Attach(CommentChanges);
            Comment.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return CommentChanges;
        }
    }
}