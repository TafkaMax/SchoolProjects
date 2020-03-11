using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GameSetting
    {
        public int GameSettingId { get; set; }
        [Display(Name = "Game Name")] public string GameName { get; set; } = "MineSweeper";

        [Display(Name = "Board Height"), Range(9, 20)]
        public int BoardHeight { get; set; } = 9;

        [Display(Name = "Board Width"), Range(9, 20)]
        public int BoardWidth { get; set; } = 9;

        [Display(Name = "Difficulty Level"), Range(1, 3)]
        public int DifficultyLevel { get; set; } = 1;
    }
}