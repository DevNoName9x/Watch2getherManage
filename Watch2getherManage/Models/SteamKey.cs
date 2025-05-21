using System.ComponentModel.DataAnnotations;

namespace Watch2getherManage.Models
{
    public class SteamKey
    {
        [Key]
        public required string StreamKey { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
