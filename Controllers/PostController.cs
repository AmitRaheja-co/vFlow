using Microsoft.AspNetCore.Mvc;
using vFlow.Data;
using vFlow.Models;
using System.Collections.Generic;
using vFlow.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using vFlow.Repository;
using vFlow.Repository.IRepository;
using Microsoft.Extensions.Hosting;

namespace vFlow.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PostController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public IActionResult Index()
        {
            QuesAns quesAns = new QuesAns();
            quesAns.posts = (List<Post>)_unitOfWork.Posts.GetAll();
            quesAns.Answers = (List<Answer>)_unitOfWork.Answers.GetAll();
            quesAns.AppUsers = (List<AppUser>)_unitOfWork.AppUsers.GetAll();

            quesAns.posts = quesAns.posts.OrderByDescending(p => p.TimeStamp).ToList();
            quesAns.Comments = quesAns.Comments.OrderByDescending(p => p.TimeStamp).ToList();
            quesAns.Answers = quesAns.Answers.OrderByDescending(a => a.UpVote?.Count ?? 0)
                               .ThenByDescending(a => a.TimeStamp)
                               .ToList();

            //return Json(quesAns);

            return View(quesAns);
        }

        public IActionResult CreatePost(Post post)
        {
            if (post == null)
            {
                post = new Post();
                return Ok("post is null here do not know why");
            }
            
            return View(post);
        }

        [HttpPost]
        public IActionResult CreatePost(Post post, string allTags = null, string RemoveTags = null)
        {
            var claims = (ClaimsIdentity)User.Identity;
            var id = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            post.TimeStamp = DateTime.Now;
            post.AppUserId = id;

            bool isPresent = _unitOfWork.Posts.Get(u => u.Id == post.Id) != null;
            var curr_post = _unitOfWork.Posts.Get(u => u.Id == post.Id);
            if (allTags!=null)
            {
                allTags = allTags.Trim().ToUpper();
            }
            if (RemoveTags != null)
            {
                RemoveTags = RemoveTags.Trim().ToUpper(); 
            }
            if (post.Tags == null)
            {
                post.Tags = new List<string>();
            }
            if (isPresent && curr_post.Tags == null)
            {
                curr_post.Tags = new List<string>();
            }
            if (curr_post != null)
            {
                foreach (var i in curr_post.Tags)
                {
                    post.Tags.Add(i);
                }
            }

            if (!string.IsNullOrEmpty(RemoveTags))
            {
                var tagsToRemove = RemoveTags.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var tagToRemove in tagsToRemove)
                {
                    post.Tags.Remove(tagToRemove);
                }
            }

            if (allTags != null)
            {
                var obj = allTags.Split(' ').ToList();
                foreach (var i in obj)
                {
                    if (i != null)
                    {
                        post.Tags.Add(i);
                    }
                }
            }
            
            if (!isPresent)
            {
                _unitOfWork.Posts.Add(post);
            }
            else
            {
                _unitOfWork.Posts.Update(post);
            }
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }


        public IActionResult UpdatePost(int postId)
        {
            var claims = (ClaimsIdentity)User.Identity;
            var id = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            var obj = _unitOfWork.Posts.Get(u => u.Id == postId);
            if (!User.IsInRole("Admin") && obj.AppUserId != id)
            {
                return Forbid(); // Return 403 Forbidden if the user is not authorized
            }
            return RedirectToAction("CreatePost", obj);

        }
        
        [HttpPost]
        public IActionResult DeletePost(int postId)
        {
            var claims = (ClaimsIdentity)User.Identity;
            var id = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            var obj = _unitOfWork.Posts.Get(u => u.Id == postId);
            if (!User.IsInRole("Admin") && obj.AppUserId != id)
            {
                return Forbid(); // Return 403 Forbidden if the user is not authorized
            }
            _unitOfWork.Posts.Remove(obj);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult AnsPost(int id)
        {
            QuesAns quesAns = new QuesAns();
            Post currPost = _unitOfWork.Posts.Get(u => u.Id == id);
            quesAns.posts = new List<Post>() { currPost };
            quesAns.Answers = (List<Answer>)_unitOfWork.Answers.GetAll(u => u.PostId == id);
            quesAns.Comments = (List<Comment>)_unitOfWork.Comments.GetAll();
            quesAns.AppUsers = (List<AppUser>)_unitOfWork.AppUsers.GetAll();

            quesAns.posts = quesAns.posts.OrderByDescending(p => p.TimeStamp).ToList();
            quesAns.Comments = quesAns.Comments.OrderByDescending(p => p.TimeStamp).ToList();
            quesAns.Answers = quesAns.Answers.OrderByDescending(a => a.UpVote?.Count ?? 0)
                               .ThenByDescending(a => a.TimeStamp)
                               .ToList();


            return View(quesAns);
        }

        [HttpPost]
        public IActionResult AddAns(int postId, string answerText) {

            if (answerText != null)
            {

                Answer ans = new Answer();
                var claims = (ClaimsIdentity)User.Identity;
                var Userid = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

                ans.AppUserId = Userid;
                ans.TimeStamp = DateTime.Now;
                ans.PostId = postId;
                ans.Body = answerText;

                _unitOfWork.Answers.Add(ans);
                _unitOfWork.Save();
            }
            return RedirectToAction("AnsPost", new { id = postId });
        }
        public IActionResult EditAns(int ansId)
        {
            var claims = (ClaimsIdentity)User.Identity;
            var id = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var obj = _unitOfWork.Answers.Get(u => u.Id == ansId);
            if (!User.IsInRole("Admin") && obj.AppUserId != id)
            {
                return Forbid(); // Return 403 Forbidden if the user is not authorized
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult EditAns(int Id,string AnsBody)
        {

            var obj = _unitOfWork.Answers.Get(u => u.Id == Id);
            obj.Body = AnsBody;
            _unitOfWork.Answers.Edit(obj);
            _unitOfWork.Save();
            return RedirectToAction("AnsPost", new { id = obj.PostId });
        }
        [HttpPost]
        public IActionResult DeleteAns(int ansId, int postId)
        {
            var obj = _unitOfWork.Answers.Get(u => u.Id == ansId);
            var claims = (ClaimsIdentity)User.Identity;
            var id = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!User.IsInRole("Admin") && obj.AppUserId != id)
            {
                return Forbid(); // Return 403 Forbidden if the user is not authorized
            }
            _unitOfWork.Answers.Remove(obj);
            _unitOfWork.Save();
            return RedirectToAction("AnsPost", new { id = postId });
        }

        [HttpPost]
        public IActionResult AddComment(int AnsId,string commentText,int postId)
        {
            if (commentText != null)
            {
                Comment comment = new Comment();
                var claims = (ClaimsIdentity)User.Identity;
                var Userid = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

                comment.AppUserId = Userid;
                comment.TimeStamp = DateTime.Now;
                comment.AnsId = AnsId;
                comment.Body = commentText;

                _unitOfWork.Comments.Add(comment);
                _unitOfWork.Save();
            }

            //return Json(comment)
            return RedirectToAction("AnsPost", new { id = postId });
        }
        [HttpPost]
        public IActionResult EditComment(int postId, int commentId, string commentText)
        {
            Comment obj = _unitOfWork.Comments.Get(u => u.Id == commentId);
            var claims = (ClaimsIdentity)User.Identity;
            var id = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!User.IsInRole("Admin") && obj.AppUserId != id)
            {
                return Forbid(); // Return 403 Forbidden if the user is not authorized
            }
            if (obj == null)
            {
                return NotFound();
            }
            obj.Body = commentText;

            _unitOfWork.Comments.Edit(obj);
            _unitOfWork.Save();

            return RedirectToAction("AnsPost", new { id = postId });
        }
        [HttpPost]
        public IActionResult DeleteComment(int commentId,int postId)
        {
            var obj = _unitOfWork.Comments.Get(u => u.Id == commentId);
            var claims = (ClaimsIdentity)User.Identity;
            var id = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!User.IsInRole("Admin") && obj.AppUserId != id)
            {
                return Forbid(); // Return 403 Forbidden if the user is not authorized
            }
            _unitOfWork.Comments.Remove(obj);
            _unitOfWork.Save();
            return RedirectToAction("AnsPost", new { id = postId });
        }

        [HttpPost]
        public IActionResult Upvote(int ansId,int postId)
        {
            var claims = (ClaimsIdentity)User.Identity;
            var Userid = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            Answer ans = _unitOfWork.Answers.Get(u=>u.Id == ansId);


            if (ans.UpVote == null) {
                ans.UpVote = new List<string>();
            }
            var ifAlreadyPresent = ans.UpVote.FirstOrDefault(obj => obj == Userid);
            
            if(ifAlreadyPresent==null)
            {
                ans.UpVote.Add(Userid);
            }
            else
            {
                ans.UpVote.Remove(Userid);
            }
            _unitOfWork.Save();

            return RedirectToAction("AnsPost", new {id = postId});
        }
    }
}
