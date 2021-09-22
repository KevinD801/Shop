using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Shop
{
    struct Item
    {
        public int cost;
        public string name;
    }

    class Game
    {
        private Player _player;
        private Shop _shop;
        private bool _gameOver;
        private int _currentScene;

        // Items
        private Item _sword;
        private Item _shield;
        private Item _healthPotion;

        private Item[] _shopInventory;
        

        // Run the game
        public void Run()
        {
            Start();

            while (!_gameOver)
            {
                Update();
            }

            End();
        }

        // Performed once when the game begins
        public void Start()
        {
            _gameOver = false;
            _player = new Player();
            InitializeItems();
            _shopInventory = new Item[] { _sword, _shield, _healthPotion };
            _shop = new Shop(_shopInventory);
        }

        // Repeated until the game ends
        public void Update()
        {
            DisplayCurrentScene();
        }

        // Performed once when the game ends
        public void End()
        {

        }

        private void InitializeItems()
        {
            _sword.name = "Sword";
            _sword.cost = 500;

            _shield.name = "Shield";
            _shield.cost = 10;

            _healthPotion.name = "Health Potion";
            _healthPotion.cost = 15;

            
        }

        /// <summary>
        /// Gets an input from the player based on some given decision
        /// </summary>
        /// <param name="description">The context for the input</param>
        /// <param name="option1">The first option the player can choose</param>
        /// <param name="option2">The second option the player can choose</param>
        /// <returns></returns>
        int GetInput(string description, params string[] options)
        {
            string input = "";
            int inputReceived = -1;

            while (inputReceived == -1)
            {
                // Print options
                Console.WriteLine(description);
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine((i + 1) + ". " + options[i]);
                }

                Console.WriteLine("> ");

                // Get input from player
                input = Console.ReadLine();

                // If the player typed an int...
                if (int.TryParse(input, out inputReceived))
                {
                    // ...decrement the input and check if it's within the bounds of the array
                    inputReceived--;
                    if (inputReceived < 0 || inputReceived >= options.Length)
                    {
                        // Set input received to be default value
                        inputReceived = -1;

                        // Display error message
                        Console.WriteLine("Invalid Input");
                        Console.ReadKey(true);
                    }
                }

                // If the player didn't type an int
                else
                {
                    // Set input received to be the default value
                    inputReceived = -1;
                    Console.WriteLine("Invalid Input");
                    Console.ReadKey(true);
                }
                Console.Clear();
            }
            return inputReceived;
        }

        void Save()
        {
            // Create a new stream writer
            StreamWriter writer = new StreamWriter("SaveData.txt");

            // Save current scene index
            writer.WriteLine(_currentScene);

            // Save player
            _player.Save(writer);

            // Close writer when done saving
            writer.Close();
        }

        public bool Load()
        {
            bool loadSuccessful = true;

            // If the file doesn't exist...
            if (!File.Exists("SaveData.txt"))
                //...return false
                loadSuccessful = false;

            // Create a new reader to read from the text file
            StreamReader reader = new StreamReader("SaveData.txt");

            // If the first line can't be converted into an integer...
            if (!int.TryParse(reader.ReadLine(), out _currentScene))
                //...return false
                loadSuccessful = false;

            if (!_player.Load(reader))
                loadSuccessful = false;

            // Close the reader once loading is finished
            reader.Close();

            return loadSuccessful;
        }

        void DisplayCurrentScene()
        {
            switch (_currentScene)
            {
                case 0:
                    DisplayOpeningMenu();
                    break;
                case 1:
                    DisplayShopMenu();
                    GetShopMenuOption();
                    break;

            }
        }

        void DisplayOpeningMenu()
        {
            // Print a welcome message and all the choices to the screen.
            int input = GetInput("Welcome to the RPG Shop Simlator! What would you like do", 
                "Start Shopping", "Load Inventory");
            Console.WriteLine("> ");

            if (input == 0)
            {
                _currentScene++;
            }
            else if (input == 2)
            {
                if (Load())
                {
                    Console.WriteLine("Load Successful");
                    Console.ReadKey(true);
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine("Load Failed");
                Console.ReadKey(true);
                Console.Clear();
            }
        }

        string [] GetShopMenuOption()
        {

        }

        void DisplayShopMenu()
        {
            Console.WriteLine("Your Gold: ");
            Console.WriteLine("Your Inventory: ");

            Console.WriteLine("What would you like to purchase?");

            int input = GetInput("What would you like to purchase?", _sword.name, _shield.name, _healthPotion.name, "Save Game", "Quit Game");
            Console.WriteLine("> ");

            // Save Game
            if (input == 0)
            {
                Save();
                Console.WriteLine("Saved Game");
                Console.ReadKey(true);
                Console.Clear();
                return;
            }

            // Load Game
            else if (input == 1)
            {
                _gameOver = true;
            }
        }

        public void PrintInventory(Item[] inventory)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + inventory[i].name + inventory[i].cost);
            }
        }

        private void OpenShopMenu()
        {
            PrintInventory(_shopInventory);

            // Get player input
            char input = Console.ReadKey().KeyChar;

            // Set itemIndex to be the indec the player selected
            int itemIndex = -1;
            switch (input)
            {
                case '1':
                    {
                        itemIndex = 0;
                        break;
                    }
                case '2':
                    {
                        itemIndex = 1;
                        break;
                    }
                case '3':
                    {
                        itemIndex = 2;
                        break;
                    }
                default:
                    {
                        return;
                    }
            }

            // If the player can't afford the item print a message to let them know
            if (_player.GetGold() < _shopInventory[itemIndex].cost)
            {
                Console.WriteLine("You cant afford this.");
                return;
            }

            // Ask the player to replace a slot in their own inventory
            Console.WriteLine("Choose a slot to replace.");
            PrintInventory(_player.GetInventory());

            // Get player input
            input = Console.ReadKey().KeyChar;

            // Set the value of the playerIndex based on the player's choice
            int playerIndex = -1;
            switch (input)
            {
                case '1':
                    {
                        playerIndex = 0;
                        break;
                    }
                case '2':
                    {
                        playerIndex = 1;
                        break;
                    }
                case '3':
                    {
                        playerIndex = 2;
                        break;
                    }
                default:
                    {
                        return;
                    }
            }

            // Sell item to player and replace the weapon at the index with the newly purchased weapon
            _shop.Sell(_player, itemIndex, playerIndex);
        }
    }
}
