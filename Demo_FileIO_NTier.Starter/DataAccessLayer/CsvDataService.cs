using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Demo_FileIO_NTier.Models;

namespace Demo_FileIO_NTier.DataAccessLayer
{
    public class CsvDataService : IDataService
    {
        private string _dataFilePath;

        public CsvDataService()
        {
            _dataFilePath = DataSettings.dataFilePath;
        }
        
        /// <summary>
        /// Read all the items for a CSV file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Character> ReadAll()
        {
            List<string> characterStrings = new List<string>();
            List<Character> character = new List<Character>();

            try
            {
                StreamReader sr = new StreamReader(_dataFilePath);

                using (sr)
                {
                    while (!sr.EndOfStream)
                    {
                        characterStrings.Add(sr.ReadLine());
                    }

                    foreach (string ch in characterStrings)
                    {
                        character.Add(CharacterObjectBuilder(ch));
                    }
                }        
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return character;

        }


        public void WriteAll(IEnumerable<Character> character)
        {
            
            //List<string> characterString = new List<string>();
            //List<Character> character = new List<Character>();

            try
            {
                StreamWriter sw = new StreamWriter(_dataFilePath);
                
                using (sw)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Clear();
                    
                    foreach (var ch in character)
                    {
                        sb.AppendLine(CharacterStringBuilder(ch));
                    }
                    
                    sw.Write(sb.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Helper class to build an Character object
        /// </summary>
        /// <param name="characterString"></param>
        /// <returns></returns>
        private Character CharacterObjectBuilder(string characterString)
        {
            const char DELINEATOR = ',';
            string[] properties = characterString.Split(DELINEATOR);

            Character character = new Character
            {
                Id = Convert.ToInt32(properties[0]),
                LastName = properties[1],
                FirstName = properties[2],
                Address = properties[3],
                City = properties[4],
                State = properties[5],
                Zip = properties[6],
                Age = Convert.ToInt32(properties[7]),
                Gender = (Character.GenderType)Enum.Parse(typeof(Character.GenderType), properties[8])
            };

            return character;
        }

        private string CharacterStringBuilder(Character characterObject)
        {
            const char DELINEATOR = ',';
            string characterString;

            characterString =
                characterObject.Id + DELINEATOR +
                characterObject.LastName + DELINEATOR +
                characterObject.FirstName + DELINEATOR +
                characterObject.Address + DELINEATOR +
                characterObject.City + DELINEATOR +
                characterObject.State + DELINEATOR +
                characterObject.Zip + DELINEATOR +
                characterObject.Age + DELINEATOR +
                characterObject.Gender;
            
            return characterString;
        }

    }
}