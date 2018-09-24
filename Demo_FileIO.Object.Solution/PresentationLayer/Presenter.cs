using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_FileIO
{
    public class Presenter
    {
        //
        // window size
        //
        private const int WINDOW_WIDTH = ViewSettings.WINDOW_WIDTH;
        private const int WINDOW_HEIGHT = ViewSettings.WINDOW_HEIGHT;

        //
        // horizontal and vertical margins in console window for display
        //
        private const int DISPLAY_HORIZONTAL_MARGIN = ViewSettings.DISPLAY_HORIZONTAL_MARGIN;
        private const int DISPALY_VERITCAL_MARGIN = ViewSettings.DISPALY_VERITCAL_MARGIN;

        //private List<Character> _characters;
        bool _runApp = true;

        //
        // dictionary to dynamically manage menu choices
        //
        private Dictionary<char, UserAction> menuItems = new Dictionary<char, UserAction>()
        {
            { '1', UserAction.ListAllCharacters },
            { '2', UserAction.DisplayCharacterDetail },
            { '3', UserAction.AddCharacter },
            { '4', UserAction.UpdateCharacter },
            { '5', UserAction.DeleteCharacter },
            { '6', UserAction.QueryByAge },
            { '7', UserAction.Quit }
        };

        public Presenter()
        {
            InitializeConsoleWindow();
            ManageAppLoop();
        }

        private void InitializeConsoleWindow()
        {
            Console.WindowWidth = WINDOW_WIDTH;
            Console.WindowHeight = WINDOW_HEIGHT;
        }

        private void ManageAppLoop()
        {
            DisplayWelcomeScreen();

            UserAction userActionChoice;

            do
            {
                userActionChoice = DisplayGetUserAction();
                ProcessUserAction(userActionChoice);
            } while (_runApp);

            DisplayClosingScreen();
        }

        private UserAction DisplayGetUserAction()
        {
            UserAction userActionChoice = UserAction.None;

            //
            // set a string variable with a length equal to the horizontal margin and filled with spaces
            //
            string leftTab = ConsoleUtil.FillStringWithSpaces(DISPLAY_HORIZONTAL_MARGIN);

            //
            // set up display area
            //
            DisplayReset();

            //
            // display the menu
            //
            DisplayMessage("");
            Console.WriteLine(ConsoleUtil.Center("Action Menu", WINDOW_WIDTH));
            DisplayMessage("");

            foreach (var menuItem in menuItems)
            {
                Console.WriteLine(leftTab + $"{menuItem.Key}) {ConsoleUtil.ToLabelFormat(menuItem.Value.ToString())}");
            }

            DisplayMessage("");
            DisplayPromptMessage("Enter the number/letter for the menu choice: ");
            ConsoleKeyInfo userResponse = Console.ReadKey(true);

            if (menuItems.ContainsKey(userResponse.KeyChar))
            {
                menuItems.TryGetValue(userResponse.KeyChar, out userActionChoice);
            }
            else
            {
                DisplayMessage("");
                DisplayMessage("");
                DisplayMessage("It appears you have selected an incorrect choice.");
                DisplayMessage("");
                DisplayMessage("Press any key to try again or the ESC key to exit.");

                userResponse = Console.ReadKey(true);
                if (userResponse.Key == ConsoleKey.Escape)
                {
                    userActionChoice = UserAction.Quit;
                }
            }
            return userActionChoice;
        }

        private void ProcessUserAction(UserAction userActionChoice)
        {
            switch (userActionChoice)
            {
                case UserAction.None:
                    break;
                case UserAction.ListAllCharacters:
                    DisplayListOfCharacters();
                    break;
                case UserAction.DisplayCharacterDetail:
                    break;
                case UserAction.AddCharacter:
                    DisplayAddCharacter();
                    break;
                case UserAction.UpdateCharacter:
                    break;
                case UserAction.DeleteCharacter:
                    break;
                case UserAction.QueryByAge:
                    break;
                case UserAction.Quit:
                    _runApp = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// display a list of character ids and full name
        /// </summary>
        private void DisplayListOfCharacters()
        {
            bool success;
            string message;

            CharactersBLL charactersBLL = new CharactersBLL();
            List<Character> characters = charactersBLL.GetCharacters(out success, out message) as List<Character>;

            DisplayReset();

            if (success)
            {
                DisplayMessage("");
                Console.WriteLine(ConsoleUtil.Center("Display All Characters", WINDOW_WIDTH));
                DisplayMessage("");

                DisplayCharacterTable(characters);
            }
            else
            {
                DisplayMessage(message);
            }

            DisplayContinuePrompt();
        }

        private void DisplayAddCharacter()
        {
            bool success;
            string message;

            DisplayReset();

            Character character = new Character();

            DisplayReset();

            DisplayMessage("");
            Console.WriteLine(ConsoleUtil.Center("Add Character", WINDOW_WIDTH));
            DisplayMessage("");

            DisplayPromptMessage("Last Name:");
            character.LastName = Console.ReadLine();
            DisplayPromptMessage("First Name:");
            character.FirstName = Console.ReadLine();
            DisplayPromptMessage("Address:");
            character.Address = Console.ReadLine();
            DisplayPromptMessage("City:");
            character.City = Console.ReadLine();
            DisplayPromptMessage("State:");
            character.State = Console.ReadLine();
            DisplayPromptMessage("Zip:");
            character.Zip = Console.ReadLine();

            int age;
            GetInteger("Age:", 1, 100, out age);
            character.Age = age;

            Character.GenderType gender;
            DisplayPromptMessage("Gender [MALE, FEMALE]:"); ;
            while (!Enum.TryParse<Character.GenderType>(Console.ReadLine().ToUpper(), out gender))
            {
                DisplayMessage("Please use one of the following values [MALE, FEMALE]");
                DisplayPromptMessage("Gender [MALE, FEMALE]:"); ;
            }
            character.Gender = gender;

            CharactersBLL charactersBLL = new CharactersBLL();
            charactersBLL.AddCharacter(character, out success, out message);

            if (!success)
            {
                DisplayMessage(message);
            }

            DisplayContinuePrompt();
        }

        private void DisplayCharacterTable(List<Character> characters)
        {
            StringBuilder columnHeader = new StringBuilder();

            columnHeader.Append("Id".PadRight(8));
            columnHeader.Append("Full Name".PadRight(25));

            DisplayMessage(columnHeader.ToString());

            foreach (Character character in characters)
            {
                StringBuilder characterInfo = new StringBuilder();

                characterInfo.Append(character.Id.ToString().PadRight(8));
                characterInfo.Append(character.FullName().PadRight(25));

                DisplayMessage(characterInfo.ToString());
            }
        }

        private void DisplayCharacterDetail(Character character)
        {
            Console.WriteLine();
            Console.WriteLine($"Id: {character.Id}");
            Console.WriteLine($"Last Name: {character.LastName}");
            Console.WriteLine($"First Name: {character.FirstName}");
            Console.WriteLine($"Address: {character.Address}");
            Console.WriteLine($"City: {character.City}");
            Console.WriteLine($"State: {character.State}");
            Console.WriteLine($"Zip: {character.Zip}");
            Console.WriteLine($"Age: {character.Age}");
            Console.WriteLine($"Gender: {character.Gender}");
            Console.WriteLine();
        }





        /// <summary>
        /// reset display to default size and colors including the header
        /// </summary>
        private static void DisplayReset()
        {
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);

            Console.Clear();
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.White;

            Console.WriteLine(ConsoleUtil.FillStringWithSpaces(WINDOW_WIDTH));
            Console.WriteLine(ConsoleUtil.Center("The Flintstone Characters", WINDOW_WIDTH));
            Console.WriteLine(ConsoleUtil.FillStringWithSpaces(WINDOW_WIDTH));

            Console.ResetColor();
            Console.WriteLine();
        }

        /// <summary>
        /// display the Continue prompt
        /// </summary>
        private void DisplayContinuePrompt()
        {
            Console.CursorVisible = false;

            Console.WriteLine();

            Console.WriteLine(ConsoleUtil.Center("Press any key to continue.", WINDOW_WIDTH));
            Console.ReadKey();

            Console.WriteLine();

            Console.CursorVisible = true;
        }


        /// <summary>
        /// display the Exit prompt
        /// </summary>
        private void DisplayClosingScreen()
        {
            DisplayReset();

            Console.CursorVisible = false;

            Console.WriteLine();
            DisplayMessage("Thank you for using our application. Press any key to Exit.");

            Console.ReadKey();

            System.Environment.Exit(1);
        }

        /// <summary>
        /// display the welcome screen
        /// </summary>
        private void DisplayWelcomeScreen()
        {
            Console.Clear();
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.White;

            Console.WriteLine(ConsoleUtil.FillStringWithSpaces(WINDOW_WIDTH));
            Console.WriteLine(ConsoleUtil.Center("Welcome to", WINDOW_WIDTH));
            Console.WriteLine(ConsoleUtil.Center("The Flintstones Database", WINDOW_WIDTH));
            Console.WriteLine(ConsoleUtil.FillStringWithSpaces(WINDOW_WIDTH));

            Console.ResetColor();
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display a message in the message area
        /// </summary>
        /// <param name="message">string to display</param>
        private static void DisplayMessage(string message)
        {
            //
            // calculate the message area location on the console window
            //
            const int MESSAGE_BOX_TEXT_LENGTH = WINDOW_WIDTH - (2 * DISPLAY_HORIZONTAL_MARGIN);
            const int MESSAGE_BOX_HORIZONTAL_MARGIN = DISPLAY_HORIZONTAL_MARGIN;

            // message is not an empty line, display text
            if (message != "")
            {
                //
                // create a list of strings to hold the wrapped text message
                //
                List<string> messageLines;

                //
                // call utility method to wrap text and loop through list of strings to display
                //
                messageLines = ConsoleUtil.Wrap(message, MESSAGE_BOX_TEXT_LENGTH, MESSAGE_BOX_HORIZONTAL_MARGIN);
                foreach (var messageLine in messageLines)
                {
                    Console.WriteLine(messageLine);
                }
            }
            // display an empty line
            else
            {
                Console.WriteLine();
            }
        }

        /// <summary>
        /// display a message in the message area without a new line for the prompt
        /// </summary>
        /// <param name="message">string to display</param>
        private static void DisplayPromptMessage(string message)
        {
            //
            // calculate the message area location on the console window
            //
            const int MESSAGE_BOX_TEXT_LENGTH = WINDOW_WIDTH - (2 * DISPLAY_HORIZONTAL_MARGIN);
            const int MESSAGE_BOX_HORIZONTAL_MARGIN = DISPLAY_HORIZONTAL_MARGIN;

            //
            // create a list of strings to hold the wrapped text message
            //
            List<string> messageLines;

            //
            // call utility method to wrap text and loop through list of strings to display
            //
            messageLines = ConsoleUtil.Wrap(message, MESSAGE_BOX_TEXT_LENGTH, MESSAGE_BOX_HORIZONTAL_MARGIN);

            for (int lineNumber = 0; lineNumber < messageLines.Count() - 1; lineNumber++)
            {
                Console.WriteLine(messageLines[lineNumber]);
            }

            Console.Write(messageLines[messageLines.Count() - 1]);
        }

        /// <summary>
        /// get a valid integer from the player - note: if max and min values are both 0, range validation is disabled
        /// </summary>
        /// <param name="prompt">prompt message in console</param>
        /// <param name="minimumValue">min. value</param>
        /// <param name="maximumValue">max. value</param>
        /// <param name="integerChoice">out value</param>
        /// <returns></returns>
        private bool GetInteger(string prompt, int minimumValue, int maximumValue, out int integerChoice)
        {
            bool validResponse = false;
            integerChoice = 0;

            //
            // validate on range if either minimumValue and maximumValue are not 0
            //
            bool validateRange = (minimumValue != 0 || maximumValue != 0);

            DisplayPromptMessage(prompt);
            while (!validResponse)
            {
                if (int.TryParse(Console.ReadLine(), out integerChoice))
                {
                    if (validateRange)
                    {
                        if (integerChoice >= minimumValue && integerChoice <= maximumValue)
                        {
                            validResponse = true;
                        }
                        else
                        {
                            DisplayMessage($"You must enter an integer value between {minimumValue} and {maximumValue}. Please try again.");
                            DisplayPromptMessage(prompt);
                        }
                    }
                    else
                    {
                        validResponse = true;
                    }
                }
                else
                {
                    DisplayMessage($"You must enter an integer value. Please try again.");
                    DisplayPromptMessage(prompt);
                }
            }

            Console.CursorVisible = false;

            return true;
        }
    }
}
