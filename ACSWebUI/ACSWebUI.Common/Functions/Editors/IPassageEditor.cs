using System.Collections.Generic;
using ACSWebUI.Common.Model;

namespace ACSWebUI.Common.Functions.Editors {
    public interface IPassageEditor {
        void Add(Passage passage);
        void AddMany(IEnumerable<Passage> passages);
    }
}