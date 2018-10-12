using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Demo_FileIO_NTier.Models;
using Newtonsoft.Json;
using Newtonsoft;
using System.Xml.Serialization;
using Demo_FileIO_NTier.DataAccessLayer;

namespace Demo_FileIO_NTier.DataAccessLayer
{
    public class JsonDataService : IDataService
    {
        private string _dataFilePath;

        /// <summary>
        /// reads all the things from the json string/data
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Character> ReadAll()
        {
            List<Character> character = new List<Character>();

            try
            {
                using (StreamReader sr = new StreamReader(_dataFilePath))
                {
                    string jsonString = sr.ReadToEnd();

                    Characters characterList = JsonConvert.DeserializeObject<RootObject>(jsonString).Characters;
                    character = characterList.Character;
                }
                      
            }
            catch (Exception e)
            {
                throw;
            }

            return character;
        }


        public void WriteAll(IEnumerable<Character> characters)
        {
            RootObject rootObject = new RootObject();
            rootObject.Characters = new Characters();
            rootObject.Characters.Character = characters as List<Character>;

            string jsonString = JsonConvert.SerializeObject(rootObject);

            try
            {
                StreamWriter writer = new StreamWriter(_dataFilePath);
                using (writer)
                {
                    writer.WriteLine(jsonString);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public JsonDataService()
        {
            _dataFilePath = DataSettings.dataFilePath;
        }

        public JsonDataService(string dataFile)
        {
            _dataFilePath = dataFile;
        }
    }
}