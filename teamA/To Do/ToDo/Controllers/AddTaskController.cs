using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using ToDo.Models;
using ToDo.ViewModels;

namespace ToDo.Controllers
{
    public class AddTaskController : Controller
    {
        private readonly TeamAContext _db;

        public AddTaskController(TeamAContext db) => _db = db;

        public IActionResult AddTask()
        {
            return View();
        }

        public IActionResult HomeBack()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTaskForm addTaskForm) //　タスク登録
        {
            /*
            if (task.StartTime >= task.FinishTime) //時間入力のエラー処理
            {
                ViewData["time_error"] = "終了時間は開始時間よりも後の日時を設定して下さい";
                return View("AddTask");
            }

            DateTime dt = DateTime.Now;
            if (dt > task.StartTime) //時間入力のエラー処理
            {
                ViewData["time_error2"] = "現在日時よりも後の日時を入力して下さい";
                return View("AddTask");
            }

            int Task_len = new StringInfo(task.TaskName).LengthInTextElements;
            if (Task_len > 30) //文字数制限のエラー処理
            {
                ViewData["content_error"] = "30文字以内で入力して下さい";
                return View("AddTask");
            }

            int Comment_len = 0;
            if (!System.String.IsNullOrEmpty(task.Comment))
            {
                Comment_len = new StringInfo(task.Comment).LengthInTextElements;
                if (Comment_len > 100)　//文字数制限のエラー処理
                {
                    ViewData["comment_error"] = "100文字以内で入力して下さい";
                    return View("AddTask");
                }
            }
            */

            if (!ModelState.IsValid)
            {
                return View("AddTask", addTaskForm);
            }

            var add = new Models.TaskTable //タスク情報の追加
            {
                TaskName = addTaskForm.TaskName,
                StartTime = addTaskForm.StartTime,
                FinishTime = addTaskForm.FinishTime,
                Comment = addTaskForm.Comment,
                ColorId = addTaskForm.ColorId,
                UserId = HttpContext.Session.GetString("UserId"),
                CreatedTime = DateTime.Now,
                MyPriority = addTaskForm.MyPriority,
                DeleteFlag = false,
                CompleteFlag = false
            };
            _db.Add(add);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }        
    }            
}    
