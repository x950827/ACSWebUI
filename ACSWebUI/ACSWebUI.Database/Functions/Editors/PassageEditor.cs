using System.Collections.Generic;
using ACSWebUI.Common.Functions.Editors;
using ACSWebUI.Common.Model;
using ACSWebUI.Database.Extensions;

namespace ACSWebUI.Database.Functions.Editors {
    public class PassageEditor : IPassageEditor {
        private readonly PassageDatabase accessDatabase;
        public PassageEditor(PassageDatabase accessDatabase) {
            this.accessDatabase = accessDatabase;
        }

        public void Add(Passage passage) {
            accessDatabase.Passages.Add(passage.ToTable());
            accessDatabase.SaveChanges();
        }

        public void AddMany(IEnumerable<Passage> passages) {
            accessDatabase.Passages.AddRange(passages.ToTables());
            accessDatabase.SaveChanges();
        }
    }
}