namespace Models
{
    public class Field
    {
        public Animal AnimalOnField { get; set; }
        public bool IsGrass { get; set; } 

        public Field(bool isGrass)
        {
            IsGrass = isGrass;
        }
    }
}