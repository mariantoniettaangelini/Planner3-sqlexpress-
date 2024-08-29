using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner3.Data.MODELS;

[Table("WorkoutSession")]
public class WorkoutSession
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Level { get; set; }
    public int Duration { get; set; }
    public string Image {  get; set; }
    public string Type { get; set; }

    // Più esercizi in una sessione
    public ICollection<Exercise> Exercises { get; set; }
}
