using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using ConsoleTables;
using Logic;
using Interfaces;
using Models;
using Persistence;

namespace ConsoleTestEnvironment
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(130, 60);
            Console.SetWindowPosition(0, 0);


            IDatabase db = new Database();
            var dbCtrl = new DatabaseLogic(db);

            var boardCtrl = new BoardLogic(amountOfLions: 10, amountOfRabbits: 30);

            var ct = new ConsoleTable("C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "C10", "C11", "C12", "C13",
                "C14", "C15", "C16", "C17", "C18", "C19", "C20");

            foreach (var row in boardCtrl.Board)
            {
                ct.AddRow(row.Select(s => s.AnimalOnField).ToArray());
            }

            ct.Write(Format.Alternative);

            var animalList = boardCtrl.Board.SelectMany(s => s).Where(s => s.AnimalOnField != null)
                .Select(s => s.AnimalOnField);


            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(1000);
                boardCtrl.SimulateRound();
                Console.Clear();

                var ct2 = new ConsoleTable("C1", "C2", "C3", "C4", "C5", "C6", "C7", "C8", "C9", "C10", "C11", "C12",
                    "C13", "C14", "C15", "C16", "C17", "C18", "C19", "C20");

                foreach (var row in boardCtrl.Board)
                {
                    ct2.AddRow(row.Select(s => s.AnimalOnField).ToArray());
                }

                ct2.Write(Format.Alternative);


            }

        }



    }
}
