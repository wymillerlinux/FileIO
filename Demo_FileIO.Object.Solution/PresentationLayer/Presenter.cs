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
            DisplayPromptMessage("Enter the number for the menu choice: ");
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
                    DisplayDetailOfCharacter();
                    break;
                case UserAction.AddCharacter:
                    DisplayAddCharacter();
                    break;
                case UserAction.UpdateCharacter:
                    DisplayUpdateCharacter();
                    break;
                case UserAction.DeleteCharacter:
                    DisplayDeleteCharacter();
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
            characters = characters.OrderBy(c => c.Id).ToList();

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

        /// <summary>
        /// get a character id and display the details
        /// </summary>
        private void DisplayDetailOfCharacter()
        {
            bool success;
            string message;
            int characterId;
            Character character;

            CharactersBLL charactersBLL = new CharactersBLL();
            List<Character> characters = charactersBLL.GetCharacters(out success, out message) as List<Character>;

            DisplayReset();

            if (success)
            {
                DisplayMessage("");
                Console.WriteLine(ConsoleUtil.Center("Detail of Character", WINDOW_WIDTH));
                DisplayMessage("");

                characterId = DisplayChooseCharacter("Detail of", characters);

                character = charactersBLL.GetCharacterById(characterId, out success, out message);

                if (success)
                {
                    DisplayReset();

                    DisplayMessage("");
                    Console.WriteLine(ConsoleUtil.Center("Delete Character", WINDOW_WIDTH));
                    DisplayMessage("");

                    DisplayCharacter(character);
                }
            }
            else
            {
                DisplayMessage(message);
            }

            DisplayContinuePrompt();
        }

        /// <summary>
        /// add a new character
        /// </summary>
        private void DisplayAddCharacter()
        {
            bool success;
            string message;

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
            character.Age = GetInteger("Age:", 1, 100, out age) ? age : 0;

            Character.GenderType gender;
            DisplayPromptMessage("Gender [MALE, FEMALE, NOTSPECIFIED]:"); ;
            while (!Enum.TryParse<Character.GenderType>(Console.ReadLine().ToUpper(), out gender))
            {
                DisplayMessage("Please use one of the following values [MALE, FEMALE, NOTSPECIFIED]");
                DisplayPromptMessage("Gender [MALE, FEMALE, NOTSPECIFIED]:"); ;
            }
            character.Gender = gender;

            CharactersBLL charactersBLL = new CharactersBLL();
            charactersBLL.AddCharacter(character, out success, out message);

            DisplayReset();

            DisplayMessage("");
            Console.WriteLine(ConsoleUtil.Center("Add Character", WINDOW_WIDTH));
            DisplayMessage("");
            DisplayMessage(message);

            DisplayContinuePrompt();
        }

        /// <summary>
        /// delete a character
        /// </summary>
        private void DisplayDeleteCharacter()
        {
            bool success;
            string message;
            int characterId;

            CharactersBLL charactersBLL = new CharactersBLL();
            List<Character> characters = charactersBLL.GetCharacters(out success, out message) as List<Character>;

            DisplayReset();

            if (success)
            {
                DisplayMessage("");
                Console.WriteLine(ConsoleUtil.Center("Delete Character", WINDOW_WIDTH));
                DisplayMessage("");

                characterId = DisplayChooseCharacter("Delete", characters);

                charactersBLL.DeleteCharacter(characterId, out success, out message);

                if (!success)
                {
                    DisplayReset();

                    DisplayMessage("");
                    Console.WriteLine(ConsoleUtil.Center("Delete Character", WINDOW_WIDTH));

                }
            }

            DisplayReset();

            DisplayMessage("");
            Console.WriteLine(ConsoleUtil.Center("Delete Character", WINDOW_WIDTH));
            DisplayMessage("");
            DisplayMessage(message);

            DisplayContinuePrompt();
        }

        /// <summary>
        /// update a character
        /// </summary>
        private void DisplayUpdateCharacter()
        {
            bool success;
            string message;
            string userResponse;
            int characterId;
            Character character;

            CharactersBLL charactersBLL = new CharactersBLL();
            List<Character> characters = charactersBLL.GetCharacters(out success, out message) as List<Character>;

            DisplayReset();

            if (success)
            {
                DisplayMessage("");
                Console.WriteLine(ConsoleUtil.Center("Update Character", WINDOW_WIDTH));
                DisplayMessage("");

                characterId = DisplayChooseCharacter("Update", characters);

                character = charactersBLL.GetCharacterById(characterId, out success, out message);

                if (success)
                {
                    DisplayReset();

                    DisplayMessage("");
                    Console.WriteLine(ConsoleUtil.Center("Update Character", WINDOW_WIDTH));
                    DisplayMessage("");

                    DisplayMessage("Type a new value or press the Enter key to keep the current value.");
                    DisplayMessage("");

                    DisplayPromptMessage($"Last Name: {character.LastName} > ");
                    character.LastName = GetUpdateCharacterProperty(character.LastName);
                    DisplayPromptMessage($"First Name: {character.FirstName} > ");
                    character.FirstName = GetUpdateCharacterProperty(character.FirstName);
                    DisplayPromptMessage($"Address: {character.Address} >");
                    character.Address = GetUpdateCharacterProperty(character.Address);
                    DisplayPromptMessage($"City: {character.City} >");
                    character.City = GetUpdateCharacterProperty(character.City);
                    DisplayPromptMessage($"State: {character.State} >");
                    character.State = GetUpdateCharacterProperty(character.State);
                    DisplayPromptMessage($"Zip: {character.Zip} >");
                    character.Zip = GetUpdateCharacterProperty(character.Zip);

                    int age;
                    DisplayPromptMessage($"Age: {character.Age} >");
                    character.Age = GetInteger($"Age:", 1, 100, out age) ? age : character.Age;

                    Character.GenderType gender;
                    DisplayPromptMessage($"Gender [MALE, FEMALE, NOTSPECIFIED]: {character.Gender} >");
                    userResponse = Console.ReadLine().ToUpper();
                    if (!String.IsNullOrEmpty(userResponse))
                    {
                        while (!Enum.TryParse<Character.GenderType>(userResponse, out gender))
                        {
                            DisplayMessage("Please use one of the following values [MALE, FEMALE, NOTSPECIFIED]");
                            DisplayPromptMessage($"Gender [MALE, FEMALE, NOTSPECIFIED]:");
                            userResponse = Console.ReadLine().ToUpper();
                        }

                        character.Gender = gender;
                    }

                    charactersBLL.UpdateCharacter(character, out success, out message);
                }
            }

            DisplayReset();

            DisplayMessage("");
            Console.WriteLine(ConsoleUtil.Center("Update Character", WINDOW_WIDTH));
            DisplayMessage("");
            DisplayMessage(message);

            DisplayContinuePrompt();
        }

        /// <summary>
        /// capture the user response to update a character property
        /// return the user response if non-empty else 
        /// return the current property value
        /// </summary>
        /// <param name="currentPropertyValue">current property value</param>
        /// <returns>new property or current value</returns>
        private static string GetUpdateCharacterProperty(string currentPropertyValue)
        {
            string userResponse = Console.ReadLine();
            return String.IsNullOrEmpty(userResponse) ? currentPropertyValue : userResponse;
        }

        /// <summary>
        /// display a list of character Ids and full names
        /// allow the user to choose an Id
        /// </summary>
        /// <param name="action">Detail, Delete, or Update</param>
        /// <param name="characters">list of Characters</param>
        /// <returns></returns>
        private int DisplayChooseCharacter(string action, List<Character> characters)
        {
            int characterId;

            DisplayMessage("");
            Console.WriteLine(ConsoleUtil.Center($"Choose Character Id to {action}", WINDOW_WIDTH));
            DisplayMessage("");

            DisplayCharacterTable(characters);

            DisplayMessage("");

            GetInteger("Enter Character Id:", 1, 100, out characterId);

            return characterId;
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

            DisplayMessage(columnHeader.ToString());

            characters = characters.OrderBy(c => c.Id).ToList();

            foreach (Character character in characters)
            {
                StringBuilder characterInfo = new StringBuilder();

                characterInfo.Append(character.Id.ToString().PadRight(8));
                characterInfo.Append(character.FullName().PadRight(25));

                DisplayMessage(characterInfo.ToString());
            }
        }

        /// <summary>
        /// display all character properties
        /// </summary>
        /// <param name="character">character</param>
        private void DisplayCharacter(Character character)
        {
            DisplayMessage("");
            DisplayMessage($"Id: {character.Id}");
            DisplayMessage($"Last Name: {character.LastName}");
            DisplayMessage($"First Name: {character.FirstName}");
            DisplayMessage($"Address: {character.Address}");
            DisplayMessage($"City: {character.City}");
            DisplayMessage($"State: {character.State}");
            DisplayMessage($"Zip: {character.Zip}");
            DisplayMessage($"Age: {character.Age}");
            DisplayMessage($"Gender: {character.Gender}");
            DisplayMessage("");
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
            string userResponse;
            integerChoice = 0;

            //
            // validate on range if either minimumValue and maximumValue are not 0
            //
            bool validateRange = (minimumValue != 0 || maximumValue != 0);

            DisplayPromptMessage(prompt);
            while (!validResponse)
            {
                if (!String.IsNullOrEmpty(userResponse = Console.ReadLine()))
                {
                    if (int.TryParse(userResponse, out integerChoice))
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
                else
                {
                    return false;
                }

            }

            return true;
        }
    }
}
