using RetroGamesLauncher.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace RetroGamesLauncher.Models
{
    public class GameInfo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }        
        public string ImagePath { get; set; }        
        public string Description { get; set; }
        [Required]
        public string RomPath { get; set; }        
        public string ScreenshotPath { get; set; }
        [Required]
        public Emulators EmulatorId { get; set; }
    }    
}
