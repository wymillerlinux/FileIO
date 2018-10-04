using System;
using System.Collections.Generic;
using System.Linq;
using Demo_FileIO_NTier.BusinessLogicLayer;
using Demo_FileIO_NTier.Models;
using System.Text;
using System.Threading.Tasks;

namespace Demo_FileIO_NTier.PresentationLayer
{
    class Presenter
    {
        static CharacterBLL _charactersBLL;

        public Presenter(CharacterBLL characterBLL)
        {
            _charactersBLL = characterBLL;
            ManageApplicationLoop();
        }

        private void ManageApplicationLoop()
        {
            DisplayWelcomeScreen();
            DisplayListOfCharacters();
            DisplayClosingScreen();
        }

        /// <summary>
        /// display a list of character ids and full name
        /// </summary>
        private void DisplayListOfCharacters()
        {
            bool success;
            string message;

            List<Character> characters = _charactersBLL.GetCharacters(out success, out message) as List<Character>;
            characters = characters.OrderBy(c => c.Id).ToList();

            DisplayHeader("List of Characters");

            if (success)
            {
                DisplayCharacterTable(characters);
            }
            else
            {
                Console.WriteLine(message);
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display a table with id and full name columns
        /// </summary>
        /// <param name="characters">characters</param>
        private void DisplayCharacterTable(List<Character> characters)
        {
            StringBuilder columnHeader = new StringBuilder();

            columnHeader.Append("Id".PadRight(8));
            columnHeader.Append("Full Name".PadRight(25));

            Console.WriteLine(columnHeader.ToString());

            characters = characters.OrderBy(c => c.Id).ToList();

            foreach (Character character in characters)
            {
                StringBuilder characterInfo = new StringBuilder();

                characterInfo.Append(character.Id.ToString().PadRight(8));
                characterInfo.Append(character.FullName().PadRight(25));

                Console.WriteLine(characterInfo.ToString());
            }
        }

        /// <summary>
        /// display page header
        /// </summary>
        /// <param name="headerText">text for header</param>
        static void DisplayHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"\t\t{headerText}");
            Console.WriteLine();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display Welcome Screen
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tWelcome");

            DisplayContinuePrompt();
        }

        /// <summary>
        /// Display Closing Screen
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tGood Bye");

            DisplayContinuePrompt();
        }

    }
}
