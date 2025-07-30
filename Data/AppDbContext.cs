using Microsoft.EntityFrameworkCore;
using RetroGamesLauncher.Models;

namespace RetroGamesLauncher.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<GameInfo> Games { get; set; }
        public DbSet<GameGender> GameGenders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Cria o banco no diretório do app
            string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "games.db");
            options.UseSqlite($"Data Source={dbPath}");
        }
    }
}
