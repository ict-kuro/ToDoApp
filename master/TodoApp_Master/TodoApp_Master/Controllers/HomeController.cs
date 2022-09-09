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

        public IActionResult Index(TaskHome taskHome)
        {
            //ログイン状態を確認
            taskHome.LoginStatus = Misc.IsLogin(SetSession.SessionUserId);
           
            return View(taskHome);
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
    }
}