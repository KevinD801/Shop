using System;
using System.Collections.Generic;
using System.Text;

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

        private Item _sword;
        private Item _shield;
        private Item _healthPotion;

        private Item[] _shopInventory;
        

        //Run the game
        public void Run()
        {
            Start();

            while (_gameOver == false)
            {
                Update();
            }

            End();
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

        public void PrintInventory(Item[] inventory)
        {
            for (int i = 0; i < inventory.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + inventory[i].name + inventory[i].cost);
            }
        }

        private void OpenShopMenu()
        {
            //Print a welcome message and all the choices to the screen
            Console.WriteLine("Welcome! Please selct an item.");
            PrintInventory(_shopInventory);

            //Get player input
            char input = Console.ReadKey().KeyChar;

            //Set itemIndex to be the indec the player selected
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

            //If the player can't afford the item print a message to let them know
            if (_player.GetGold() < _shopInventory[itemIndex].cost)
            {
                Console.WriteLine("You cant afford this.");
                return;
            }

            //Ask the player to replace a slot in their own inventory
            Console.WriteLine("Choose a slot to replace.");
            PrintInventory(_player.GetInventory());
            //Get player input
            input = Console.ReadKey().KeyChar;

            //Set the value of the playerIndex based on the player's choice
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

            //Sell item to player and replace the weapon at the index with the newly purchased weapon
            _shop.Sell(_player, itemIndex, playerIndex);
        }

        //Performed once when the game begins
        public void Start()
        {
            _gameOver = false;
            _player = new Player();
            InitializeItems();
            _shopInventory = new Item[] { _sword, _shield, _healthPotion};
            _shop = new Shop(_shopInventory);
        }

        //Repeated until the game ends
        public void Update()
        {
            OpenShopMenu();
        }

        //Performed once when the game ends
        public void End()
        {

        }
    }
}
