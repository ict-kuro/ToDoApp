/*その他細かい関数をまとめたクラス*/
namespace TodoApp_Master.Miscs
{
    public class Misc
    {
        //ログインしているかの確認メソッド
        public static Boolean IsLogin(string userid)
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
