using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Threading.Channels;
using Models;

namespace Logic
{
    public class BoardLogic
    {
        //Properties
        public int AmountOfLions { get; }
        public int AmountOfRabbits { get; }
        public static int BoardSize { get; set; }
        public List<List<Field>> Board { get; set; }
        //Constructor
        public BoardLogic(int amountOfLions, int amountOfRabbits, int boardSize = 20)
        {
            AmountOfLions = amountOfLions;
            AmountOfRabbits = amountOfRabbits;
            BoardSize = boardSize;

            Board = new List<List<Field>>();

            //Setup new simulation:
            CreateBoard();
            CreateAnimals();
        }
        //Methods
        private void CreateBoard()
        {
            for (int i = 0; i < BoardSize; i++)
            {
                Board.Add(new List<Field>());

                for (int j = 0; j < BoardSize; j++)
                {
                    Board[i].Add(new Field(GrassChance()));
                }
            }
        }
        private static bool GrassChance()
        {
            Random R = new Random();

            var roll = R.Next(1, 6);

            var isGrass = roll == 5;

            return isGrass;
        }
        private void CreateAnimals()
        {
            for (int i = 0; i < AmountOfLions; i++)
            {
                (int xCoordinate,int yCoordinate) = RandomCoordinates();

                while (Board[xCoordinate][yCoordinate].AnimalOnField != null)
                {
                    (xCoordinate, yCoordinate) = RandomCoordinates();
                }



                Animal tempLion = new Lion();
                Board[xCoordinate][yCoordinate].AnimalOnField = tempLion;
                Board[xCoordinate][yCoordinate].AnimalOnField.AnimalGender = RandomGender();
            }

            for (int i = 0; i < AmountOfRabbits; i++)
            {
                (int xCoordinate, int yCoordinate) = RandomCoordinates();

                while (Board[xCoordinate][yCoordinate].AnimalOnField != null)
                {
                    (xCoordinate, yCoordinate) = RandomCoordinates();
                }
                var tempRabbit = new Rabbit();
                Board[xCoordinate][yCoordinate].AnimalOnField = tempRabbit;
                Board[xCoordinate][yCoordinate].AnimalOnField.AnimalGender = RandomGender();
            }

        }
        private static Gender RandomGender()
        {
            Random r = new Random();

            int roll = r.Next(0, 4);

            return roll == 0 ? Gender.Female : Gender.Male;
        }
        public void SimulateRound()
        {
            List<Animal> usedAnimals = new List<Animal>();

            foreach (var field in Board.SelectMany(s=>s))
            {
                if (field.AnimalOnField != null && !usedAnimals.Contains(field.AnimalOnField))
                {
                    usedAnimals.Add(field.AnimalOnField);
                    MoveAnimal(field);
                }
            }
            Eat();

            Procreate();
        }
        private void Procreate()
        {
            List<Animal> usedAnimals = new List<Animal>();

            Random r = new Random();

            foreach (var field in Board.SelectMany(s=>s))
            {
                if (field.AnimalOnField == null || usedAnimals.Contains(field.AnimalOnField)) continue;

                var listOfSuitableMates = GetListOfSuitableMates(field);

                if (listOfSuitableMates.Count <= 0) continue;

                var chosenMate = listOfSuitableMates[r.Next(listOfSuitableMates.Count)].AnimalOnField;
                usedAnimals.Add(field.AnimalOnField);
                usedAnimals.Add(chosenMate);

                (int xCoordinate, int yCoordinate) = RandomCoordinates();
                while (Board[xCoordinate][yCoordinate].AnimalOnField!= null)
                {
                    (xCoordinate, yCoordinate) = RandomCoordinates();
                }

                if (chosenMate is Lion)
                {
                    for (int i = 0; i < chosenMate.ReproductionRate; i++)
                    {
                        Board[xCoordinate][yCoordinate].AnimalOnField = new Lion {AnimalGender = RandomGender()};
                        usedAnimals.Add(Board[xCoordinate][yCoordinate].AnimalOnField);
                        usedAnimals.Add(chosenMate);
                    }
                           
                }
                else if (chosenMate is Rabbit)
                {
                    for (int i = 0; i < chosenMate.ReproductionRate; i++)
                    {
                        Board[xCoordinate][yCoordinate].AnimalOnField = new Rabbit {AnimalGender = RandomGender()};
                        usedAnimals.Add(Board[xCoordinate][yCoordinate].AnimalOnField);
                        usedAnimals.Add(chosenMate);
                    }
                           
                }
            }
        }
        public void Eat()
        {
            foreach (var field in Board.SelectMany(s => s))
            {
                if (field.AnimalOnField is Lion)
                {
                    EatRabbit(field);
                }
                else if (field.AnimalOnField is Rabbit)
                {
                    EatGrass(field);
                }
            }
        }
        private void EatGrass(Field field)
        {
            if (field.IsGrass == true)
            {
                field.AnimalOnField.Eat();
            }
        }
        public void EatRabbit(Field field)
        {
            Random R = new Random();

            var chosenPray = GetFieldsWithRabbitsInRange(field).ElementAtOrDefault(R.Next(GetFieldsWithRabbitsInRange(field).Count));

            if (chosenPray != null)
            {
                //Remove the Rabbit from field and AllAnimals
                chosenPray.AnimalOnField = null;
                //Lion gains weight
                field.AnimalOnField.Eat();
            }
        }
        private void MoveAnimal(Field field)
        {
            var validFields = GetValidField(field);
            
            var tempAnimal = field.AnimalOnField;

            field.AnimalOnField = null;

            var animal = Board.SelectMany(s => s).First(s => s == validFields).AnimalOnField;


            Board.SelectMany(s => s).First(s => s == validFields).AnimalOnField = tempAnimal;

            Board.SelectMany(s => s).First(s => s == validFields).AnimalOnField.Move();

            if (Board.SelectMany(s => s).First(s => s == validFields).AnimalOnField.Weight < 0)
            {
                Board.SelectMany(s => s).First(s => s == validFields).AnimalOnField = null;
            }
        }
        public Field GetValidField(Field field)
        {
            List<Field> validFields = GetListOfValidMoves(field);

            if (validFields.Count < 1)
            {
                return field;
            }
            Random R = new Random();
            return validFields[R.Next(0, validFields.Count)];
        }
        public List<Field> GetSurroundingFields(Field field)
        {
            (int xCoordinate, int yCoordinate) = GetXY(field);

            List<(int x, int y)> options = GetFields(field.AnimalOnField.MovementRange);

            var surroundingFields = Board.SelectMany(s => s)
                .Where(p => options.Contains(
                    (GetXY(p).Item1 - xCoordinate, GetXY(p).Item2 - yCoordinate)))
                .ToList();

            return surroundingFields;
        }
        public List<Field> GetFieldsWithRabbitsInRange(Field field)
        {
            return GetSurroundingFields(field).Where(p => p.AnimalOnField is Rabbit).ToList();
        }
        private List<Field> GetListOfSuitableMates(Field field)
        {
            return GetSurroundingFields(field).Select(s => s).Where(s => s.AnimalOnField != null)
                .Where(s => s.AnimalOnField.AnimalGender != field.AnimalOnField.AnimalGender && s.AnimalOnField.GetType() == field.AnimalOnField.GetType()).ToList();

        }
        public List<Field> GetListOfValidMoves(Field field)
        {
            return GetSurroundingFields(field).Where(s => s.AnimalOnField == null).ToList();
        }
        private List<(int x, int y)> GetFields(int input)
        {
            List<(int, int)> directions = new List<(int, int)>();

            while (input > 0)
            {
                directions.AddRange(new List<(int, int)>() { (input, input), (input, -input), (-input, -input), (-input, input), (-input, 0), (0, -input), (input, 0), (0, input) });
                input--;
            }

            return directions;
        }
        public (int, int) GetXY(Field field)
        {
            var targetList = Board.Select(s => s).First(s => s.Contains(field));
            int xCoordinate = Board.FindIndex(s => s == targetList);
            int yCoordinate = targetList.FindIndex(s => s == field);
            
            return (xCoordinate, yCoordinate);
        }
        private static (int, int) RandomCoordinates()
        {
            Random R = new Random();
            return (R.Next(0, BoardSize), R.Next(0, BoardSize));
        }
    }
}