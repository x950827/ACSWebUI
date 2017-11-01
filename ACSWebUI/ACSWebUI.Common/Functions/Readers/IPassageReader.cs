using ACSWebUI.Common.Model;

namespace ACSWebUI.Common.Functions.Readers {
    public interface IPassageReader {
        Passage Get(int id);
        Passage[] GetAll();
        string GetAllInJson();
        string GetRangeInJsone(int lastIndex);
        int GetLastIndex();
    }
}