using System.Collections.Generic;
using FF6WCToolsLib;

namespace StatsCompanionLib
{
    /// <summary>
    /// A class representing a Worlds Collide character.
    /// </summary>
    public class Character
    {
        private string _name;
        private byte _level;
        private byte[] _characterSpellsData;
        private byte _vigor;
        private byte _speed;
        private byte _stamina;
        private byte _magpower;
        private string _esper;
        private string _rHand;
        private string _lHand;
        private string _helmet;
        private string _armor;
        private string _relic1;
        private string _relic2;
        private List<string> _commands;
        private List<string> _spells;

        public string Name { get => _name; set => _name = value; }
        public byte Level { get => _level; set => _level = value; }
        public byte Vigor { get => _vigor; set => _vigor = value; }
        public byte Speed { get => _speed; set => _speed = value; }
        public byte Stamina { get => _stamina; set => _stamina = value; }
        public byte Magpower { get => _magpower; set => _magpower = value; }
        public string Esper { get => _esper; set => _esper = value; }
        public string RHand { get => _rHand; set => _rHand = value; }
        public string LHand { get => _lHand; set => _lHand = value; }
        public string Helmet { get => _helmet; set => _helmet = value; }
        public string Armor { get => _armor; set => _armor = value; }
        public string Relic1 { get => _relic1; set => _relic1 = value; }
        public string Relic2 { get => _relic2; set => _relic2 = value; }
        public List<string> Commands { get => _commands; set => _commands = value; }
        public List<string> Spells { get => _spells; set => _spells = value; }

        public Character(byte[] characterData, byte[] characterSkillsData, string name)
        {
            _name = name;
            _level = characterData[0x08];
            _commands = new() {
                WCData.CommandDict[characterData[0x16]], 
                WCData.CommandDict[characterData[0x17]],
                WCData.CommandDict[characterData[0x18]], 
                WCData.CommandDict[characterData[0x19]] 
            };
            _vigor = characterData[0x1A];
            _speed = characterData[0x1B];
            _stamina = characterData[0x1C];
            _magpower = characterData[0x1D];
            _esper = WCData.EsperDict[characterData[0x1E]];
            _rHand = WCData.ItemDict[characterData[0x1F]];
            _lHand = WCData.ItemDict[characterData[0x20]];
            _helmet = WCData.ItemDict[characterData[0x21]];
            _armor = WCData.ItemDict[characterData[0x22]];
            _relic1 = WCData.ItemDict[characterData[0x23]];
            _relic2 = WCData.ItemDict[characterData[0x24]];
            _characterSpellsData = characterSkillsData;
            _spells = new List<string>();
            GetSpellList(_characterSpellsData);
        }

        public void GetSpellList(byte[] characterSpells)
        {
            for (byte i = 0; i < characterSpells.Length; i++)
            {
                if (characterSpells[i] == 0xFF)
                {
                    Spells.Add(WCData.SpellDict[i]);
                }
            } 
        }
    }
}
