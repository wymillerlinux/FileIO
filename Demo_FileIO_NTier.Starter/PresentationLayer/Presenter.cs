using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo_FileIO_NTier.BusinessLogicLayer;
using Demo_FileIO_NTier.Models;

namespace Demo_FileIO_NTier.PresentationLayer
{
    public class Presenter
    {
        private static CharacterBLL _characterBLL;

        /// <summary>
        /// Constructor for the presenter class
        /// </summary>
        /// <param name="characterBll"></param>
        public Presenter(CharacterBLL characterBll)
        {
            _characterBLL = characterBll;
            ManageAppLoop();
        }

        /// <summary>
        /// Manages the application loop
        /// </summary>
        private void ManageAppLoop()
        {
            DisplayWelcomeScreen();
            DisplayListOfCharacters();
            DisplayClosingScreen();
        }

        /// <summary>
        /// Display the header
        /// </summary>
        /// <param name="message"></param>
        static void DisplayHeader(string message)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"\t\t{message}");
            Console.WriteLine();
        }

        /// <summary>
        /// Display the continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Display the welcome screen
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.WriteLine();
            Console.WriteLine("\t\tWelcome to the FileIO project!");
            Console.WriteLine("\t\tNMC CIT 255");
            
            DisplayContinuePrompt();
        }

        /// <summary>
        /// Display the closing screen
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for trying the FileIO project!");
            Console.WriteLine();
            Console.WriteLine("\t\tGoodbye!");
            
            DisplayContinuePrompt();
        }

        /// <summary>
        /// Display the character table
        /// </summary>
        /// <param name="character"></param>
        private void DisplayCharacterTable(List<Character> character)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Id".PadRight(8));
            sb.Append("Full Name".PadRight(25));

            Console.WriteLine(sb.ToString());

            character = character.OrderBy(c => c.Id).ToList();

            foreach (var ch in character)
            {
                StringBuilder info = new StringBuilder();

                info.AppendLine(ch.Id.ToString().PadRight(8));
                info.AppendLine(ch.FullName().PadRight(25));

                Console.WriteLine(info.ToString());
            }
        }

        /// <summary>
        /// Display list of characters
        /// Implements DisplayCharacterTable method
        /// </summary>
        private void DisplayListOfCharacters()
        {
            bool success;
            string message;

            List<Character> character = _characterBLL.GetCharacters(out success, out message) as List<Character>;
            character = character.OrderBy(c => c.Id).ToList();
            
            DisplayHeader("List of characters");

            if (success)
            {
                DisplayCharacterTable(character);
            }
            else
            {
                Console.WriteLine(message);
            }
            
            DisplayContinuePrompt();
        }

    }
}