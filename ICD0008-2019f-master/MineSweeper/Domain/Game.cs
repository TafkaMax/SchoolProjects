using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Game
    {
        [Display(Name = "SaveGame ID")]
        public int GameId { get; set; } = default!;
        
        public bool FirstMove { get; set; } = default!;

        [Display(Name = "Save name")]
        public string GameName { get; set; } = default!;

        [Display(Name = "Board Height")]
        public int Height { get; set; } = default!;

        [Display(Name = "Board Width")]
        public int Width { get; set; } = default!;
        
        [Display(Name = "Difficulty level")]
        public int DifficultyLevel { get; set; }
        
        public bool GameLost { get; set; }

        public bool GameWon { get; set; }
        
        public ICollection<Cell> Cells { get; } = new List<Cell>();

    }
}