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

        private List<Character> _characters;
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
            ManageAppLoop();
        }

        private void ManageAppLoop()
        {
            DisplayWelcomeScreen();

            UserAction userActionChoice;

            do
            {
                userActionChoice = GetUserAction();
                ProcessUserAction(userActionChoice);
            } while (_runApp);

            DisplayClosingScreen();
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

        private void DisplayListOfCharacters()
        {
            DisplayReset();

            try
            {
                CharactersBLL cBll = new CharactersBLL();
                _characters = cBll.GetCharacters() as List<Character>;

                if (_characters != null)
                {
                    foreach (Character character in _characters)
                    {
                        DisplayCharacter(character);
                    }
                }
                else
                {
                    DisplayMessage("It appears there is no data in the file.");
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Unable to locate the data file.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            DisplayContinuePrompt();
        }

        private void DisplayCharacter(Character character)
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

        private UserAction GetUserAction()
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
                Console.WriteLine(leftTab + $"{ConsoleUtil.ToLabelFormat(menuItem.Key.ToString())}) {menuItem.Value}");
            }

            DisplayMessage("");
            DisplayPromptMessage("Enter the number/letter for the menu choice: ");
            ConsoleKeyInfo userResponse = Console.ReadKey(true);

            if (menuItems.ContainsKey(userResponse.KeyChar))
            {
                menuItems.TryGetValue(userResponse.KeyChar, out userActionChoice);
                ProcessUserAction(userActionChoice);
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
        Console.WriteLine(ConsoleUtil.Center("The Ski Run Rater", WINDOW_WIDTH));
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
        ConsoleKeyInfo response = Console.ReadKey();

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
}
}
