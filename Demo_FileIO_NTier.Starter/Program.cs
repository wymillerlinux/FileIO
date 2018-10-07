using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo_FileIO_NTier.BusinessLogicLayer;
using Demo_FileIO_NTier.DataAccessLayer;
using Demo_FileIO_NTier.PresentationLayer;

namespace Demo_FileIO_NTier
{
    class Program
    {
        static void Main(string[] args)
        {
            IDataService dataService = new CsvDataService();
            //IDataService dataService = new XmlDataService();
            CharacterBLL characterBll = new CharacterBLL(dataService);
            Presenter presenter = new Presenter(characterBll);
        }
    }
}
