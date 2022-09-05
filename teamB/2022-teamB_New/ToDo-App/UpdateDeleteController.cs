using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Text.RegularExpressions;
using ToDo_App.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Task = ToDo_App.Models.Task;
using Microsoft.AspNetCore.Http;

namespace ToDo_App.Controllers
{
    public class UpdateDeleteController : Controller
    {

        private readonly TeamBContext _db;
        public UpdateDeleteController(TeamBContext db) => _db = db;

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(/*Task task string taskname, DateTime start_date, DateTime start_time, DateTime deadline_date, DateTime deadline_time, string comment, int groupID, string priority, string image, string image_data*/)
        {


            //TaskId = int.Parse(ViewData["TaskId"]);

            string UserId;
            ViewData["SessionUserId"] = null;
            //セッションの値にログインIDが入っている場合
            if (HttpContext.Session.GetString(SetSession.SessionUserId) != null)
            {
                ViewData["SessionUserId"] = HttpContext.Session.GetString(SetSession.SessionUserId);
                //ログインしているユーザーIDを格納
                UserId = HttpContext.Session.GetString(SetSession.SessionUserId);
            }
            else
            {
                //入っていない場合はそのまま
                return View();
            }

            //セッションにタスクIDを保存
            var TaskId = HttpContext.Session.GetInt32(SetSession.SessionTaskId);
            Console.WriteLine(TaskId);
            Console.WriteLine(UserId);
            
            var UserId_check = _db.Tasks.Where(x => x.UserId == UserId).FirstOrDefault();
            var TaskId_check = _db.Tasks.Where(x => x.TaskId == TaskId).FirstOrDefault();

            if (TaskId_check == null || UserId_check == null)
            {
                RedirectToAction("Index");
            }



            if ( TaskId_check != null && UserId_check != null)
            {
                Task task = _db.Tasks.Where(x => x.TaskId == TaskId).FirstOrDefault();
                /*var task = new Models.Task
                {
                    StartDate = DateTime.Now,
                    DeadlineDate = DateTime.Now,
                    RegisterDate = DateTime.Now,
                    Priority = 3,
                    ImagePath = null,
                    Comment = null,
                    Taskname = "delete",
                    TaskStatus = false,
                    GroupId = 1,

                };*/
                _db.Remove(task);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Display");
            }
            else
            {
                return View();
}
        }

        public IActionResult Index()
        {
            ViewData["GroupId"] = new SelectList(_db.TaskGroups, "GroupId", "Groupname");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(/*Models.Task task string taskname, DateTime start_date, DateTime start_time, DateTime deadline_date, DateTime deadline_time, string comment, int groupID, string priority, string image, string image_data*/ string id)
        {
            /*Console.WriteLine(taskname);
            Console.WriteLine(start_date);
            Console.WriteLine(start_time);
            Console.WriteLine(deadline_date);
            Console.WriteLine(deadline_time);
            Console.WriteLine(comment);
            Console.WriteLine(groupID);
            Console.WriteLine(priority);
            Console.WriteLine(image);
            Console.WriteLine(image_data);*/

            DateTime dt = new DateTime(0001, 01, 01, 0, 00, 00);

            Console.WriteLine(dt);

            int taskname_len;
            int comment_len;

            //task更新のため必ず入力する必要なし
            /*if (System.String.IsNullOrEmpty(taskname))
            {
                ViewData["message"] = "タスク名を入力してください。";
                return View("Index");
            }
            if (start_date == dt)
            {
                ViewData["message"] = "期限が入力されていません。";
                return View("Index");
            }
            if (priority == "--")
            {
                ViewData["message"] = "優先度を選択してください。";
                return View("Index");
            }*/

            string UserId;
            ViewData["SessionUserId"] = null;
            //セッションの値にログインIDが入っている場合
            if (HttpContext.Session.GetString(SetSession.SessionUserId) != null)
            {
                ViewData["SessionUserId"] = HttpContext.Session.GetString(SetSession.SessionUserId);
                //ログインしているユーザーIDを格納
                UserId = HttpContext.Session.GetString(SetSession.SessionUserId);
            }
            else
            {
                //入っていない場合はそのまま
                return View();
            }

            //セッションにタスクIDを保存
            var TaskId = HttpContext.Session.GetInt32(SetSession.SessionTaskId);
            Console.WriteLine(TaskId);
            Console.WriteLine(UserId);

            var UserId_check = _db.Tasks.Where(x => x.UserId == UserId).FirstOrDefault();
            var TaskId_check = _db.Tasks.Where(x => x.TaskId == TaskId).FirstOrDefault();

            if (TaskId_check == null || UserId_check == null)
            {
                return RedirectToAction("Index", "Display");
            }

            /*var Groupname = HttpContext.Session.GetString(SetSession.SessionGroupname);
            var Priority = HttpContext.Session.GetInt32(SetSession.SessionPriority);
            var Taskname = HttpContext.Session.GetString(SetSession.SessionTaskname);

            var DeadlineDate = HttpContext.Session.Get<DateTime>(SetSession.SessionDeadlineDate);
            var RegisterDate = HttpContext.Session.Get<DateTime>(SetSession.SessionRegisterDate);
            var Comment = HttpContext.Session.GetInt32(SetSession.SessionComment);*/

            taskname_len = new StringInfo(taskname).LengthInTextElements;
            comment_len = 0;
            if (!System.String.IsNullOrEmpty(comment))
            {
                comment_len = new StringInfo(comment).LengthInTextElements;
            }

            if (start_date.Date > deadline_date.Date || (start_date.Date == deadline_date.Date && start_time.TimeOfDay > deadline_time.TimeOfDay))
            {
                ViewData["message"] = "タスクの開始日と期限日の関係が正しくありません。";
                return View("Index");
            }

            if (taskname_len >= 256)
            {
                ViewData["message"] = "タスク名の文字数は255文字までです。";
                return View("Index");
            }

            bool jpg_b = false;
            bool png_b = false;
            bool gif_b = false;

            if (!System.String.IsNullOrEmpty(image))
            {
                jpg_b = (Regex.IsMatch(image, ".jpg$"));
                png_b = (Regex.IsMatch(image, ".png$"));
                gif_b = (Regex.IsMatch(image, ".gif$"));
            }

            if (!System.String.IsNullOrEmpty(image) && !(jpg_b == true || png_b == true || gif_b == true))
            {
                ViewData["message"] = "挿入されたファイルは画像ファイルではありません。";
                return View("Index");
            }

            if (!System.String.IsNullOrEmpty(comment) && comment_len >= 65535)
            {
                ViewData["message"] = "コメントの文字数は65535文字までです。";
                return View("Index");
            }

            //if (ModelState.IsValid)
            //{
            var task = new Models.Task
            {
                StartDate = start_date,
                DeadlineDate = deadline_date,
                Priority = int.Parse(priority),
                ImagePath = image,
                Comment = comment,
                Taskname = taskname,
                TaskStatus = false,
                GroupId = groupID,

            };
            _db.Add(task);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Display");

            //}
            //return RedirectToAction("Index","Display");
        }

    }
}
