using RetroGamesLauncher.Models;

namespace RetroGamesLauncher.Data.Repositories;

public class GameGenderRepository : IGameGenderRepository
{
    private readonly AppDbContext _context;
    public GameGenderRepository(AppDbContext context)
    {
        _context = context;
    }
    public void Add(GameGender gameGender)
    {
        _context.GameGenders.Add(gameGender);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public List<GameGender> GetAll()
    {
        return _context.GameGenders.OrderBy(g => g.Gender).ToList();
    }

    public GameGender GetByGender(string title)
    {
        throw new NotImplementedException();
    }

    public Task<List<GameGender>> GetByGenderLike(string title)
    {
        throw new NotImplementedException();
    }

    public GameGender GetById(int id)
    {
        throw new NotImplementedException();
    }

    public int GetTotalCount()
    {
        throw new NotImplementedException();
    }

    public void Update(GameGender gameGender)
    {
        throw new NotImplementedException();
    }
}
