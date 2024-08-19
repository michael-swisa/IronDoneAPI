using System.ComponentModel.DataAnnotations;
using IronDoneAPI.Enums;

namespace IronDoneAPI.Models
{
    public class Attack
    {
        public Guid? id { get; set; }

        [AllowedValues("Iran", "Chutim")]
        public string? Origin { get; set; }
        public int? MissileCount { get; set; }
        public List<string>? MissileTypes { get; set; } = new List<string>();
        public AttackStatus? Status { get; set; }
        public DateTime? StartedAt { get; set; }
    }
}
