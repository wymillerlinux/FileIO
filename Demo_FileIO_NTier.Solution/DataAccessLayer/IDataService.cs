using System.Collections.Generic;
using Demo_FileIO_NTier.Models;

namespace Demo_FileIO_NTier.DataAccessLayer
{
    public interface IDataService
    {
        IEnumerable<Character> ReadAll();
        void WriteAll(IEnumerable<Character> characters);
    }
}
