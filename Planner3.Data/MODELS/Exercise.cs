using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Planner3.Data.MODELS;

[Table("Exercise")]
public class Exercise
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Name { get; set; }
    [MaxLength(500)]
    public string Description { get; set; }
    [MaxLength(50)]
    public string Type { get; set; }
    public string MuscleGroup { get; set; }
    public string Image { get; set; }

    // Un esercizio in più sessioni
    [JsonIgnore]
    public ICollection<WorkoutSession> WorkoutSessions { get; set; }
}
