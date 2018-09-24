using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_FileIO
{
    class Program
    {
        static void Main(string[] args)
        {
            Presenter presenter = new Presenter();



            //IDataService dataService = new CsvDataService();
            //List<Character> characters = new List<Character>();

            //try
            //{
            //    CharactersBLL cBll = new CharactersBLL();
            //    characters = cBll.GetCharacters() as List<Character>;
            //}
            //catch (FileNotFoundException)
            //{
            //    Console.WriteLine("Unable to locate the data file.");
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}


            //Character newCharacter = new Character()
            //{
            //    LastName = "Flintstone",
            //    FirstName = "Dino",
            //    Address = "301 Cobblestone Way",
            //    City = "Bedrock",
            //    State = "MI",
            //    Zip = "70777",
            //    Age = 7,
            //    Gender = Character.GenderType.FEMALE
            //};

            //try
            //{
            //    CharactersBLL cBll = new CharactersBLL();
            //    cBll.AddCharacter(newCharacter);
            //}
            //catch (FileNotFoundException)
            //{
            //    Console.WriteLine("The file could not be found.");
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //DisplayCharacters(characters);

            //Console.WriteLine();
            //Console.WriteLine("\nPress any key to exit.");
            //Console.ReadKey();
        }

        //static void DisplayCharacters(List<Character> characters)
        //{
        //    foreach (Character character in characters)
        //    {
        //        Console.WriteLine();
        //        Console.WriteLine($"Id: {character.Id}");
        //        Console.WriteLine($"Last Name: {character.LastName}");
        //        Console.WriteLine($"First Name: {character.FirstName}");
        //        Console.WriteLine($"Address: {character.Address}");
        //        Console.WriteLine($"City: {character.City}");
        //        Console.WriteLine($"State: {character.State}");
        //        Console.WriteLine($"Zip: {character.Zip}");
        //        Console.WriteLine($"Age: {character.Age}");
        //        Console.WriteLine($"Gender: {character.Gender}");
        //        Console.WriteLine();
        //    }
        //}
    }
}
