using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KeyPass.Models
{
    public class User : IdentityUser
    {
        [Display(Name = "ФИО")]
        public string FullName { get; set; }
        [Display(Name = "Подразделение")]
        public virtual Department? Department { get; set; }
        [Display(Name = "Подразделение")]
        public int? DepartmentId { get; set; }
        public virtual ICollection<Pass> Passes { get; } = new List<Pass>();
    }
}
