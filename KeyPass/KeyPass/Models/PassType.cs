using System.ComponentModel.DataAnnotations;

namespace KeyPass.Models
{
    public class PassType
    {
        public int Id { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Продолжительность в днях")]
        public int Duration { get; set; }
        [Display(Name = "Описание")]
        public string? Description { get; set; }
        [Display(Name = "Подразделение")]
        public virtual Department? Department { get; set; }
        [Display(Name = "Подразделение")]
        public int DepartmentId { get; set; }
        public virtual ICollection<Pass> Passes { get; } = new List<Pass>();
    }
}
