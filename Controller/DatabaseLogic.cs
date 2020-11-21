using System.Collections.Generic;
using Interfaces;
using Models;

namespace Logic
{
    public class DatabaseLogic
    {
        private readonly IDatabase _db;

        public DatabaseLogic(IDatabase db)
        {
            this._db = db;
        }

        public void SaveGameState(GameState gs)
        {
            _db.SaveGameState(gs);
        }

        public List<GameState> GetGameStateHistory()
        {
            return _db.GetGameStateHistory();
        }
    }
}
