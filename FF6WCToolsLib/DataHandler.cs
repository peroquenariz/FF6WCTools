using FF6WCToolsLib.DataTemplates;
using System;
using System.Collections.Generic;
using static FF6WCToolsLib.WCData;

namespace FF6WCToolsLib;

/// <summary>
/// Provides methods for processing FF6's memory and data.
/// </summary>
public static class DataHandler
{
    /// <summary>
    /// Takes the full item equipability data and extracts the equipability for the current battle characters.
    /// </summary>
    /// <param name="fullEquipability">Concatenated data of item equipability.</param>
    /// <returns>A byte with the battle equipability flags.</returns>
    public static byte GetEquipability(int fullEquipability)
    {
        byte equipFlags = 0b00001111; // Initialize flags as unequippable.
        if (BattleActorData.CurrentLineupData == null) return equipFlags;

        // Iterate each battle character and toggle equipability bit.
        for (int i = 0; i < BattleActorData.CurrentLineupData.Length; i++)
        {
            int characterIndex = BattleActorData.CurrentLineupData[i];
            if (CheckBitSet(fullEquipability, characterIndex))
            {
                equipFlags = ToggleBit(equipFlags, (byte)(1 << i));
            }
        }

        return equipFlags;
    }

    public static byte[] GetBattleLineup(byte[] masks)
    {
        byte[] battleLineup = InitializeArrayWithData(4, 0xFF);

        for (byte i = 0; i < masks.Length; i++)
        {
            byte maskValue = masks[i];
            if (maskValue != 0xFF)
            {
                // Halve maskValue to get the character order
                // 0: top, 3: bottom
                battleLineup[maskValue / 2] = i;
            }
        }

        return battleLineup;
    }
    
    /// <summary>
    /// Checks if the item is a consumable or throwable (non-equippable) item.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <returns>True if the item is non-equippable, otherwise false.</returns>
    public static bool IsItemConsumable(Item item)
    {
        return CheckItemInRange(item, RANGE_CONSUMABLES) ||
               CheckItemInRange(item, RANGE_SKEANS) ||
               CheckItemInRange(item, RANGE_NINJASTARS);
    }

    public static void InitializeInverseCharDict()
    {
        // Build inverse character dictionary
        for (byte i = 0x80; i < 0xC6; i++)
        {
            CHAR_TO_BYTE_DICT.Add(CHAR_DICT[i], i);
        }
        CHAR_TO_BYTE_DICT.Add(' ', 0xFF);
    }

    /// <summary>
    /// Takes 2 arrays of the same size and type and checks if they have the same data.
    /// </summary>
    /// <returns>true if they're equal, otherwise false.</returns>
    public static bool AreArraysEqual(byte[] arr1, byte[] arr2)
    {
        bool equal = false;
        
        if (arr1.Length != arr2.Length) return equal;
        
        for (int i = 0; i < arr1.Length; i++)
        {
            if (arr1[i] != arr2[i])
            {
                return equal;
            }
        }
        
        equal = true;
        return equal;
    }

    /// <summary>
    /// Concatenates the monster indexes.
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
    /// Checks if the Tzen Thief reward was bought.
    /// </summary>
    /// <param name="esperCount">Number of espers owned when entering Tzen Thief area.</param>
    /// <param name="esperCountPrevious">Number of espers owned when exiting Tzen Thief area.</param>
    /// <param name="tzenThiefBit">Event bit that indicates if Tzen Thief was bought.</param>
    /// <returns></returns>
    public static TzenThiefBought CheckTzenThiefBought(byte esperCount, byte esperCountPrevious, bool tzenThiefBit)
    {
        TzenThiefBought tzenThiefBought = TzenThiefBought.None;
        if (tzenThiefBit && esperCount == esperCountPrevious)
        {
            tzenThiefBought = TzenThiefBought.Item;
        }
        else if (tzenThiefBit && esperCount > esperCountPrevious)
        {
            tzenThiefBought = TzenThiefBought.Esper;
        }
        return tzenThiefBought;
    }

    /// <summary>
    /// Checks if the player peeked what reward World of Balance Tzen thief has.
    /// </summary>
    /// <param name="dialogWaitingForInput">Value is 1 if the dialog box is waiting for an input.</param>
    /// <param name="dialogPointer">Position of the dialog choice pointer.</param>
    /// <param name="dialogChoiceSelected">Zero based index of selected dialog choice.</param>
    /// <returns></returns>
    public static TzenThiefPeekWob PeekTzenThiefRewardWob (byte dialogWaitingForInput, byte dialogPointer, byte dialogChoiceSelected)
    {
        TzenThiefPeekWob tzenThiefRewardWob = TzenThiefPeekWob.Did_not_check;
        
        // Tzen Thief WoB dialog box height is different due to it having more lines if it has the "glowing stone" text.
        // If the dialog box is waiting for an input (Yes/No)
        if (dialogWaitingForInput != 0)
        {
            // If it's an esper, the "glowing stone" text will make the textbox have an extra line, so dialogPointer will be a higher value.
            // "Yes" choice will be dialogPointer value 4, "No" choice will be 6
            if ((dialogChoiceSelected == 0 && dialogPointer == 4) || (dialogChoiceSelected == 1 && dialogPointer == 6))
            {
                tzenThiefRewardWob = TzenThiefPeekWob.Esper;
            }
            // If it's an esper, "Yes" choice will be dialogPointer value 2, "No" choice will be 4
            else if ((dialogChoiceSelected == 0 && dialogPointer == 2) || (dialogChoiceSelected == 1 && dialogPointer == 4))
            {
                tzenThiefRewardWob = TzenThiefPeekWob.Item;
            }
        }
        return tzenThiefRewardWob;
    }

    /// <summary>
    /// Checks if the player peeked World of Ruin Tzen thief.
    /// </summary>
    /// <param name="dialogIndex">The dialog index.</param>
    /// <returns></returns>
    public static TzenThiefPeekWor PeekTzenThiefRewardWor(int dialogIndex)
    {
        TzenThiefPeekWor tzenThiefRewardWor = TzenThiefPeekWor.Did_not_check;
        
        if (dialogIndex == DIALOG_INDEX_WOR_TZEN_THIEF)
        {
            tzenThiefRewardWor = TzenThiefPeekWor.Unknown;
        }
        return tzenThiefRewardWor;
    }

    /// <summary>
    /// Takes the character bytes and counts the characters currently available.
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
    /// Iterates through character data and extracts the list of available commands in the seed.
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
    /// Creates a list of the available characters at the current point in the game.
    /// </summary>
    /// <param name="charactersBytes">An array that contains the bytes of the available characters.</param>
    /// <returns>A list with the available characters.</returns>
    public static List<string> GetAvailableCharacters(byte[] charactersBytes)
    {
        var availableCharacters = new List<string>();
        
        for (int i = 0; i < 8; i++) // First byte check.
        {
            if (CheckBitSet(charactersBytes[0], BIT_FLAGS[i]))
            {
                availableCharacters.Add(CHARACTER_NAMES[i]);
            }
        }
        
        for (int i = 0; i < 6; i++) // Second byte check.
        {
            if (CheckBitSet(charactersBytes[1], BIT_FLAGS[i]))
            {
                availableCharacters.Add(CHARACTER_NAMES[i + 8]);
            }
        }
        
        return availableCharacters;
    }

    /// <summary>
    /// Creates a list of the available commands at the current point in the game.
    /// </summary>
    /// <param name="charactersBytes">An array that contains the bytes of the available characters.</param>
    /// <param name="characterCommands">An array that contains the full list of commands available in the seed.</param>
    /// <returns>A list with the available commands.</returns>
    public static List<string> GetAvailableCommands(byte[] charactersBytes, byte[] characterCommands)
    {
        var availableCommands = new List<string>();
        
        for (int i = 0; i < 8; i++) // First byte check.
        {
            if (CheckBitSet(charactersBytes[0], BIT_FLAGS[i]))
            {
                string command = COMMAND_DICT[characterCommands[i]];
                if (!availableCommands.Contains(command))
                {
                    availableCommands.Add(command);
                }
            }
        }
        
        for (int i = 0; i < 4; i++) // Second byte check. Skip Gogo and Umaro (they don't have commands).
        {
            if (CheckBitSet(charactersBytes[1], BIT_FLAGS[i]))
            {
                string command = COMMAND_DICT[characterCommands[i + 8]];
                if (!availableCommands.Contains(command))
                {
                    availableCommands.Add(command);
                }
            }
        }
        
        if (CheckBitSet(charactersBytes[1], BIT_FLAGS[3])) // If Gau is in the party, get his 2nd command.
        {
            string command = COMMAND_DICT[characterCommands[12]];
            if (!availableCommands.Contains(command))
            {
                availableCommands.Add(command);
            }
        }

        return availableCommands;
    }

    /// <summary>
    /// Creates a list of killed dragons at the current point in the game.
    /// </summary>
    /// <param name="dragonsBytes">An array that contains the bytes of the killed dragons.</param>
    /// <returns></returns>
    public static List<string> GetDragonsKilled(byte[] dragonsBytes)
    {
        var dragonsKilled = new List<string>();
        
        for (int i = 0; i < 6; i++)
        {
            if (CheckBitSet(dragonsBytes[0], DRAGON_FLAGS_1[i]))
            {
                string dragon = DRAGON_DICT[DRAGON_FLAGS_1[i]];
                dragonsKilled.Add(dragon);
            }
        }
        
        for (int i = 0; i < 2; i++)
        {
            if (CheckBitSet(dragonsBytes[1], DRAGON_FLAGS_2[i]))
            {
                string dragon = DRAGON_DICT[DRAGON_FLAGS_2[i]];
                dragonsKilled.Add(dragon);
            }
        }
        return dragonsKilled;
    }

    /// <summary>
    /// Takes the chest data and counts the amount of chests opened at the current point in the game.
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
    /// Takes the character data and gets the maximum character level at the current point in the game.
    /// </summary>
    /// <param name="characterData">An array containing the full character data.</param>
    /// <returns>The maximum level of all characters.</returns>
    public static byte GetMaximumCharacterLevel(byte[] characterData)
    {
        byte characterMaxLevel = 0;
        
        for (int i = 0; i < 14; i++)
        {
            byte characterLevel = characterData[i * 37 + CHARACTER_DATA_LEVEL_OFFSET];
            if (characterLevel > characterMaxLevel)
            {
                characterMaxLevel = characterLevel;
            }
        }
        
        return characterMaxLevel;
    }

    /// <summary>
    /// Checks if a given item exists in the inventory.
    /// </summary>
    /// <param name="inventoryData">The inventory byte array.</param>
    /// <param name="itemValue">Byte value of the item to check.</param>
    /// <returns>true if the item exists in inventory, otherwise false.</returns>
    public static bool CheckIfItemExistsInInventory(byte[] inventoryData, byte itemValue)
    {
        bool itemExists = false;
        
        for (int i = 0; i < INVENTORY_SIZE; i++)
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
    /// Takes a byte and a bit flag and checks if the bit is set.
    /// </summary>
    /// <param name="data">The byte to check.</param>
    /// <param name="flag">The bit flag to use.</param>
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
    /// Toggles a bit in a given byte.
    /// </summary>
    /// <param name="data">The byte to modify.</param>
    /// <param name="flag">The bit to flip.</param>
    /// <returns></returns>
    public static byte ToggleBit(byte data, byte flag)
    {
        // Check if the bit is set.
        bool isBitSet = CheckBitSet(data, flag);

        // Toggle on/off depending its status.
        if (isBitSet)
        {
            return data &= (byte)~flag;
        }
        else
        {
            return data |= flag;
        }
    }

    /// <summary>
    /// Takes a byte and a bit offset and checks if the bit is set.
    /// </summary>
    /// <param name="data">The byte to check.</param>
    /// <param name="offset">The byte offset.</param>
    /// <returns></returns>
    public static bool CheckBitByOffset(byte data, int offset)
    {
        bool isSet = false;
        
        if ((data & BIT_FLAGS[offset % 8]) != 0)
        {
            isSet = true;
        }
        
        return isSet;
    }

    /// <summary>
    /// Takes a byte and checks how many bits are set.
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
    /// Takes a byte array and concatenates it into a single integer.
    /// </summary>
    /// <param name="byteArray">The byte array to concatenate.</param>
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

    /// <summary>
    /// Decatenates an integer into a byte array of a specified size.
    /// </summary>
    /// <param name="number">The number to decatenate.</param>
    /// <param name="numberSizeInBytes">Size of the data.</param>
    /// <returns></returns>
    public static byte[] DecatenateInteger(int number, int numberSizeInBytes)
    {
        byte[] array = BitConverter.GetBytes(number);
        return array[..numberSizeInBytes];
    }

    /// <summary>
    /// Checks if a given item index is part of an item range.
    /// </summary>
    /// <param name="itemIndex">The item index to check.</param>
    /// <param name="range">The item range to compare against.</param>
    /// <returns>True if the item is in range, otherwise false.</returns>
    public static bool CheckItemInRange(byte itemIndex, Range range)
    {
        return itemIndex >= range.Start.Value && itemIndex <= range.End.Value;
    }

    /// <summary>
    /// Checks if a given item is part of an item range.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <param name="range">The item range to compare against.</param>
    /// <returns>True if the item is in range, otherwise false.</returns>
    public static bool CheckItemInRange(Item item, Range range)
    {
        return CheckItemInRange((byte)item, range);
    }

    /// <summary>
    /// Extracts the character name from the character data in SRAM.
    /// These might change if a rename card was used, or in crowd control!
    /// </summary>
    /// <param name="characterData">The data block of a given character.</param>
    /// <returns>A name string with the white-spaces trimmed.</returns>
    public static string GetCharacterName(byte[] characterData)
    {
        char[] characterName = new char[CHARACTER_DATA_NAME_SIZE];
        byte[] characterNameBytes = characterData[CHARACTER_DATA_NAME_OFFSET..(CHARACTER_DATA_NAME_OFFSET + 6)];

        for (byte i = 0; i < characterName.Length; i++)
        {
            characterName[i] = CHAR_DICT[characterNameBytes[i]];
        }

        string name = new string(characterName);

        return name.Trim();
    }

    /// <summary>
    /// Takes the byte that contains an item stat boost properties,
    /// and extracts the stat boosts and plus/minus signals.
    /// Structure: [VIG/STAM | VIG/STAM sign | SPEED/MAGPOW | SPEED/MAGPOW sign]
    /// </summary>
    /// <param name="itemStatBoost">The item stat boost byte.</param>
    /// <returns>An array containing the separated info.</returns>
    public static byte[] GetItemStatBoostInfo(byte itemStatBoost)
    {
        byte[] statBoostInfo = new byte[4];

        statBoostInfo[0] = (byte)(itemStatBoost & 0x07);
        statBoostInfo[1] = (byte)(itemStatBoost & 0x08);
        statBoostInfo[2] = (byte)((itemStatBoost & 0x70) >> 4);
        statBoostInfo[3] = (byte)((itemStatBoost & 0x80) >> 4);

        return statBoostInfo;
    }

    public static StatBoostValues GetStatBoostValues(byte vigorAndSpeed, byte staminaAndMagic)
    {
        byte[] vigorAndSpeedData = GetItemStatBoostInfo(vigorAndSpeed);
        byte[] staminaAndMagicData = GetItemStatBoostInfo(staminaAndMagic);

        int vigor = vigorAndSpeedData[1] == 0 ? vigorAndSpeedData[0] : vigorAndSpeedData[0] * -1;
        int speed = vigorAndSpeedData[3] == 0 ? vigorAndSpeedData[2] : vigorAndSpeedData[2] * -1;
        int stamina = staminaAndMagicData[1] == 0 ? staminaAndMagicData[0] : staminaAndMagicData[0] * -1;
        int magic = staminaAndMagicData[3] == 0 ? staminaAndMagicData[2] : staminaAndMagicData[2] * -1;

        return new StatBoostValues(vigor, speed, stamina, magic);
    }

    /// <summary>
    /// Constructs a byte with the given stat boost information.
    /// Use it with bitwise operators, otherwise you'll overwrite other stat boosts.
    /// </summary>
    /// <param name="statType">Which one of the 4 stats to modify.</param>
    /// <param name="value">The value of the stat boost.</param>
    /// <param name="statBoostData">A byte with the 4 corresponding bits of data.</param>
    /// <returns>true if the stat resides in the high 4 bits, otherwise false.</returns>
    public static bool SetStatBoost(Stat statType, int value, out byte statBoostData)
    {
        // Clamp the value from -7 to 7. Should be sanitized in Crowd Control anyways.
        int clampedValue = Math.Clamp(value, -7, 7);
        
        statBoostData = 0;
        byte offset = 0;

        // Set the sign of the boost.
        bool isNegative = clampedValue < 0;
        
        // Save absolute value of the boost.
        byte absValue = (byte)MathF.Abs(clampedValue);

        // Offset the data if it's one of the stats in the high 4 bytes.
        bool isHighBits = statType is Stat.speed or Stat.magic;
        if (isHighBits) offset = 4;

        // Create the byte data.
        statBoostData |= (byte)(absValue << offset);
        if (isNegative)
        {
            statBoostData |= (byte)(0x08 << offset);
        }

        return isHighBits;
    }

    /// <summary>
    /// Takes a byte array and extracts the name or text stored.
    /// </summary>
    /// <param name="nameBytes">The text data in bytes.</param>
    /// <returns>A trimmed string with the name.</returns>
    public static string ExtractName(byte[] nameBytes)
    {
        char[] nameChars = new char[nameBytes.Length];

        for (byte i = 0; i < nameBytes.Length; i++)
        {
            nameChars[i] = CHAR_DICT[nameBytes[i]];
        }

        return new string(nameChars).Trim();
    }

    /// <summary>
    /// Takes a string and encodes the name in a byte array.
    /// </summary>
    /// <param name="name">The name to encode.</param>
    /// <returns>A byte array with the encoded name.</returns>
    public static byte[] EncodeName(string name, int blockSize, byte fillData = 0xFF)
    {
        int maxCharacterCount = name.Length;

        if (name.Length > blockSize)
        {
            maxCharacterCount = blockSize;
        }

        byte[] encodedName = InitializeArrayWithData(blockSize, fillData);
        
        for (int i = 0; i < maxCharacterCount; i++)
        {
            char character = name[i];
            if (character == ' ' && fillData != 0xFF)
            {
                encodedName[i] = fillData;
                continue;
            }
            encodedName[i] = CHAR_TO_BYTE_DICT[character];
        }

        return encodedName;
    }

    /// <summary>
    /// Creates an array filled with a given type of data.
    /// </summary>
    /// <param name="arraySize">The size of the array to initialize.</param>
    /// <param name="defaultData">The byte to fill the array with.</param>
    /// <returns>An array initialized with a given byte.</returns>
    public static byte[] InitializeArrayWithData(int arraySize, byte defaultData)
    {
        byte[] byteArray = new byte[arraySize];

        for (int i = 0; i < arraySize; i++)
        {
            byteArray[i] = defaultData;
        }

        return byteArray;
    }

    /// <summary>
    /// Reads the NMI jump code and extracts the game state.
    /// </summary>
    /// <param name="gameStateData">NMI jump code data.</param>
    /// <returns>The current game state.</returns>
    public static GameState GetGameState(byte[] gameStateData)
    {
        int firstTwoBytes = ConcatenateByteArray(gameStateData[0..2]);
        byte lastByte = gameStateData[2];
        GameState gameState;

        // Look at these magic numbers, aren't they beautiful?
        // These values are taken from the FF6 TAS Lua script.
        // I have no idea where they came from, but they work. :)
        // Thanks JCMTG for showing me the script.
        if (firstTwoBytes == 0x0ba7 && lastByte == 0xC1)
        {
            gameState = GameState.BATTLE;
        }
        else if (firstTwoBytes == 0x0182 && lastByte == 0xC0)
        {
            gameState = GameState.FIELD;
        }
        else if (firstTwoBytes == 0xa728 && lastByte == 0xEE)
        {
            gameState = GameState.WORLD;
        }
        else if (firstTwoBytes == 0x1387 && lastByte == 0xC3)
        {
            gameState = GameState.MENU;
        }
        else if ((firstTwoBytes == 0xa509 && lastByte == 0xEE) ||
                    (firstTwoBytes == 0xa94d && lastByte == 0xEE))
        {
            gameState = GameState.MODE7;
        }
        else
        {
            // We discovered a new game state!
            gameState = GameState.UNKNOWN;
        }

        return gameState;
    }

    /// <summary>
    /// Looks for a given character in the current lineup data, and determines if it's in battle.
    /// </summary>
    /// <param name="character">The character to check.</param>
    /// <returns>true if the character is in battle, false if it isn't or if the game isn't in battle mode.</returns>
    public static bool IsCharacterInBattle(Character character, out int actorIndex)
    {
        bool isCharacterInBattle = false;
        actorIndex = 0;

        if (BattleActorData.CurrentLineupData != null)
        {
            for (int i = 0; i < BattleActorData.CurrentLineupData.Length; i++)
            {
                if (BattleActorData.CurrentLineupData[i] == (int)character)
                {
                    isCharacterInBattle = true;
                    actorIndex = i;
                    break;
                }
            } 
        }

        return isCharacterInBattle;
    }
}