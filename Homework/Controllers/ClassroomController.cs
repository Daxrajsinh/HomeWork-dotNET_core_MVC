using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Homework.Models;
using Homework.Models.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MailKit.Net.Smtp;
using MimeKit;
using System.Security.Authentication;
using Homework.Models.Repositories;
using Homework.Models;
using Homework.ViewModels;

namespace Homework.Controllers
{
    public class ClassroomController : Controller
    {
        private readonly IClassroomRepository _classRepo;
        private readonly IClassroomUserRepository _classUserRepo;
        private readonly IBlackBoardRepository _boardRepo;
        private readonly IInviteRepository _inviteRepo;
        private readonly IAssignmentRepository _assignmentRepo;
        private readonly ISubmittedAssignmentRepository _submittedAssignmentRepo;
        private readonly ICommentRepository _commentRepo;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        //private readonly System.Web.Mvc.HtmlHelper _htmlHelper;
        public ClassroomController(IClassroomRepository classRepo, IClassroomUserRepository classUser,
            IBlackBoardRepository boardRepo, IInviteRepository inviteRepo, IAssignmentRepository assignmentRepo,
            ISubmittedAssignmentRepository submittedAssignmentRepo,
            ICommentRepository commentRepo,
            IHostingEnvironment hostingEnvironment, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _classRepo = classRepo;
            _classUserRepo = classUser;
            _boardRepo = boardRepo;
            _inviteRepo = inviteRepo;
            _assignmentRepo = assignmentRepo;
            _submittedAssignmentRepo = submittedAssignmentRepo;
            _commentRepo = commentRepo;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public ViewResult Index()
        {
            var model = _classRepo.GetAllClassrooms();
            return View(model);
        }
        public ViewResult Details(int id)
        {
            Classroom Classroom = _classRepo.GetClassroom(id);
            if (Classroom == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            return View(Classroom);
        }
        [Authorize]

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Classroom model)
        {
            string Id = null;
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                Id = _userManager.GetUserId(HttpContext.User);
            }
            if (ModelState.IsValid)
            {
                Classroom newClass = new Classroom
                {
                    title = model.title,
                    description = model.description,
                    AppUserID = Id,
                    time_created = DateTime.Now
                };
                _classRepo.Add(newClass);

                ClassroomUser newClassUser = new ClassroomUser
                {
                    ClassroomId = newClass.ID,
                    AppUserId = Id,
                    Role = "Mentor"
                };
                _classUserRepo.Add(newClassUser);

                return RedirectToAction("Home", new { id = newClass.ID, loadPartial = "BlackBoard" });
            }
            return View();
        }
        [Authorize]

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Classroom Class = _classRepo.GetClassroom(id);
            Classroom newClass = new Classroom
            {
                ID = Class.ID,
                title = Class.title,
                description = Class.description,
                AppUserID = Class.AppUserID
            };
            return View(newClass);
        }
        [HttpPost]
        public IActionResult Edit(Classroom model)
        {
            if (ModelState.IsValid)
            {
                Classroom Class = _classRepo.GetClassroom(model.ID);
                Class.title = model.title;
                Class.description = model.description;
                Classroom updatedClass = _classRepo.Update(Class);
                return RedirectToAction("Home", new { id = model.ID, loadPartial = "BlackBoard" });
            }
            return View(model);
        }
        [Authorize]

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(ClassroomHomeViewModel chvm)
        {
            var Class = _classRepo.GetClassroom(chvm.Classroom.ID);
            _classRepo.Delete(Class.ID);
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Home(int id, string loadPartial)
        {
            Classroom Classroom = _classRepo.GetClassroom(id);
            string userId = null;
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                userId = _userManager.GetUserId(HttpContext.User);
            }
            ClassroomUser classUser = _classUserRepo.GetClassroomUser(id, userId);
            if (Classroom == null || classUser == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            ClassroomHomeViewModel chvm = new ClassroomHomeViewModel();
            chvm.Classroom = Classroom;
            chvm.BlackBoards = _boardRepo.GetClassBlackBoards(id);
            List<List<Comment>> Comments = new List<List<Comment>>();
            foreach (BlackBoard bb in chvm.BlackBoards)
            {
                Comments.Add(_commentRepo.GetBlackBoardComments(bb.Id).ToList());
            }
            chvm.Comments = Comments;
            chvm.ClassroomUserRole = classUser.Role;
            chvm.ClassroomMentors = _classUserRepo.GetClassroomMentors(id);
            chvm.ClassroomStudents = _classUserRepo.GetClassroomStudents(id);
            chvm.StudentInvites = _inviteRepo.GetAllInvites(id);
            chvm.Assignments = _assignmentRepo.GetClassAssignments(id);
            ViewData["loadPartial"] = loadPartial;
            return View(chvm);
        }
        [HttpGet]
        public IActionResult BlackBoard(int id)
        {
            ViewBag.ClassId = id;
            return View();
        }
        [HttpPost]
        public IActionResult BlackBoard(ClassroomHomeViewModel model)
        {
            string Id = null;
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                Id = _userManager.GetUserId(HttpContext.User);
            }
            if (ModelState.IsValid)
            {
                string filename = null;
                List<string> files = new List<string>();
                if (model.BlackBoardViewModel.Files != null)
                {
                    foreach (IFormFile file in model.BlackBoardViewModel.Files)
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "blackboard");
                        filename = Guid.NewGuid().ToString() + "_" + file.FileName;
                        files.Add(filename);
                        string filePath = Path.Combine(uploadsFolder, filename);
                        file.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }
                BlackBoard newBoard = new BlackBoard
                {
                    ClassroomId = Convert.ToInt32(model.BlackBoardViewModel.ClassId),
                    AppUserId = Id,
                    content = model.BlackBoardViewModel.content,
                    FilesPaths = string.Join(",", files)
                };
                _boardRepo.Add(newBoard);
            }
            return RedirectToAction("Home", new { id = model.BlackBoardViewModel.ClassId, loadPartial = "BlackBoard" });
        }
        public ViewResult BlackBoardIndex()
        {
            var model = _boardRepo.GetAllBlackBoards();
            return View(model);
        }

        public IActionResult DeleteBlackBoard(int id)
        {
            BlackBoard bb = _boardRepo.GetBlackBoard(id);
            if (bb == null)
            {
                return View("NotFound");
            }
            return View("BlackBoardDelete", bb);
        }
        [Authorize]
        [HttpPost, ActionName("DeleteBlackBoard")]
        public IActionResult BlackBoardDeleteConfirmed(int id)
        {
            BlackBoard bb = _boardRepo.GetBlackBoard(id);
            _boardRepo.Delete(bb.Id);
            return RedirectToAction("Home", new { id = bb.ClassroomId, loadPartial = "BlackBoard" });
        }
        [HttpPost]
        public IActionResult InviteStudents(string ClassId, string emails)
        {
            int id = Convert.ToInt32(ClassId);
            Classroom classroom = _classRepo.GetClassroom(id);
            AppUser user = _userManager.FindByIdAsync(_userManager.GetUserId(HttpContext.User)).Result;
            string[] Emails = emails.Split(" ");
            foreach (string email in Emails)
            {
                //    //Send Mail
                //    string DigiClassEmailId = "";
                //    string DigiClassPassword = "";
                //    MimeMessage message = new MimeMessage();
                //    MailboxAddress from = new MailboxAddress(user.UserName, DigiClassEmailId);
                //    message.From.Add(from);

                //    MailboxAddress to = new MailboxAddress("Student", email);
                //    message.To.Add(to);

                //    message.Subject = "Invite to "+classroom.title+" Blackboard";

                //    BodyBuilder bodyBuilder = new BodyBuilder();
                //    bodyBuilder.HtmlBody = "<div>" +
                //        "Hello Student," +
                //        "<br/><br/>" +
                //        "You've been invited to <b>" + classroom.title + "<b/>" +
                //        " Blackboard!" +
                //        "<br/><br/>" +
                //        "<a target=\"_blank\" style=\"color:#1b6ec2\" href=\"https://localhost:44300/Classroom/AcceptStudentInvite/" + classroom.ID + "\">Accept Invitation</a>&nbsp;&nbsp;" +
                //        "<a target=\"_blank\" style=\"color:#dc3545\" href=\"https://localhost:44300/Classroom/DeclineStudentInvite/" + classroom.ID + "\">Decline Invitation</a>" +
                //        "</div>";
                //    message.Body = bodyBuilder.ToMessageBody();

                //    SmtpClient client = new SmtpClient();
                //    client.CheckCertificateRevocation = false;
                //    client.SslProtocols = SslProtocols.Ssl3 | SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12 | SslProtocols.Tls13;
                //    client.Connect("smtp.gmail.com",465,true);
                //    client.Authenticate(DigiClassEmailId,DigiClassPassword);

                //    client.Send(message);
                //    client.Disconnect(true);
                //    client.Dispose();
                //    //Mail sent

                Invite invite = new Invite
                {
                    ClassroomId = id,
                    Email = email
                };
                _inviteRepo.Add(invite);
            }
            return RedirectToAction("Home", new { id = id, loadPartial = "People" });
        }
        public IActionResult AcceptStudentInvite(int id)
        {
            int classid = id;
            string userId = _userManager.GetUserId(HttpContext.User);
            string useremail = _userManager.FindByIdAsync(userId).Result.Email;
            ClassroomUser newClassUser = new ClassroomUser
            {
                ClassroomId = classid,
                AppUserId = userId,
                Role = "Student"
            };
            _classUserRepo.Add(newClassUser);
            _inviteRepo.Delete(classid, useremail);
            return RedirectToAction("Home", new { id = classid, loadPartial = "BlackBoard" });
        }

        public IActionResult DeclineStudentInvite(int id)
        {
            int classid = id;
            string userId = _userManager.GetUserId(HttpContext.User);
            string useremail = _userManager.FindByIdAsync(userId).Result.Email;
            _inviteRepo.Delete(classid, useremail);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult NewAssignment(ClassroomHomeViewModel model)
        {
            string Id = null;
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                Id = _userManager.GetUserId(HttpContext.User);
            }
            if (ModelState.IsValid)
            {
                string filename = null;
                List<string> files = new List<string>();
                if (model.AssignmentViewModel.Files != null)
                {
                    foreach (IFormFile file in model.AssignmentViewModel.Files)
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "posted_assignments");
                        filename = Guid.NewGuid().ToString() + "_" + file.FileName;
                        files.Add(filename);
                        string filePath = Path.Combine(uploadsFolder, filename);
                        file.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }
                Assignment newAssignment = new Assignment
                {
                    ClassroomID = Convert.ToInt32(model.AssignmentViewModel.ClassId),
                    AppUserID = Id,
                    Title = model.AssignmentViewModel.Title,
                    Description = model.AssignmentViewModel.Description,
                    Files = string.Join(",", files)
                };
                _assignmentRepo.Add(newAssignment);
            }
            return RedirectToAction("Home", new { id = model.AssignmentViewModel.ClassId, loadPartial = "Assignments" });
        }
        public IActionResult DeleteAssignment(int id)
        {
            Assignment a = _assignmentRepo.GetAssignment(id);
            if (a == null)
            {
                return View("NotFound");
            }
            return View(a);
        }

        [Authorize]
        [HttpPost, ActionName("DeleteAssignment")]
        public IActionResult AssignmentDeleteConfirmed(int id)
        {
            Assignment a = _assignmentRepo.GetAssignment(id);
            _assignmentRepo.Delete(a.ID);
            return RedirectToAction("Home", new { id = a.ClassroomID, loadPartial = "Assignments" });
        }
        [HttpGet]
        public IActionResult SubmitAssignment(int id)
        {
            ViewData["AssignmentId"] = id;
            Assignment assignment = _assignmentRepo.GetAssignment(id);
            ViewBag.ClassId = assignment.ClassroomID;
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult SubmitAssignment(SubmitAssignmentViewModel model)
        {
            string userId = null;
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                userId = _userManager.GetUserId(HttpContext.User);
            }
            if (ModelState.IsValid)
            {
                Assignment assignment = _assignmentRepo.GetAssignment(Convert.ToInt32(model.AssignmentID));
                string filename = null;
                List<string> files = new List<string>();
                if (model.Files != null)
                {
                    foreach (IFormFile file in model.Files)
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "submitted_assignments");
                        filename = Guid.NewGuid().ToString() + "_" + file.FileName;
                        files.Add(filename);
                        string filePath = Path.Combine(uploadsFolder, filename);
                        file.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }
                SubmittedAssignment newAssignment = new SubmittedAssignment
                {
                    AssignmentID = Convert.ToInt32(model.AssignmentID),
                    AppUserID = userId,
                    Title = assignment.Title,
                    Description = model.Description,
                    Files = string.Join(",", files)
                };
                _submittedAssignmentRepo.Add(newAssignment);
                return RedirectToAction("Home", new { id = assignment.ClassroomID, loadPartial = "Assignments" });
            }
            return View("NotFound");
        }
        [Authorize]
        public IActionResult ViewSubmissions(int id)
        {
            string userId = null;
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                userId = _userManager.GetUserId(HttpContext.User);
            }
            Assignment assignment = _assignmentRepo.GetAssignment(id);
            if (assignment == null)
            {
                return View("NotFound");
            }
            ClassroomUser classroomUser = _classUserRepo.GetClassroomUser(assignment.ClassroomID, userId);
            if (classroomUser == null)
            {
                return View("NotFound");
            }
            ViewData["AssignmentTitle"] = assignment.Title;
            ViewData["Role"] = classroomUser.Role;
            ViewBag.ClassId = assignment.ClassroomID;
            IEnumerable<SubmittedAssignment> assignments = null;
            if (classroomUser.Role == "Mentor")
            {
                ViewData["EmptyMessage"] = "No submissions right now T-T. Check again later.";
                IEnumerable<ClassroomUser> notSubmitted = _submittedAssignmentRepo.GetPeopleNotSubmitted(id);
                ViewData["Count"] = notSubmitted.Count();
                ViewBag.RemainingPeople = notSubmitted;
                assignments = _submittedAssignmentRepo.GetSubmittedAssignments(id);
            }
            if (classroomUser.Role == "Student")
            {
                ViewData["EmptyMessage"] = "You haven't made any submissions yet -.- !";
                assignments = _submittedAssignmentRepo.GetUserSubmittedAssignments(id, userId);
            }
            return View(assignments);
        }
        [Authorize]
        [HttpPost, ActionName("LeaveClassroom")]
        public IActionResult LeaveClassroomConfirmed(ClassroomHomeViewModel chvm)
        {
            string userId = null;
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                userId = _userManager.GetUserId(HttpContext.User);
                _classUserRepo.Delete(chvm.Classroom.ID, userId);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AddComment(ClassroomHomeViewModel model)
        {
            string Id = null;
            int classId = 0;
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                Id = _userManager.GetUserId(HttpContext.User);
                Comment newComment = new Comment
                {
                    AppUserId = Id,
                    BlackBoardId = model.Comment.BlackBoardId,
                    TimeCreated = DateTime.Now,
                    Content = model.Comment.Content,
                };
                classId = _boardRepo.GetBlackBoard(model.Comment.BlackBoardId).ClassroomId;
                _commentRepo.Add(newComment);
            }
            return RedirectToAction("Home", new { id = classId, loadPartial = "BlackBoard" });
        }
    }
}