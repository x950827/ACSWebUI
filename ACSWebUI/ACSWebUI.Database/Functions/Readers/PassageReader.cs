using System;
using System.Linq;
using ACSWebUI.Common.Functions.Readers;
using ACSWebUI.Common.Model;
using ACSWebUI.Database.Extensions;
using Newtonsoft.Json;

namespace ACSWebUI.Database.Functions.Readers {
    public class PassageReader : IPassageReader {
        private readonly PassageDatabase accessDatabase;
        public PassageReader(PassageDatabase accessDatabase) {
            this.accessDatabase = accessDatabase;
        }

        public Passage Get(int id) {
            return accessDatabase.Passages.FirstOrDefault(p => p.SkipId == id).FromTable();
        }

        public Passage[] GetAll() {
            var passages = accessDatabase.Passages.FromTables();
            accessDatabase.Passages.RemoveRange(accessDatabase.Passages);
            accessDatabase.SaveChanges();
            return passages;
        }

        public string GetAllInJson() {
            var json = JsonConvert.SerializeObject(accessDatabase.Passages.FromTables());
            accessDatabase.Passages.RemoveRange(accessDatabase.Passages);
            accessDatabase.SaveChanges();
            return json;
        }

        public string GetRangeInJsone(int lastIndex) {
            return JsonConvert.SerializeObject(accessDatabase.Passages.Where(p => p.id >= lastIndex).FromTables());
        }

        public int GetLastIndex() {
            //if ()
            try {
                return accessDatabase.Passages.Max(p => p.id);
            }
            catch (InvalidOperationException e) {
                return -1;
            }
        }
    }
}