namespace Domain
{
    public class Cell
    {
        public int CellId { get; set; } = default!;

        public int x { get; set; } = default!;

        public int y { get; set; } = default!;

        public int GameId { get; set; } = default!;

        public Game Game { get; set; } = default!;

        public bool IsBomb { get; set; } = default!;

        public bool IsFlag { get; set; } = default!;

        public bool IsOpened { get; set; } = default!;
        
    }
    
    
}