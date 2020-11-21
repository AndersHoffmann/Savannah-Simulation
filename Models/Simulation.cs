using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Simulation
    {
        public DateTime StartingTime { get; set; }
        public DateTime EndingTime { get; set; }
        public List<GameState> GameStateHistory { get; set; }


    }
}
