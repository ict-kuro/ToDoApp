using System.ComponentModel.DataAnnotations;

namespace ToDo_App.ViewModels
{
    public class UpdateForm
    {
        [Required(ErrorMessage = "期限日は必須入力項目です。")]
        public DateTime DeadlineDate { get; set; }
        [Required(ErrorMessage = "開始日は必須入力項目です。")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "優先度は必須入力項目です。")]
        public int Priority { get; set; }
        [StringLength(65535, ErrorMessage = "コメントは65535文字までです。")]
        public string? Comment { get; set; }
        [Required(ErrorMessage = "タスク名は必須入力項目です。")]
        [StringLength(255, ErrorMessage = "タスク名は255文字までです。")]
        public string Taskname { get; set; } = null!;
        public int TaskId { get; set; }
        public bool? TaskStatus { get; set; }
        public int GroupId { get; set; }
        public string? ImageName { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
