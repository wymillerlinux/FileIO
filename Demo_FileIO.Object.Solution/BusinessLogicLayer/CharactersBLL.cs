using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Demo_FileIO
{
    public class CharactersBLL
    {
        IDataService _dataService;
        List<Character> _characters;

        public IEnumerable<Character> GetCharacters(out bool success, out string message)
        {
            _characters = null;
            success = false;
            message = "";
            try
            {
                _dataService = new CsvDataService();
                _characters = _dataService.ReadAll() as List<Character>;
                if (_characters.Count > 0)
                {
                    success = true;
                }
                else
                {
                    message = "It appears there is no data in the file.";
                }
            }
            catch (FileNotFoundException)
            {
                message = "Unable to locate the data file.";
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return _characters;
        }

        public Character GetCharacterById(int id, out bool success, out string message)
        {
            success = false;
            message = "";
            Character character = new Character();

            if (ValidCharacterIds().Contains(id))
            {
                try
                {
                    _dataService = new CsvDataService();
                    _characters = _dataService.ReadAll() as List<Character>;
                    character = _characters.FirstOrDefault(c => c.Id == id);
                    success = true;
                }
                catch (FileNotFoundException)
                {
                    message = "Unable to locate the data file.";
                }
                catch (Exception e)
                {
                    message = e.Message;
                }
            }
            else
            {
                message = "Invalid character Id entered.";
            }

            return character;
        }

        public void AddCharacter(Character character, out bool success, out string message)
        {
            success = false;
            message = "";
            try
            {
                _dataService = new CsvDataService();
                _characters = _dataService.ReadAll() as List<Character>;
                character.Id = MaximumCurrentId() + 1;
                _characters.Add(character);
                _dataService.WriteAll(_characters);
                success = true;
                message = "Character added.";
            }
            catch (FileNotFoundException)
            {
                message = "Unable to locate the data file.";
            }
            catch (Exception e)
            {
                message = e.Message;
            }
        }

        public void UpdateCharacter(Character character, out bool success, out string message)
        {
            success = false;
            message = "";

            //_characters.RemoveAll(c => c.Id == character.Id);
            //_characters.Add(character);
            //_dataService.WriteAll(_characters);

            try
            {
                _dataService = new CsvDataService();
                _characters = _dataService.ReadAll() as List<Character>;
                _characters.RemoveAll(c => c.Id == character.Id);
                _characters.Add(character);
                _dataService.WriteAll(_characters);
                success = true;
                message = "Character updated.";
            }
            catch (FileNotFoundException)
            {
                message = "Unable to locate the data file.";
            }
            catch (Exception e)
            {
                message = e.Message;
            }
        }

        public void DeleteCharacter(int id, out bool success, out string message)
        {
            success = false;
            message = "";

            if (ValidCharacterIds().Contains(id))
            {
                try
                {
                    _dataService = new CsvDataService();
                    _characters = _dataService.ReadAll() as List<Character>;
                    _characters.RemoveAll(c => c.Id == id);
                    _dataService.WriteAll(_characters);
                    success = true;
                    message = "Character deleted.";
                }
                catch (FileNotFoundException)
                {
                    message = "Unable to locate the data file.";
                }
                catch (Exception e)
                {
                    message = e.Message;
                }
            }
            else
            {
                message = "Invalid character Id entered.";
            }
        }

        private int MaximumCurrentId()
        {
            return _characters.Max(c => c.Id);
        }

        private IEnumerable<int> ValidCharacterIds()
        {
            return _characters.Select(c => c.Id).Distinct();
        }
    }
}
