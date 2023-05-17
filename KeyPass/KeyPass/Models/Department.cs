using System.ComponentModel.DataAnnotations;

namespace KeyPass.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string? Description { get; set; }
        public virtual ICollection<User> Users { get; } = new List<User>();
    }
}
