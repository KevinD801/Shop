using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Shop
{
    class Player
    {
        private int _gold;
        private Item[] _inventory;

        public Player()
        {
            _gold = 100;

            // Creates a new item array with three items with default values
            _inventory = new Item[3];
        }

        public bool Buy(Item item, int inventoryIndex)
        {
            // Check to see if the player can afford the item
            if (_gold >= item.cost)
            {
                // Pay for item.
                _gold -= item.cost;

                // Place item in inventory array.
                _inventory[inventoryIndex] = item;
                return true;
            }

            return false;
        }

        public string GetItemNames()
        {

        }

        public int GetGold()
        {
            return _gold;
        }

        public Item[] GetInventory()
        {
            return _inventory;
        }

        public void Save(StreamWriter writer)
        {
            writer.WriteLine();
            base.Save(writer);
            writer.WriteLine(_currentItemIndex);
        }

        public bool Load(StreamReader reader)
        {
            // If the base loading function fails..
            if (!base.Load(reader))
            {
                // ...return false
                return false;
            }

            // If the current line can't be converted into an int...
            if (!int.TryParse(reader.ReadLine(), out _currentItemIndex))
            {
                // ...return false
                return false;
            }

            // Return whether or not the item was equipped successfully
            return TryEquipItem(_currentItemIndex);
        }
    }
}
