using RetroGamesLauncher.Models;

namespace RetroGamesLauncher.Data.Repositories;
public interface IGameRepository
{
    List<GameInfo> GetAll();
    Task<List<GameInfo>> GetByPaging(int pageIndex, int pageSize);
    GameInfo GetById(int id);
    GameInfo GetByTitle(string title);
    int GetTotalCount();
    void Add(GameInfo game);
    void Update(GameInfo game);
    void Delete(int id);
}

