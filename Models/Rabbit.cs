using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Models
{
    public class Rabbit : Animal
    {
        //Properties
        public override double Weight { get; set; } = 10;
        public override int MovementRange { get; set; } = 2;
        public override int ReproductionRate { get; set; } = 4;

        private static int _counter;
        //Constructor
        public Rabbit()
        {
            Interlocked.Increment(ref _counter);
            Id = _counter;
        }

        //Methods
        public override void Move()
        {
            Weight -= 1;
        }

        public override void Eat()
        {
            Weight += 5;
        }

        public override string ToString()
        {
            return $"R{Id}";
           
        }
    }
}
