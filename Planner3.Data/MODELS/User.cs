using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner3.Data.MODELS;

[Table("User")]
public class User
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; }

    [Required, MaxLength(100)]
    public string Email { get; set; }

    [Required, MaxLength(100)]
    public string Password { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }

    [MaxLength(10)]
    public string Gender { get; set; }

    public decimal Height { get; set; }
    public decimal Weight { get; set; }

    [MaxLength(50)]
    public string ExperienceLevel { get; set; }

    [MaxLength(255)]
    public string Goals { get; set; }

}
