using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using ToDo.Models;
using ToDo.ViewModels;

namespace ToDo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly TeamAContext _db;

        public HomeController(ILogger<HomeController> logger, TeamAContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAllTasks(DateTime start, DateTime end)
        {

            var res = _db.TaskTables.Where(x => x.DeleteFlag != true && HttpContext.Session.GetString("UserId") == x.UserId).Select(e => new
            {
                Id = e.TaskId,
                Title = e.TaskName,
                Start = e.StartTime,
                End = e.FinishTime,
                color = e.CompleteFlag ? "#aaaaaa": e.ColorId,
                textColor = e.CompleteFlag ? "black": "#34ebe8",
                url = Url.Action("Detail" ,new{ id = e.TaskId })
            }).ToList();
            return Json(res);
        }


        //詳細表示・カレンダーから
        public IActionResult Detail(int id)
        {
            if (!IsLogin(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("Index", "Login");
            }


            var task = _db.TaskTables.Where(x => x.TaskId == id).FirstOrDefault();

            var edittaskform = new EditTaskForm
            {
                TaskId = id,

                TaskName = task.TaskName,

                StartTime = task.StartTime,

                FinishTime = task.FinishTime,

                Comment = task.Comment,

                MyPriority = task.MyPriority,

                ColorId = task.ColorId
            };

            if (task.CompleteFlag == false)
            {
                ViewData["Complete"] = "完了";
            }
            else if(task.CompleteFlag == true)
            {
                ViewData["Complete"] = "未完了";
            }

            return View("Detail", edittaskform);
        }

        //タスク削除
        public IActionResult Delete( int taskId )
        {
            var task = _db.TaskTables.Where(x => x.TaskId == taskId).FirstOrDefault();

            if (task != null)
            {
                task.DeleteFlag = true;

                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        //タスク完了
        public IActionResult Complete(int taskId)
        {
            var task = _db.TaskTables.Where(x => x.TaskId == taskId).FirstOrDefault();

            if (task != null)
            {
                task.CompleteFlag ^= true;
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        //タス ク更新
        public IActionResult Edit(EditTaskForm editTaskForm)
        {

            if (!ModelState.IsValid)
            {
                return View("Detail", editTaskForm);
            }
            if( editTaskForm.StartTime > editTaskForm.FinishTime)
            {
                ViewData["message"] = "不正な時間登録です";
                return View("Detail");
            }
            var newTask = _db.TaskTables.Where(x => x.TaskId == editTaskForm.TaskId).FirstOrDefault();

            newTask.TaskName = editTaskForm.TaskName;

            newTask.StartTime = editTaskForm.StartTime;

            newTask.FinishTime = editTaskForm.FinishTime;

            newTask.Comment = editTaskForm.Comment;

            newTask.MyPriority = editTaskForm.MyPriority;

            newTask.ColorId = editTaskForm.ColorId;

            _db.SaveChanges();

            return RedirectToAction("Index");
        }



        public IActionResult Index()
        {
            if (!IsLogin(HttpContext.Session.GetString("UserId")))
            {
                return RedirectToAction("Index", "Login");
            }


            var NoneTasks = _db.TaskTables.Where(x => x.DeleteFlag == false && x.CompleteFlag == false && HttpContext.Session.GetString("UserId") == x.UserId).ToList().Count();

            if (NoneTasks > 0)
            {
                ViewData["TaskAlert"] = "残りのタスクは " + NoneTasks + "個です。\r\n終わってねぇじゃんwwwwwww";
                ViewData["PngAlert"] = "https://2.bp.blogspot.com/-0X7AXvzJma0/WerLDbnoOkI/AAAAAAABHsk/TPkUxX5a7388t3xhUZTZ5CdC2aWWvZccgCLcBGAs/s800/pose_kage_warau_man.png";
            }
            else
            {
                ViewData["TaskAlert"] = "残りのタスクは " + NoneTasks + "個です。\r\nなんと素晴らしいことでしょう！";
                ViewData["PngAlert"] = "https://4.bp.blogspot.com/-PSd2xMIqM5M/VJ6Xcu17JhI/AAAAAAAAqLE/wSlWHEFvLIg/s800/hakusyu.png";
            }



            var task_list = _db.TaskTables.Where(x => x.DeleteFlag == false && x.CompleteFlag == false && HttpContext.Session.GetString("UserId") == x.UserId)
            .ToList()
            .Select(e => new TaskForm
            {
                TaskId = e.TaskId,
                TaskName = e.TaskName,
                Priority = e.MyPriority,
                Order = GetDisplayOrder(e.MyPriority),
                Url = Url.Action("Detail", new { taskId = e.TaskId })
            }).OrderBy(e => e.Order).ToList();
            return View(task_list);


        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult AddTask()
        {
            return RedirectToAction("AddTask", "AddTask");
        }

        public int GetDisplayOrder(string priority)
        {
            switch(priority)
            {
                case "高":
                    return 1;
                case "中":
                    return 2;
                default:
                    return 3;
            }
        }

        public Boolean IsLogin(string userid)
        {
            if (userid == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}