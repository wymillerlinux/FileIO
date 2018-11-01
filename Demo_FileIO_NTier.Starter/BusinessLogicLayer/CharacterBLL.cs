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
        
        /// <summary>
        /// Returns IEnumerable of character if the character count is over zero.
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public IEnumerable<Character> GetCharacters(out bool success, out string message)
        {
            _character = null;
            success = false;
            message = "";

            try
            {
                //_dataService = new CsvDataService();
                //_dataService = new XmlDataService(DataSettings.dataFilePath);
                _character = _dataService.ReadAll() as List<Character>;
                _character.OrderBy(c => c.Id);

                if (_character.Count > 0)
                {
                    success = true;
                }
                else
                {
                    message = "No data. Is the file empty?";
                }
            }
            catch (FileNotFoundException)
            {
                message = "The file is not found. Does the file exist?";
            }
            // added additional catch statement in case the cast does not work as intended
            catch (InvalidCastException)
            {
                message = "Operation cannot be casted.";
            }
            catch (Exception e)
            {
                message = e.Message;
            }

            return _character;
        }
    }
}