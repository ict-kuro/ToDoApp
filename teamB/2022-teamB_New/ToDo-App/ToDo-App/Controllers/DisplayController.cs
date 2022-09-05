using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using ToDo_App.Models;
//using ToDo_App.ViewModels;


namespace ToDo_App.Controllers
{
    public class DisplayController : Controller
    {
        private readonly TeamBContext _db;
        //ホストのルートパス環境を取得
        private readonly IWebHostEnvironment _env;
        //コンストラクト
        public DisplayController(IWebHostEnvironment env, TeamBContext db)
        {
            _env = env;
            _db = db;
        }

        public IActionResult Index(string sortid)
        {
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
           
            //タスクテーブルからユーザーIDが等しく、未完了のものをリストで取得
            var Tasks = _db.Tasks.Where(x => x.UserId == UserId && x.TaskStatus == false).ToList();

            //sortidをもとにソート処理する idがない場合はデフォルトで期限が近い順
            switch (sortid)
            {
                //期限が近い順
                case "deadlineDec":
                    Tasks = Tasks.OrderByDescending(x => x.DeadlineDate).ToList();
                    break;
                //期限が遠い順
                case "deadlineAsc":
                    Tasks = Tasks.OrderBy(x => x.DeadlineDate).ToList();
                    break;
                //優先度高い順
                case "priority":
                    Tasks = Tasks.OrderBy(x => x.Priority).ToList();
                    break;
                //登録日が近い順
                case "registDec":
                    Tasks = Tasks.OrderByDescending(x => x.RegisterDate).ToList();
                    break;
                //期限が近い順
                default :
                    Tasks = Tasks.OrderByDescending(x => x.DeadlineDate).ToList();
                    break;
            }

            
            //テスト用
            //Console.WriteLine(UserId);
            //ビューにタスクのリストを渡す
            return View(Tasks);
        }

        //タスク詳細の表示
        //ビューから渡されたタスクIDと一致するタスクの情報を取得する
        public IActionResult Task_det(string id)
        {
            ViewData["SessionUserId"] = null;
            ViewData["TaskId"] = null;

            //ログインされている場合
            if (HttpContext.Session.GetString(SetSession.SessionUserId) != null)
            {
                ViewData["SessionUserId"] = HttpContext.Session.GetString(SetSession.SessionUserId);
            }
            else
            {
                //入っていない場合はそのまま
                return View();
            }

            //タスクidが指定されていない場合
            if (id == null)
            {
                return View();
            }
            else
            {
                ViewData["TaskId"] = id;
                //セッションにタスクIDを保存
                HttpContext.Session.SetInt32(SetSession.SessionTaskId, int.Parse(id));
            }

            var Tasks = _db.Tasks.Where(x => x.TaskId == int.Parse(id)).FirstOrDefault();
            
            //タスクのグループIDと一致するグループ名を取得
            var Group = _db.TaskGroups.Where(x => x.GroupId == Tasks.GroupId).FirstOrDefault();

            if (Tasks.ImagePath != null)
            {
                string filePath = "IMG/";
                filePath += HttpContext.Session.GetString(SetSession.SessionUserId);
                filePath += "/";
                filePath += Tasks.ImagePath;
                var filepath1 = _env.MapWebRootPath(filePath);
                
                //ファイルの存在チェック
                if (System.IO.File.Exists(filepath1))
                {
                    var filepath2 = Path.Combine("https://localhost:7215",filePath);
                    Console.WriteLine("存在します");
                    ViewData["ImagePath"] = filepath2;
                }
                
            }

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
            //優先度の数値を日本語に反映
            /*if (Tasks.Priority == 4)
            {
                ViewData["Priority"] = "高";
            }
            else if (Tasks.Priority == 5)
            {
                ViewData["Priority"] = "中";
            }
            else
            {
                ViewData["Priority"] = "低";
            }
            */
            //グループ名を確認
            ViewData["Groupname"] = Group.Groupname;
            return View(Tasks);
        }

        //タスク完了処理
        public IActionResult Task_Comp(string[] comp)
        {
            for(int i = 0; i < comp.Length; i++)
            {
                int compId = int.Parse(comp[i]);
                var compTask = _db.Tasks.Where(x => x.TaskId == compId).FirstOrDefault();
                compTask.TaskStatus = true;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //完了タスク表示
        public IActionResult Task_Result()
        {

            string UserId;
            //ページを開いた時の日時
            DateTime dt = DateTime.Now;

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
                return View();
            }

            //ユーザーが登録している全てのタスク
            var AllTasks = _db.Tasks.Where(x => x.UserId == UserId).ToList();
            //完了しているタスク
            var ResultTasks = _db.Tasks.Where(x => x.UserId == UserId && x.TaskStatus == true).ToList();
            //タスクの個数(総数、完了数)
            float AllTaskCnt = 0, ResultTaskCnt = 0;
            //dtの値から、今月が期限になっているタスクの個数を求める
            foreach(var items in AllTasks)
            {
                if(items.DeadlineDate.Month == dt.Month)
                {
                    AllTaskCnt++;
                }
            }
            //dtの値から今月が期限になっている完了タスクの個数を求める
            foreach(var items in ResultTasks)
            {
                if(items.DeadlineDate.Month == dt.Month)
                {
                    ResultTaskCnt++;
                }
            }

            ViewData["NotResultTasks"] = AllTaskCnt - ResultTaskCnt;
            
            //達成率を求める            
            ViewData["TaskRatio"] = Math.Floor((ResultTaskCnt / AllTaskCnt) * 100.0);
        
            return View(ResultTasks);
        }

        public IActionResult TaskCalendar()
        {
            return View();
        }
    }
}
