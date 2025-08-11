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
    /// <summary>
    /// Retorna todos os jogos cadastrados no banco, incluindo o gênero associado.
    /// </summary>
    public List<GameInfo> GetAll()
    {
        try
        {
            return _context.Games.Include(g => g.Gender).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving games from the repository", ex);
        }
    }

    /// <summary>
    /// Retorna uma lista paginada de jogos, ordenados por título.
    /// </summary>
    /// <param name="pageIndex">Número da página (iniciando em 1).</param>
    /// <param name="pageSize">Quantidade de registros por página.</param>
    public async Task<List<GameInfo>> GetByPaging(int pageIndex, int pageSize)
    {
        try
        {
            int offset = (pageIndex - 1) * pageSize;
            return await _context.Games
                .Include(g => g.Gender)
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

    /// <summary>
    /// Retorna um jogo pelo ID informado.
    /// </summary>
    /// <param name="id">ID do jogo.</param>
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

    /// <summary>
    /// Retorna um jogo pelo título exato (ignorando maiúsculas e minúsculas).
    /// </summary>
    /// <param name="title">Título do jogo.</param>
    public GameInfo GetByTitle(string title)
    {
        try
        {
            return _context.Games.Include(g => g.Gender).FirstOrDefault(g => g.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                   ?? throw new KeyNotFoundException($"Game with title '{title}' not found.");
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving game by title from the repository", ex);
        }
    }

    /// <summary>
    /// Retorna todos os jogos cujo título contenha o texto informado (busca parcial).
    /// </summary>
    /// <param name="title">Parte do título a ser buscada.</param>
    public async Task<List<GameInfo>> GetByTitleLike(string title)
    {
        try
        {
            return await _context.Games.Include(g => g.Gender)
                .Where(g => EF.Functions.Like(g.Title, $"%{title}%"))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error retrieving games by similar title from the repository", ex);
        }
    }

    /// <summary>
    /// Retorna a quantidade total de jogos cadastrados.
    /// </summary>
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

    /// <summary>
    /// Adiciona um novo jogo ao banco de dados.
    /// </summary>
    /// <param name="game">Objeto do jogo a ser adicionado.</param>
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

    /// <summary>
    /// Remove um jogo pelo ID informado.
    /// </summary>
    /// <param name="id">ID do jogo a ser removido.</param>
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

    /// <summary>
    /// Atualiza os dados de um jogo existente.
    /// </summary>
    /// <param name="game">Objeto do jogo com os novos dados.</param>
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
