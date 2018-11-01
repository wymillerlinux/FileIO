using Demo_FileIO_NTier.BusinessLogicLayer;
using Demo_FileIO_NTier.DataAccessLayer;
using Demo_FileIO_NTier.PresentationLayer;

namespace Demo_FileIO_NTier
{
    class Program
    {
        static void Main(string[] args)
        {
            //IDataService dataService = new XmlDataService();
            //IDataService dataService = new XmlDataService(DataSettings.dataFilePath);
            IDataService dataService = new JsonDataService(DataSettings.dataFilePath);
            CharacterBLL characterBll = new CharacterBLL(dataService);
            Presenter presenter = new Presenter(characterBll);
        }
    }
}
