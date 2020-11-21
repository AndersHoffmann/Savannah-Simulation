using System;
using System.Collections.Generic;
using System.Text;
using Interfaces;
using Models;

namespace Persistence
{
    public class Database : IDatabase
    {
        public void SaveGameState(GameState gs)
        {
            throw new NotImplementedException();
        }

        public List<GameState> GetGameStateHistory()
        {
            throw new NotImplementedException();
        }
    }
}
