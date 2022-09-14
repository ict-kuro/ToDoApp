/*タスク表示とカレンダー表示のコントローラー*/

using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TodoApp_Master.Models;
using TodoApp_Master.ViewModels;
using TodoApp_Master.Miscs;


namespace TodoApp_Master.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TodoAppContext _db;
        public HomeController(ILogger<HomeController> logger, TodoAppContext db)
        {
            _logger = logger;
            _db = db;
        }

        /*カレンダー用のタスク情報抽出*/
        public IActionResult GetAllTasks(DateTime start, DateTime end)
        {

            var res = _db.TaskTables.Where(x => x.DeleteFlag != true && HttpContext.Session.GetString(SetSession.SessionUserId) == x.UserId).Select(e => new
            {
                Id = e.TaskId,
                Title = e.TaskName,
                Start = e.StartTime,
                End = e.DeadLine,
                //color = e.TaskStatus ? "#aaaaaa" : e.ColorId,
                //textColor = e.CompleteFlag ? "black" : "#34ebe8",
                url = Url.Action("Index", new { id = e.TaskId })
            }).ToList();
            return Json(res);
        }

        public IActionResult Index()
        {
            //ログイン状態を確認
            ViewData["LoginStatus"] = Misc.IsLogin(HttpContext.Session.GetString(SetSession.SessionUserId));
            string userId = HttpContext.Session.GetString(SetSession.SessionUserId);
            if(userId == null)
            {
                return View();
            }

            //タスクテーブルからユーザーIDが等しく、未完了のものをリストで取得
            var Tasks = _db.TaskTables.Where(x => x.UserId == userId && x.TaskStatus == false).ToList();

            return View(Tasks);
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

        //タスク登録ウインドウ処理
        public IActionResult ShowModal()
        {
            var userid = HttpContext.Session.GetString(SetSession.SessionUserId);
            var user = _db.Users.Where(x => x.UserId == userid).FirstOrDefault();

            return PartialView("_Modal",user);
        }
    }
}