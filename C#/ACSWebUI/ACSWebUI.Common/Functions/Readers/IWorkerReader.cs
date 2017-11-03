using ACSWebUI.Common.Model;

namespace ACSWebUI.Common.Functions.Readers {
    public interface IWorkerReader {
        Worker Get(int id);
        Worker[] GetAll();
        Worker Get(string keyCode);
    }
}