using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_FileIO
{
    public class CharactersBLL
    {
        IDataService _dataService;
        List<Character> _characters;

        public IEnumerable<Character> GetCharacters()
        {
            return _characters;
        }

        public Character GetCharacterById(int id)
        {
            return _characters.FirstOrDefault(c => c.Id == id);
        }

        public void AddCharacter(Character character)
        {
            _characters.Add(character);
            _dataService.WriteAll(_characters);
        }

        public void UpdateCharacter(Character character)
        {

        }

        public CharactersBLL()
        {
            _dataService = new CsvDataService();
            _characters = _dataService.ReadAll() as List<Character>;
        }
    }
}
