using RetroGamesLauncher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroGamesLauncher.Data.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly AppDbContext _context;
        public GameRepository(AppDbContext context)
        {
            _context = context;
        }

        #region Get
        public IEnumerable<GameInfo> GetAll()
        {
            try
            {
                return _context.Games.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving games from the repository", ex);
            }
        }

        public GameInfo GetById(int id)
        {
            try
            {
                return _context.Games.Find(id) ?? throw new KeyNotFoundException($"Game with Id {id} not found.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving game by Id from the repository", ex);
            }
        }

        public GameInfo GetByTitle(string title)
        {
            try
            {
                return _context.Games.FirstOrDefault(g => g.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                       ?? throw new KeyNotFoundException($"Game with title '{title}' not found.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving game by title from the repository", ex);
            }
        } 
        #endregion

        public void Add(GameInfo game)
        {
            try
            {
                _context.Games.Add(game);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding game to the repository", ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                _context.Games.Remove(_context.Games.Find(id));
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting game from the repository", ex);
            }
        }

        public void Update(GameInfo game)
        {
            try
            {
                var existingGame = _context.Games.Find(game.Id) ?? throw new KeyNotFoundException($"Game with Id {game.Id} not found.");
                existingGame.Title = game.Title;
                existingGame.Description = game.Description;
                existingGame.ImagePath = game.ImagePath;
                existingGame.ScreenshotPath = game.ScreenshotPath;
                existingGame.RomPath = game.RomPath;
                existingGame.EmulatorId = game.EmulatorId;
                _context.Games.Update(existingGame);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating game in the repository", ex);
            }
        }
    }
}
