using System.Collections.Generic;

namespace StatsCompanion
{
    /// <summary>
    /// A class representing a Worlds Collide character.
    /// </summary>
    internal class Character
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
            Name = name;
            Level = characterData[0x08];
            Commands = new() {
                WCData.CommandDict[characterData[0x16]], 
                WCData.CommandDict[characterData[0x17]],
                WCData.CommandDict[characterData[0x18]], 
                WCData.CommandDict[characterData[0x19]] 
            };
            Vigor = characterData[0x1A];
            Speed = characterData[0x1B];
            Stamina = characterData[0x1C];
            Magpower = characterData[0x1D];
            Esper = WCData.EsperDict[characterData[0x1E]];
            RHand = WCData.ItemDict[characterData[0x1F]];
            LHand = WCData.ItemDict[characterData[0x20]];
            Helmet = WCData.ItemDict[characterData[0x21]];
            Armor = WCData.ItemDict[characterData[0x22]];
            Relic1 = WCData.ItemDict[characterData[0x23]];
            Relic2 = WCData.ItemDict[characterData[0x24]];
            _characterSpellsData = characterSkillsData;
            Spells = new();
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
