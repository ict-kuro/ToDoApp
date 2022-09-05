namespace ToDo_App.Models
{
    public class SetSession
    {
        //ログインしているユーザーIDをセッションで保持
        public const string SessionUserId = "_ID";
        public const string SessionTaskId = "0";
        public const string SessionGroupname = "default";
        public const string SessionComment = "default";
        public const string SessionPriority = "0";
        public const string SessionTaskname = "default";
        public DateTime SessionRegisterDate = DateTime.Now;
        public DateTime SessionDeadlineDate = DateTime.Now;

    }
}
