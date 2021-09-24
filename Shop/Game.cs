using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Shop
{
    /// <summary>
    /// Represents items
    /// </summary>
    struct Item
    {
        public string Name;
        public int Cost;
    }

    class Game
    {
        // Changes player money values
        private Player _player = new Player(350);

        private Shop _shop;
        private bool _gameOver;
        private int _currentScene;


        public void Run()
        {
            Start();

            while (!_gameOver)
            {
                Update();
            }

            End();
        }

        private void Start()
        {
            _gameOver = false;
            _currentScene = 0;

            InitializeItems();
        }

        private void Update()
        {
            DisplayCurrentScene();
        }

        /// <summary>
        /// Display end screen.
        /// </summary>
        private void End()
        {
            Console.WriteLine("Thank for shopping at TriShop, player!");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Function used to initialize game items
        /// </summary>
        private void InitializeItems()
        {
            // Initializing shop items
            Item milk = new Item { Name = "Milk", Cost = 10 };
            Item eggs = new Item { Name = "Eggs", Cost = 2 };
            Item videoGames = new Item { Name = "Video Games", Cost = 64 };

            // Array of shop list
            _shop = new Shop(milk, eggs, videoGames);
        }

        private int GetInput(string description, params string[] options)
        {
            string input = "";
            int inputReceived = -1;

            Console.WriteLine(description);

            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + options[i]);
            }

            Console.Write("> ");

            // Get input from player
            input = Console.ReadLine();

            // If the player typed an int...
            if (int.TryParse(input, out inputReceived))
            {
                // ...decrement the input and check if it's within the bounds of the array
                inputReceived--;

                if (inputReceived < 0 || inputReceived >= options.Length)
                {
                    // Set the input received to be the default value
                    inputReceived = -1;

                    // Display an error message
                    Console.WriteLine("Invalid input.");
                    Console.ReadKey(true);
                }
            }
            else
            {
                inputReceived = -1;
                Console.WriteLine("Invalid input.");
                Console.ReadKey(true);
            }

            Console.Clear();

            return inputReceived;
        }

        private void Save()
        {
            // Create a new stream writer
            StreamWriter writer = new StreamWriter("Inventory.txt");

            _player.Save(writer);
            _shop.Save(writer);

            // Close the writer when done saving
            writer.Close();
        }

        private bool Load()
        {
            // Create a new reader to read from the text file
            StreamReader reader = new StreamReader("Inventory.txt");
            
            // If the file doesn't exist...
            if (!File.Exists("Inventory.txt"))
            {
                // ... return false
                return false;
            }

            if (!_player.Load(reader))
            {
                return false;
            }
            if (!_shop.Load(reader))
            {
                return false;
            }

            // Close the reader once loading is finished
            reader.Close();

            return true;
        }

        private void DisplayCurrentScene()
        {
            switch (_currentScene)
            {
                case 0:
                    DisplayOpeningMenu();
                    break;
                case 1:
                    DisplayShopMenu();
                    break;
            }
        }
        /// <summary>
        /// Introduction and greet player.
        /// </summary>
        private void DisplayOpeningMenu()
        {
            int input = GetInput("Welcome to TriShop! Would you like to:",
                "Start new game", "Load existing game");

            // Go to DisplayShopMenu
            if (input == 0)
            {
                _currentScene = 1;
            }

            // Load game
            else if (input == 1)
            {
                if (Load())
                {
                    Console.WriteLine("Load Successful");
                    Console.ReadKey(true);
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Load Failed");
                    Console.ReadKey(true);
                    Console.Clear();
                }
            }
        }

        private string[] GetShopMenuOptions()
        {
            string[] shopItems = _shop.GetItemNames();
            string[] menuOptions = new string[shopItems.Length + 2];

            for (int i = 0; i < shopItems.Length; i++)
            {
                menuOptions[i] = shopItems[i];
            }

            // Save Game options
            menuOptions[shopItems.Length] = "Save Game";

            // Quit Game options
            menuOptions[shopItems.Length + 1] = "Quit Game";

            return menuOptions;
        }

        private void DisplayShopMenu()
        {
            string[] playerItemNames = _player.GetItemNames();

            Console.WriteLine($"Your Gold: {_player.Gold}\n");
            Console.WriteLine("Your Inventory:");

            // Take itemName inside playerItemNames array print to console
            foreach (string itemName in playerItemNames)
            {
                Console.WriteLine(itemName);
            }
            Console.WriteLine();

            int input = GetInput("What would you like to purchase?", GetShopMenuOptions());

            if (input >= 0 && input < GetShopMenuOptions().Length - 2)
            {
                if (_shop.Sell(_player, input))
                {
                    Console.Clear();
                    Console.WriteLine($"You purchased the {_shop.GetItemNames()[input]}!");
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("You don't have enough gold for that.");
                    Console.ReadKey(true);
                    Console.Clear();
                }
            }

            // Save game
            if (input == GetShopMenuOptions().Length - 2)
            {
                Console.Clear();

                // Called the Save function.
                Save();

                // Feedback the game saved
                Console.WriteLine("Game saved successfully!");
                Console.ReadKey(true);
                Console.Clear();
            }

            // Quit game
            if (input == GetShopMenuOptions().Length - 1)
            {
                _gameOver = true;
            }
        }
    }
}