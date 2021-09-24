using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Shop
{
    class Shop
    {
        private int _gold;
        private Item[] _inventory;

        public Shop(params Item[] inventory)
        {
            _inventory = inventory;
        }

        public bool Sell(Player player, int i)
        {
            if (player.Gold >= _inventory[i].Cost)
            {
                _gold += _inventory[i].Cost;
                player.Buy(_inventory[i]);
                return true;
            }
            else
            {
                return false;
            }
        }

        public string[] GetItemNames()
        {
            string[] itemNames = new string[_inventory.Length];

            for (int i = 0; i < _inventory.Length; i++)
            {
                itemNames[i] = $"{_inventory[i].Name} - {_inventory[i].Cost}g";
            }

            return itemNames;
        }

        public void Save(StreamWriter writer)
        {
            // Save Player Gold
            writer.WriteLine(_gold);

            // Save Player Inventory Amount 
            writer.WriteLine(_inventory.Length);

            for (int i = 0; i < _inventory.Length; i++)
            {
                writer.WriteLine(_inventory[i].Name);
                writer.WriteLine(_inventory[i].Cost);
            }
        }


        public bool Load(StreamReader reader)
        {
            if (int.TryParse(reader.ReadLine(), out _gold))
            {
                return false;
            }

            string inventoryLength = Console.ReadLine();

            _inventory = new Item[int.Parse(inventoryLength)];

            for (int i = 0; i < _inventory.Length; i++)
            {
                _inventory[i].Name = reader.ReadLine();
                _inventory[i].Cost = int.Parse(reader.ReadLine());
            }

            return true;
        }
    }
}