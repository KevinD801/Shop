using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Shop
{
    class Player
    {
        int _gold;
        Item[] _inventory = new Item[0];

        public int Gold
        {
            get { return _gold; }
        }

        public Player(int gold)
        {
            _gold = gold;
        }

        public void Buy(Item item)
        {
            _gold -= item.Cost;

            Item[] newInventory = new Item[_inventory.Length + 1];

            for (int i = 0; i < _inventory.Length; i++)
                newInventory[i] = _inventory[i];

            newInventory[_inventory.Length] = item;

            _inventory = newInventory;
        }

        public string[] GetItemNames()
        {
            string[] itemNames = new string[_inventory.Length];

            for (int i = 0; i < _inventory.Length; i++)
            {
                itemNames[i] = _inventory[i].Name;
            }

            return itemNames;
        }

        public void Save(StreamWriter writer)
        {
            writer.WriteLine(_gold);

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