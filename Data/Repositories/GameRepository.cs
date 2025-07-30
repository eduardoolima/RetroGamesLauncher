using Microsoft.EntityFrameworkCore;
using RetroGamesLauncher.Models;

namespace RetroGamesLauncher.Data.Repositories;

public class GameRepository : IGameRepository
{
    private readonly AppDbContext _context;
    public GameRepository(AppDbContext context)
    {
        _context = context;
    }

    #region Get
    public List<GameInfo> GetAll()
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

    public async Task<List<GameInfo>> GetByPaging(int pageIndex, int pageSize)
    {
        try
        {
            int offset = (pageIndex - 1) * pageSize;
            return await _context.Games
                .OrderBy(g => g.Title)
                .Skip(offset)
                .Take(pageSize)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving paged games from the repository", ex);
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
    public async Task<List<GameInfo>> GetByTitleLike(string title)
    {
        try
        {
            return await _context.Games
                .Where(g => EF.Functions.Like(g.Title, $"%{title}%"))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving games by similar title from the repository", ex);
        }
    }

    public int GetTotalCount()
    {
        try
        {
            return _context.Games.Count();
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving total game count from the repository", ex);
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
