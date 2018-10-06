using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Demo_FileIO_NTier.DataAccessLayer;
using Demo_FileIO_NTier.Models;

namespace Demo_FileIO_NTier.BusinessLogicLayer
{
    public class CharacterBLL
    {
        private IDataService _dataService;
        private List<Character> _character;

        /// <summary>
        /// Constructor for the business tier
        /// </summary>
        /// <param name="dataService"></param>
        public CharacterBLL(IDataService dataService)
        {
            _dataService = dataService;
        }

        public IEnumerable<Character> GetCharacters(out bool success, out string message)
        {
            _character = null;
            success = false;
            message = "";

            try
            {
                _dataService = new CsvDataService();
                _character = _dataService.ReadAll() as List<Character>;
                _character.OrderBy(c => c.Id);

                if (_character.Count > 0)
                {
                    success = true;
                }
                else
                {
                    message = "No data. Is the file empty? ¯\\_(ツ)_/¯";
                }
            }
            catch (FileNotFoundException)
            {
                message = "The file is not found. Does the file exist? ¯\\_(ツ)_/¯";
            }
            // added additional catch statement in case the cast does not work as intended
            catch (InvalidCastException)
            {
                message = "Operation cannot be casted";
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return _character;
        }
    }
}