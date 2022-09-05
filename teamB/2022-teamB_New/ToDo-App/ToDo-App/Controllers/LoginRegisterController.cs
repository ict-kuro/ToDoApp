using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using ToDo_App.Models;
using ToDo_App.ViewModels;
using static Azure.Core.HttpHeader;

namespace ToDo_App.Controllers
{
    public class LoginRegisterController : Controller
    {
        private readonly TeamBContext _db;
        public LoginRegisterController(TeamBContext db) => _db = db;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(LoginRegister loginRegister)
        {
            int name_len;
            int pass_len;

            if (!ModelState.IsValid)
            {
                return View("Index", loginRegister);
            }

            var userID_check = _db.Users.Where(x => x.UserId == loginRegister.UserId).FirstOrDefault();

            if (userID_check != null)
            {
                ViewData["message"] = "既にユーザーIDが使用されています。";
                return View("Index");
            }

            var user = new User
            {
                UserId = loginRegister.UserId,
                Password = loginRegister.Password,
                LastLogDate = DateTime.Now
            };

            _db.Add(user);
            await _db.SaveChangesAsync();

            ViewData["message"] = "新規登録が完了しました。";
            //await Task.Delay(1000);
            RedirectToPage("Login");

            return View(nameof(Index));
        }

        
    }
}
