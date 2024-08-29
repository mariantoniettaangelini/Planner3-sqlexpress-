using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planner3.Data.MODELS;

[Table("Progress")]
public class Progress
{
    [Key]
    public int Id { get; set; }
    public int WorkoutSessionId { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public WorkoutSession? WorkoutSession { get; set; }
    public bool IsCompleted { get; set; }
}
