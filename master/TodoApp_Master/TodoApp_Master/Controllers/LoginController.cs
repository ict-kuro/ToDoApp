using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using TodoApp_Master.Models;
using TodoApp_Master.ViewModels;

/*ログイン認証、ユーザー登録を行うコントローラー*/

namespace TodoApp_Master.Controllers
{
    public class LoginController : Controller
    {
        private readonly TodoAppContext _db;
        public LoginController(TodoAppContext db) => _db = db;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(LoginForm loginForm)
        {


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
                    //Console.WriteLine("存在します");
                }
                else
                {
                    Directory.CreateDirectory(filePath);
                    //Console.WriteLine("存在しません");
                }

                //ログイン成功時はタスク表示画面へ移動
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["message"] = "ログイン情報に誤りがあります";
                return View("Index");
            }
        }

        /*ユーザー登録部分*/
        public IActionResult UserRegister()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserRegister(LoginRegister loginRegister)
        {
            if (!ModelState.IsValid)
            {
                return View("UserRegister", loginRegister);
            }

            var userID_check = _db.Users.Where(x => x.UserId == loginRegister.UserId).FirstOrDefault();

            if (userID_check != null)
            {
                ViewData["message"] = "既にユーザーIDが使用されています。";
                return View("UserRegister");
            }

            if(loginRegister.Password != loginRegister.tempPassword)
            {
                ViewData["message"] = "再入力されたパスワードが異なります";
                return View("UserRegister");
            }

            var user = new User
            {
                UserId = loginRegister.UserId,
                Password = loginRegister.Password
            };

            _db.Add(user);
            await _db.SaveChangesAsync();

            ViewData["message"] = "新規登録が完了しました。";
            //await Task.Delay(1000);
            return RedirectToAction("Index");
        }
    }
}
