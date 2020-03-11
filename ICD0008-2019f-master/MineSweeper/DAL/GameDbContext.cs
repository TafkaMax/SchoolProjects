using System.Linq;
using Domain;
using Microsoft.EntityFrameworkCore;


namespace DAL
{
    public class GameDbContext : DbContext
    {
        public DbSet<GameSetting> GameSettings { get; set; } = default!;

        public DbSet<Game> Games { get; set; } = default!;

        public DbSet<Cell> Cells { get; set; } = default!;


        public GameDbContext(DbContextOptions<GameDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(
                optionsBuilder.UseSqlite(
                    @"Data source=../WebApplication/game_db.db"));
        }
    }
}