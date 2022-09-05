using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Text.RegularExpressions;
using ToDo_App.Models;
using ToDo_App.ViewModels;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Task = ToDo_App.Models.Task;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Drawing;
using ToDo_App.ViewModels.Register;

namespace ToDo_App.Controllers
{
    public class UpdateDeleteController : Controller
    {

        private readonly TeamBContext _db;
        //ホストのルートパス環境を取得
        private readonly IWebHostEnvironment _env;
        //コンストラクト
        public UpdateDeleteController(IWebHostEnvironment env, TeamBContext db)
        {
            _env = env;
            _db = db;
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(/*Task task string taskname, DateTime start_date, DateTime start_time, DateTime deadline_date, DateTime deadline_time, string comment, int groupID, string priority, string image, string image_data*/)
        {

            string UserId;
            ViewData["SessionUserId"] = null;
            //セッションの値にログインIDが入っている場合
            if (HttpContext.Session.GetString(SetSession.SessionUserId) != null)
            {
                ViewData["SessionUserId"] = HttpContext.Session.GetString(SetSession.SessionUserId);
                //ログインしているユーザーIDを格納
                UserId = HttpContext.Session.GetString(SetSession.SessionUserId);
                Console.WriteLine("データベースに参照しました。");
            }
            else
            {
                //入っていない場合はそのまま
                Console.WriteLine("データベースに参照できません。");
                return View();
            }

            //セッションにタスクIDを保存
            var TaskId = HttpContext.Session.GetString(SetSession.SessionTaskId);
            Console.WriteLine(TaskId);
            Console.WriteLine(UserId);
            
            var UserId_check = _db.Tasks.Where(x => x.UserId == UserId).FirstOrDefault();
            var TaskId_check = _db.Tasks.Where(x => x.TaskId == int.Parse(TaskId)).FirstOrDefault();

            if (TaskId_check == null || UserId_check == null)
            {
                RedirectToAction("Index");
            }



            if (TaskId_check != null && UserId_check != null)
            {
                Task task = _db.Tasks.Where(x => x.TaskId == int.Parse(TaskId)).FirstOrDefault();
                
                _db.Remove(task);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Display");
            }
            else
            {
                return RedirectToAction("Index", "Display");
}
        }

        public IActionResult Index(string id, UpdateForm updateForm)
        {

            string UserId;
            ViewData["SessionUserId"] = null;
            //セッションの値にログインIDが入っている場合
            if (HttpContext.Session.GetString(SetSession.SessionUserId) != null)
            {
                ViewData["GroupId"] = new SelectList(_db.TaskGroups, "GroupId", "Groupname");
                ViewData["Priority"] = new SelectList(_db.PriorityTables, "PriorityId", "Priorityname");
                ViewData["SessionUserId"] = HttpContext.Session.GetString(SetSession.SessionUserId);
                //ログインしているユーザーIDを格納
                UserId = HttpContext.Session.GetString(SetSession.SessionUserId);
            }
            else
            {
                //入っていない場合はログインを促す
                return RedirectToAction(nameof(Index), "Display");
            }

            string taskid;
            if (id != null)
            {
                HttpContext.Session.SetString(SetSession.SessionTaskId, id);
            }
            else
            {
                id = HttpContext.Session.GetString(SetSession.SessionTaskId);
            }

            var Tasks = _db.Tasks.Where(x => x.UserId == UserId && x.TaskId == int.Parse(id)).FirstOrDefault();

            updateForm.Taskname = Tasks.Taskname;
            ViewData["StartDate"] = Tasks.StartDate.ToString("yyyy-MM-ddThh:mm:ss");
            //updateForm.StartDate = Tasks.StartDate;
            //updateForm. = Tasks.StartDate.TimeOfDay;
            ViewData["DeadlineDate"] = Tasks.DeadlineDate.ToString("yyyy-MM-ddThh:mm:ss");           
            //updateForm.DeadlineDate = Tasks.DeadlineDate.Value;
            //ViewData["deadline_time"] = Tasks.DeadlineDate.TimeOfDay;
            updateForm.Comment = Tasks.Comment;
            //updateForm.ImageFile = Tasks.ImagePath;
            updateForm.Priority = Tasks.Priority;

            switch (Tasks.Priority)
            {
                case 3:
                    ViewData["Priority"] = "高";
                    break;
                case 2:
                    ViewData["Priority"] = "中";
                    break;
                case 1:
                    ViewData["Priority"] = "低";
                    break;
                default:
                    ViewData["Priority"] = "中";
                    break;
            }
            //タスクのグループIDと一致するグループ名を取得
            var Group = _db.TaskGroups.Where(x => x.GroupId == Tasks.GroupId).FirstOrDefault();

            updateForm.GroupId = Group.GroupId;
            ViewData["Groupname"] = Group.Groupname;
            return View(updateForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateForm updateForm)
        {
            
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
                UserId = HttpContext.Session.GetString(SetSession.SessionUserId);
                ViewData["SessionUserId"] = UserId;
                //ログインしているユーザーIDを格納
            }


            else
            {
                //入っていない場合はそのまま
                return View();
            }

            //セッションにタスクIDを保存
            var TaskId = HttpContext.Session.GetString(SetSession.SessionTaskId);
            var task = _db.Tasks.Where(x => x.TaskId == int.Parse(TaskId)).FirstOrDefault();

            var UserId_check = _db.Tasks.Where(x => x.UserId == UserId).FirstOrDefault();
            var TaskId_check = _db.Tasks.Where(x => x.TaskId == int.Parse(TaskId)).FirstOrDefault();

            if (TaskId_check == null || UserId_check == null)
            {
                return RedirectToAction("Index", "Display");
            }

            
            //taskname_len = new StringInfo(updateForm.Taskname).LengthInTextElements;
            /*comment_len = 0;
            if (!System.String.IsNullOrEmpty(updateForm.Comment))
            {
                comment_len = new StringInfo(c).LengthInTextElements;
            }*/

            if (updateForm.StartDate.Date > updateForm.DeadlineDate.Date || (updateForm.StartDate.Date == updateForm.DeadlineDate.Date && updateForm.StartDate.TimeOfDay > updateForm.DeadlineDate.TimeOfDay))
            {
                ViewData["message"] = "タスクの開始日と期限日の関係が正しくありません。";
                return RedirectToAction("Index"); ;
            }

            /*
            if (taskname_len >= 256)
            {
                ViewData["message"] = "タスク名の文字数は255文字までです。";
                return View("Index");
            }

            bool jpg_b = false;
            bool png_b = false;
            bool gif_b = false;
            */

            
            /*if (!System.String.IsNullOrEmpty(comment) && comment_len > 65535)
            {
                ViewData["message"] = "コメントの文字数は65535文字までです。";
                return View("Index");
            }*/
            
            //ファイルがアップロードされた場合
            if (updateForm.ImageFile != null)
            {
                //画像を保存するファイルパス
                string file_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "IMG", UserId);
                Console.WriteLine(file_path);
                
                //画像ファイル名を取る
                //ファイル拡張子を確認
                string image_path = updateForm.ImageFile.FileName;
                FileInfo fileinfo = new FileInfo(image_path);
                string extension = fileinfo.Extension;
                //拡張子が画像ファイルならば
                if (extension == ".png" || extension == ".gif" || extension == ".jpg")
                {

                    string dest_path = Path.Combine(file_path, image_path);
                    //サーバーに画像をアップロードする
                    using (var stream = new FileStream(dest_path, FileMode.Create))
                    {
                        updateForm.ImageFile.CopyTo(stream);
                    }
                    //パス名の対応を更新
                    task.ImagePath = image_path;
                }
                else
                {
                    ViewData["message"] = "アップロードできるのは画像ファイルのみです。";
                    return View(); ;
                }
            }

             //DB更新
                task.Taskname = updateForm.Taskname;
                task.StartDate = updateForm.StartDate;
                task.DeadlineDate = updateForm.DeadlineDate;
                task.Comment = updateForm.Comment;
                task.Priority = updateForm.Priority;
                task.Comment = updateForm.Comment;
                task.GroupId = updateForm.GroupId;
                
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), "Display");
            
            
        }

    }
}
