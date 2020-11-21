using System.Collections.Generic;
using Models;

namespace Interfaces
{
    public interface IDatabase
    {
        void SaveGameState(GameState gs);
        List<GameState> GetGameStateHistory();
    }

}
