using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ToDo.ViewModels
{
    public class AddTaskForm : IValidatableObject
    {
      
        //タスク名
        [Column("task_name")]
        [Required(ErrorMessage = "必須項目です")]
        [StringLength(40, ErrorMessage = "文字数が多すぎます")]
        public string TaskName { get; set; } = null!;

        //開始時間
        [Column("start_time", TypeName = "datetime")]
        [Required(ErrorMessage = "必須項目です")]
        public DateTime? StartTime { get; set; }

        //終了時間
        [Column("finish_time", TypeName = "datetime")]
        [Required(ErrorMessage = "必須項目です")]
        public DateTime? FinishTime { get; set; }


        //コメント
        [Column("comment")]
        [StringLength(100, ErrorMessage = "文字数が多すぎます")]
        public string? Comment { get; set; }


        //優先順位
        [Column("my_priority")]
        [StringLength(4, ErrorMessage = "不正な値が入力されています")]
        public string MyPriority { get; set; } = null!;


        //色
        [Column("color_id")]
        [StringLength(12, ErrorMessage = "不正な値が入力されています")]
        [Unicode(false)]
        public string ColorId { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(StartTime >= FinishTime)
            {
                yield return
                    new ValidationResult($"終了時間は開始時間よりも後の日時を設定してください"
                    , new[] { nameof(FinishTime) });
            }

            if (DateTime.Now >= StartTime)
            {
                yield return
                    new ValidationResult($"現在日時よりも後の日時を設定してください"
                    , new[] { nameof(StartTime) });
            }
        }       
    }
}
