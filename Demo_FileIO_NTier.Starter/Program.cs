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
            //IDataService dataService = new XmlDataService(DataSettings.dataFilePath);
            //IDataService dataService = new JsonDataService();
            CharacterBLL characterBll = new CharacterBLL(dataService);
            Presenter presenter = new Presenter(characterBll);
        }
    }
}
