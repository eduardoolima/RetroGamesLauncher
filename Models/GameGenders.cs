using System.ComponentModel.DataAnnotations;

namespace RetroGamesLauncher.Models;
public class GameGender
{    
    [Key]
    public int Id { get; set; }
    [Required]
    public string Gender { get; set; }

    public GameGender(string gender)
    {
        Gender = gender;
    }
}
