using RetroGamesLauncher.Models;

namespace RetroGamesLauncher.Data.Repositories
{
    public interface IGameGenderRepository
    {   List<GameGender> GetAll();
        GameGender GetById(int id);
        GameGender GetByGender(string title);
        Task<List<GameGender>> GetByGenderLike(string title);
        int GetTotalCount();
        void Add(GameGender gameGender);
        void Update(GameGender gameGender);
        void Delete(int id);
    }
}
