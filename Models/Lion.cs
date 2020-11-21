using System;
using System.Threading;

namespace Models
{
    public class Lion : Animal
    {

        //Properties
        public override double Weight { get; set; } = 80;
        public override int MovementRange { get; set; } = 1;
        public override int ReproductionRate { get; set; } = 1;

        private static int _counter;

        //Constructor
        public Lion()
        {
            Interlocked.Increment(ref _counter);
            Id = _counter;
        }

        //Methods
        public override void Move()
        {
            Weight -= 2;
        }

        public override void Eat()
        {
            Weight += 5;
        }

        public override string ToString()
        {
            return $"L{Id}";
        }
    }
}
