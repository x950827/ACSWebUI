using ACSWebUI.Common.Model;

namespace ACSWebUI.Common.Functions.Editors {
    public interface IWorkerEditor {
        void Add(Worker worker);
        void AddMany(Worker[] workers);
        void Change(Worker worker);
        void ChangeCode(Worker worker, string code);
        void Delete(int id);
    }
}