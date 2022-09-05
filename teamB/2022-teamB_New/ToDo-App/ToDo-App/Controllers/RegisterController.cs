using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Text.RegularExpressions;
using ToDo_App.Models;
using ToDo_App.ViewModels.Register;
using System;
using System.IO;
using System.Text;
using System.Drawing;
using NuGet.Packaging.Signing;


namespace ToDo_App.Controllers
{
    public class RegisterController : Controller
    {
        private readonly TeamBContext _db;
        public RegisterController(TeamBContext db) => _db = db;
        public IActionResult Index()
        {
            //日時選択部分の初期値設定
          

            //ログインされていない場合はログイン画面に戻る
            if (HttpContext.Session.GetString(SetSession.SessionUserId) == null)
            {
                return RedirectToAction(nameof(Index), "Display");
            }
            else
            {
                ViewData["GroupId"] = new SelectList(_db.TaskGroups, "GroupId", "Groupname");
                ViewData["Priority"] = new SelectList(_db.PriorityTables, "PriorityId", "Priorityname");
                ViewData["UserId"] = HttpContext.Session.GetString(SetSession.SessionUserId);

                return View("Index");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(addForm addForm)
        {
            ViewData["UserId"] = null;
            string userid;
            if (!ModelState.IsValid)
            {
                Console.WriteLine("誤り");
                var errormsgs = ModelState.SelectMany(x => x.Value.Errors.Select(z => z.ErrorMessage));
                foreach (var item in errormsgs)
                {
                    Console.WriteLine(item);
                }
                return View("Index");
            }

            //セッションからユーザーIDを読み込む
            if ( HttpContext.Session.GetString(SetSession.SessionUserId) != null)
            {
                userid = HttpContext.Session.GetString(SetSession.SessionUserId);
                ViewData["UserId"] = userid;
                
            }
            else
            {
                return RedirectToAction("Index", "Display");
            }

            var value = HttpContext.Request.Cookies["test"];
            //Console.WriteLine(value);
            //DateTime dt = new DateTime(0001, 01, 01, 0, 00, 00);
            
            //Console.WriteLine(dt);
            if(addForm.StartDate.Value.Date > addForm.DeadlineDate.Value.Date || (addForm.StartDate.Value.Date == addForm.DeadlineDate.Value.Date && addForm.StartDate.Value.TimeOfDay > addForm.DeadlineDate.Value.TimeOfDay))
            {
                ViewData["message"] = "タスクの開始日と期限日の関係が正しくありません。";
                return View("Index");
            }

            //ファイルがアップロードされた場合
            if (addForm.ImageFile != null)
            {
                //画像を保存するファイルパス
                string file_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "IMG", userid);
                Console.WriteLine(file_path);
                //画像ファイル名を取得
                string image_path = addForm.ImageFile.FileName;
                FileInfo fileinfo = new FileInfo(image_path);
                string extension = fileinfo.Extension;
                if(extension == ".png" || extension == ".gif" || extension == ".jpg")
                {
                    string dest_path = Path.Combine(file_path, image_path);
                    //サーバーに画像をアップロードする
                    using (var stream = new FileStream(dest_path, FileMode.Create))
                    {
                        addForm.ImageFile.CopyTo(stream);
                    }
                }
                else
                {
                    ViewData["message"] = "アップロードできるのは画像ファイルのみです。";
                    return View("Index");
                }

                var task = new Models.Task
                {
                    StartDate = addForm.StartDate.Value,
                    DeadlineDate = addForm.DeadlineDate.Value,
                    Priority = addForm.Priority.Value,
                    ImagePath = addForm.ImageFile.FileName,
                    Comment = addForm.Comment,
                    Taskname = addForm.Taskname,
                    TaskStatus = false,
                    GroupId = addForm.GroupId,
                    UserId = HttpContext.Session.GetString(SetSession.SessionUserId),
                    RegisterDate = DateTime.Now
                };
                _db.Add(task);
                await _db.SaveChangesAsync();
                Console.WriteLine((DateTime)addForm.StartDate);
                Console.WriteLine("成功");
                return RedirectToAction(nameof(Index), "Display");
            }
            else {
                var task = new Models.Task
                {
                    StartDate = addForm.StartDate.Value,
                    DeadlineDate = addForm.DeadlineDate.Value,
                    Priority = addForm.Priority.Value,
                    ImagePath = null,
                    Comment = addForm.Comment,
                    Taskname = addForm.Taskname,
                    TaskStatus = false,
                    GroupId = addForm.GroupId,
                    UserId = HttpContext.Session.GetString(SetSession.SessionUserId),
                    RegisterDate = DateTime.Now
                };
                _db.Add(task);
                await _db.SaveChangesAsync();
                Console.WriteLine("成功");
                return RedirectToAction(nameof(Index), "Display");
            }
        }
    }
}
