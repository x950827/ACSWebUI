using System;
using System.Linq;
using ACSWebUI.Common.Functions.Readers;
using ACSWebUI.Common.Model;
using ACSWebUI.Database.Extensions;

namespace ACSWebUI.Database.Functions.Readers {
    public class WorkerReader : IWorkerReader {
        private readonly WorkersDatabase accessDatabase;
        public WorkerReader(WorkersDatabase accessDatabase) {
            this.accessDatabase = accessDatabase;
        }

        public Worker Get(int id) {
            try {
                return accessDatabase.Workers.FirstOrDefault(w => w.IdWorker == id).FromTable();
            }
            catch (Exception) {
                return null;
            }
        }

        public Worker Get(string keyCode) {
            try {
                return accessDatabase.Workers.FirstOrDefault(w => w.KeyCode == keyCode).FromTable();
            }
            catch (Exception e) {
                return null;
            }
        }

        public Worker[] GetAll() {
            return accessDatabase.Workers.FromTables();
        }
    }
}