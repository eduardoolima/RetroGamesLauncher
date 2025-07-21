using RetroGamesLauncher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroGamesLauncher.Data.Repositories
{
    public interface IGameRepository
    {
        IEnumerable<GameInfo> GetAll();
        GameInfo GetById(int id);
        GameInfo GetByTitle(string title);
        void Add(GameInfo game);
        void Update(GameInfo game);
        void Delete(int id);
    }
}
