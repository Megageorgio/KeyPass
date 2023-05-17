using System.ComponentModel.DataAnnotations;

namespace KeyPass.Models
{
    public class Pass
    {
        public int Id { get; set; }
        [Display(Name = "Дата начала действия")]
        public DateTime IssueDate { get; set; }
        [Display(Name = "Дата окончания действия")]
        public DateTime? ExpirationDate { get; set; }
        [Display(Name = "Тип пропуска")]
        public virtual PassType? PassType { get; set; }
        [Display(Name = "Тип пропуска")]
        public int PassTypeId { get; set; }
        [Display(Name = "Сотрудник")]
        public virtual User? User { get; set; }
        [Display(Name = "Сотрудник")]
        public string UserId { get; set; }
    }
}