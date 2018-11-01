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
            
        }

        public JsonDataService(string datafile)
        {
            _dataFilePath = datafile;
        }
    }
}