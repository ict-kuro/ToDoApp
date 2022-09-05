using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.RegularExpressions;
using ToDo_App.Models;
using ToDo_App.ViewModels;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Http;


namespace ToDo_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly TeamBContext _db;
        public HomeController(TeamBContext db) => _db = db;

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login(LoginForm loginForm)
        {
            //Console.WriteLine(username);
            //Console.WriteLine(password);


            if (!ModelState.IsValid)
            {
                return View("Index", loginForm);
            }


            var user = _db.Users.Where(x => x.UserId == loginForm.UserId && x.Password == loginForm.Password).FirstOrDefault();

            //nullならばログイン失敗
            if (user != null)
            {
                //セッションにログインIDを保存
                HttpContext.Session.SetString(SetSession.SessionUserId, user.UserId);

                //ログイン時に画像を保存するディレクトリがなければディレクトリ作成を行う
                string filePath1 = "wwwroot/IMG";
                string filePath2 = user.UserId;
                string filePath = System.IO.Path.Combine(filePath1, filePath2);
                Console.WriteLine(filePath);
                if (Directory.Exists(filePath))
                {
                    Console.WriteLine("存在します");
                }
                else
                {
                    Directory.CreateDirectory(filePath);
                    Console.WriteLine("存在しません");
                }

                //ログイン成功時はタスク表示画面へ移動
                return RedirectToAction("Index", "Display");
            }
            else
            {
                ViewData["message"] = "ログイン情報に誤りがあります";
                return View("Index");
            }


        }
    }
}
