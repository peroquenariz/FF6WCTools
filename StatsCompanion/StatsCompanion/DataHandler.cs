using System.Collections.Generic;

namespace StatsCompanion
{
    /// <summary>
    /// A class that contains methods for processing game memory.
    /// </summary>
    internal static class DataHandler
    {
        /// <summary>
        /// Method that takes 2 arrays of the same size and type and checks if they have the same data.
        /// </summary>
        /// <param name="monsterBytes"></param>
        /// <param name="monsterBytesPrevious"></param>
        /// <returns>true if they're equal, otherwise false.</returns>
        public static bool AreArraysEqual(byte[] arr1, byte[] arr2)
        {
            bool equal = true;
            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1.Length != arr2.Length || arr1[i] != arr2[i])
                {
                    equal = false;
                    return equal;
                }
            }
            return equal;
        }

        /// <summary>
        /// Method that concatenates the monster indexes.
        /// </summary>
        /// <param name="monsterBytes">The array containing the bytes of the monster indexes.</param>
        /// <returns>An array of integers with the concatenated monster indexes.</returns>
        public static int[] GetMonsterIndexes(byte[] monsterBytes)
        {
            int[] result = new int[6];
            for (int i = 0; i < 6; i++)
            {
                byte[] arr = { monsterBytes[i * 2], monsterBytes[i * 2 + 1] }; // Original data is 6 indexes, 2 bytes each.
                int monsterIndex = ConcatenateByteArray(arr);                  // Concatenate the 2 bytes.
                result[i] = monsterIndex;                                      // Save it to the new array.
            }
            return result;
        }
        
        /// <summary>
        /// Method that checks if the Tzen Thief reward was bought.
        /// </summary>
        /// <param name="currentGP">The current amount of GP.</param>
        /// <param name="currentGPprevious">The previous amount of GP.</param>
        /// <param name="dialogIndex">The dialog index.</param>
        /// <returns></returns>
        public static string CheckTzenThiefBought(byte esperCount, byte esperCountPrevious, bool tzenThiefBit)
        {
            string tzenThiefBought = "";
            if (tzenThiefBit == true && esperCount == esperCountPrevious)
            {
                tzenThiefBought = "Item";
            }
            else if (tzenThiefBit == true && esperCount > esperCountPrevious)
            {
                tzenThiefBought = "Esper";
            }
            return tzenThiefBought;
        }

        /// <summary>
        /// Method that checks if the player peeked what reward World of Balance Tzen thief has.
        /// </summary>
        /// <param name="dialogWaitingForInput">If the dialog is waiting for an input.</param>
        /// <param name="dialogPointer">Pointer, "2": item, "4": esper.</param>
        /// <param name="dialogChoiceSelected">Zero based index of selected dialog choice.</param>
        /// <returns></returns>
        public static string PeekTzenThiefRewardWob (byte dialogWaitingForInput, byte dialogPointer, byte dialogChoiceSelected)
        {
            string tzenThiefRewardWob = "Did_not_check";
            if (dialogWaitingForInput != 0)
            {
                if ((dialogChoiceSelected == 0 && dialogPointer == 4) || (dialogChoiceSelected == 1 && dialogPointer == 6))
                {
                    tzenThiefRewardWob = "Esper";
                }
                else if ((dialogChoiceSelected == 0 && dialogPointer == 2) || (dialogChoiceSelected == 1 && dialogPointer == 4))
                {
                    tzenThiefRewardWob = "Item";
                }
            }
            return tzenThiefRewardWob;
        }

        /// <summary>
        /// Method that checks if the player peeked World of Ruin Tzen thief.
        /// </summary>
        /// <param name="dialogIndex">The dialog index.</param>
        /// <returns></returns>
        public static string PeekTzenThiefRewardWor(int dialogIndex)
        {
            string tzenThiefRewardWor = "Did_not_check";
            
            if (dialogIndex == 1570)
            {
                tzenThiefRewardWor = "Unknown";
            }
            return tzenThiefRewardWor;
        }

        /// <summary>
        /// Method that takes the character bytes and counts the characters currently available.
        /// </summary>
        /// <param name="charactersBytes">An array that contains the bytes of the available characters.</param>
        /// <returns>The amount of characters available.</returns>
        public static int GetCharacterCount(byte[] charactersBytes)
        {
            // Top 2 bits of the second byte are not used.
            int result = CountSetBits(charactersBytes[0]) + CountSetBits((byte)(charactersBytes[1] & 0x3F));
            return result;
        }

        /// <summary>
        /// Method that iterates through character data and extracts the list of available commands in the seed.
        /// </summary>
        /// <param name="characterData">An array containing the full character data.</param>
        /// <returns>An array containing all available commands in the seed.</returns>
        public static byte[] GetCharacterCommands(byte[] characterData)
        {
            byte[] characterCommands = new byte[13]; // 12 characters + Gau's extra command (no Gogo or Umaro).

            // Iterate through characterData and store all available commands in an array.
            for (uint i = 0; i < 12; i++)
            {
                characterCommands[i] = characterData[i * 37 + 23];
            }
            // Manually get Gau's 1st command.
            characterCommands[12] = characterData[11 * 37 + 22];
            return characterCommands;
        }

        /// <summary>
        /// Method that creates a list of the available characters at the current point in the game.
        /// </summary>
        /// <param name="charactersBytes">An array that contains the bytes of the available characters.</param>
        /// <returns>A list with the available characters.</returns>
        public static List<string> GetAvailableCharacters(byte[] charactersBytes)
        {
            var startingCharacters = new List<string>();
            for (int i = 0; i < 8; i++) // First byte check.
            {
                if (CheckBitSet(charactersBytes[0], WCData.BitFlags[i]))
                {
                    startingCharacters.Add(WCData.CharacterNames[i]);
                }
            }
            for (int i = 0; i < 6; i++) // Second byte check.
            {
                if (CheckBitSet(charactersBytes[1], WCData.BitFlags[i]))
                {
                    startingCharacters.Add(WCData.CharacterNames[i + 8]);
                }
            }
            return startingCharacters;
        }

        /// <summary>
        /// Method that creates a list of the available commands at the current point in the game.
        /// </summary>
        /// <param name="charactersBytes">An array that contains the bytes of the available characters.</param>
        /// <param name="characterCommands">An array that contains the full list of commands available in the seed.</param>
        /// <returns>A list with the available commands.</returns>
        public static List<string> GetAvailableCommands(byte[] charactersBytes, byte[] characterCommands)
        {
            var startingCommands = new List<string>();
            for (int i = 0; i < 8; i++) // First byte check.
            {
                if (CheckBitSet(charactersBytes[0], WCData.BitFlags[i]))
                {
                    string command = WCData.CommandDict[characterCommands[i]];
                    if (!startingCommands.Contains(command))
                    {
                        string commandReplaced = command.Replace(" - ", "__");
                        commandReplaced = command.Replace(" ", "_");
                        startingCommands.Add(commandReplaced);
                    }
                }
            }
            for (int i = 0; i < 4; i++) // Second byte check. Skip Gogo and Umaro (they don't have commands).
            {
                if (CheckBitSet(charactersBytes[1], WCData.BitFlags[i]))
                {
                    string command = WCData.CommandDict[characterCommands[i + 8]];
                    if (!startingCommands.Contains(command))
                    {
                        string commandReplaced = command.Replace(" - ", "__");
                        commandReplaced = command.Replace(" ", "_");
                        startingCommands.Add(commandReplaced);
                    }
                }
            }
            if (CheckBitSet(charactersBytes[1], WCData.BitFlags[3])) // If Gau is in the party, get his 2nd command.
            {
                string command = WCData.CommandDict[characterCommands[12]];
                if (!startingCommands.Contains(command))
                {
                    string commandReplaced = command.Replace(" - ", "__");
                    commandReplaced = command.Replace(" ", "_");
                    startingCommands.Add(commandReplaced);
                }
            }
            return startingCommands;
        }

        /// <summary>
        /// Method that creates a list of killed dragons at the current point in the game.
        /// </summary>
        /// <param name="dragonsBytes">An array that contains the bytes of the killed dragons.</param>
        /// <returns></returns>
        public static List<string> GetDragonsKilled(byte[] dragonsBytes)
        {
            var dragonsKilled = new List<string>();
            for (int i = 0; i < 6; i++)
            {
                if (CheckBitSet(dragonsBytes[0], WCData.DragonFlags1[i]))
                {
                    string dragon = WCData.DragonDict[WCData.DragonFlags1[i]];
                    dragon = dragon.Replace(" - ", "__");
                    dragon = dragon.Replace(" ", "_");
                    dragonsKilled.Add(dragon);
                }
            }
            for (int i = 0; i < 2; i++)
            {
                if (CheckBitSet(dragonsBytes[1], WCData.DragonFlags2[i]))
                {
                    string dragon = WCData.DragonDict[WCData.DragonFlags2[i]];
                    dragon = dragon.Replace(" - ", "__");
                    dragon = dragon.Replace(" ", "_");
                    dragonsKilled.Add(dragon);
                }
            }
            return dragonsKilled;
        }

        /// <summary>
        /// Method that takes the chest data and counts the amount of chests opened at the current point in the game.
        /// </summary>
        /// <param name="chestData">An array containing the bytes of chests opened.</param>
        /// <returns>The count of chests opened.</returns>
        public static int GetChestCount(byte[] chestData)
        {
            int chestCount = 0;
            for (int i = 0; i < chestData.Length; i++)
            {
                chestCount += CountSetBits(chestData[i]);
            }
            return chestCount;
        }

        /// <summary>
        /// Method that takes the character data and gets the maximum character level at the current point in the game.
        /// </summary>
        /// <param name="characterData">An array containing the full character data.</param>
        /// <returns>The maximum level of all characters.</returns>
        public static byte GetMaximumCharacterLevel(byte[] characterData)
        {
            byte characterMaxLevel = 0;
            for (int i = 0; i < 14; i++)
            {
                if (characterData[i * 37 + 8] > characterMaxLevel)
                {
                    characterMaxLevel = characterData[i * 37 + 8];
                }
            }
            return characterMaxLevel;
        }

        /// <summary>
        /// Method that checks if a given item exists in the inventory.
        /// </summary>
        /// <param name="inventoryData">The inventory byte array.</param>
        /// <param name="itemValue">Byte value of the item to check.</param>
        /// <returns>true if the item exists in inventory, otherwise false.</returns>
        public static bool CheckIfItemExistsInInventory(byte[] inventoryData, byte itemValue)
        {
            bool itemExists = false;
            for (int i = 0; i < WCData.InventorySize; i++)
            {
                if (inventoryData[i] == itemValue)
                {
                    itemExists = true;
                    break;
                }
            }
            return itemExists;
        }

        /// <summary>
        /// Method that takes a byte and a flag and checks if the bit is set.
        /// </summary>
        /// <param name="data">The byte to check.</param>
        /// <param name="flag">The flag to use.</param>
        /// <returns>true if the bit is set, otherwise false.</returns>
        public static bool CheckBitSet(int data, int flag)
        {
            bool isSet = false;
            if ((data & flag) != 0)
            {
                isSet = true;
            }
            return isSet;
        }

        /// <summary>
        /// Method that takes a byte and a bit offset and checks if the bit is set.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static bool CheckBitByOffset(byte data, int offset)
        {
            bool isSet = false;
            if ((data & WCData.BitFlags[offset % 8]) != 0)
            {
                isSet = true;
            }
            return isSet;
        }

        /// <summary>
        /// Method that takes a byte and checks how many bits are set.
        /// </summary>
        /// <param name="value">The byte to check.</param>
        /// <returns></returns>
        public static byte CountSetBits(byte value)
        {
            byte bitsSet = 0;
            for (int i = 0; i < 8; i++)
            {
                if ((value & (1 << i)) > 0)
                {
                    bitsSet++;
                }
            }
            return bitsSet;
        }

        /// <summary>
        /// Method that takes a byte array and concatenates it into a single integer.
        /// </summary>
        /// <param name="byteArray">The byte array to concatenate</param>
        /// <returns></returns>
        public static int ConcatenateByteArray(byte[] byteArray)
        {
            int concatenatedData = 0;
            for (int i = byteArray.Length - 1; i >= 0; i--)
            {
                concatenatedData += byteArray[i] << i * 8;
            }
            return concatenatedData;
        }
    }
}
