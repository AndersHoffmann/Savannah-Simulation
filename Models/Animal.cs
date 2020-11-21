using System.Collections.Generic;

namespace Models
{
    public enum Gender
    {
        Male, Female
    }
    public abstract class Animal
    {
        public abstract double Weight { get; set; }
        public abstract int MovementRange { get; set; }
        public abstract int ReproductionRate { get; set; }
        public Gender AnimalGender { get; set; }
        public int Id { get; set; }

        public abstract void Move();
        public abstract void Eat();
    }
}
