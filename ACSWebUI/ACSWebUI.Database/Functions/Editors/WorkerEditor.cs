using System.Linq;
using ACSWebUI.Common.Functions.Editors;
using ACSWebUI.Common.Model;
using ACSWebUI.Database.Extensions;

namespace ACSWebUI.Database.Functions.Editors {
    public class WorkerEditor : IWorkerEditor {
        private readonly WorkersDatabase accessDatabase;
        public WorkerEditor(WorkersDatabase accessDatabase) {
            this.accessDatabase = accessDatabase;
        }

        public void Add(Worker worker) {
            accessDatabase.Workers.Add(worker.ToTable());
            accessDatabase.SaveChanges();
        }

        public void AddMany(Worker[] workers) {
            foreach (var worker in workers) {
                var tempWorker = accessDatabase
                    .Workers
                    .FirstOrDefault(w => w.IdWorker == worker.id_worker);

                if (tempWorker == null) {
                    accessDatabase.Workers.Add(worker.ToTable());
                    accessDatabase.SaveChanges();
                    continue;
                }
                if (tempWorker.IdWorker == worker.id_worker) {
                    if (tempWorker.Active == worker.active &&
                        tempWorker.DateEnd == worker.date_end &&
                        tempWorker.FIO == worker.fio &&
                        tempWorker.KeyCode == worker.key_code &&
                        tempWorker.Temporary == worker.temporary &&
                        tempWorker.Photo == worker.foto &&
                        tempWorker.WorkPosition == worker.work_position)
                        continue;

                    if (tempWorker.Active != worker.active)
                        tempWorker.Active = worker.active;
                    if (tempWorker.DateEnd != worker.date_end)
                        tempWorker.DateEnd = worker.date_end;
                    if (tempWorker.FIO != worker.fio)
                        tempWorker.FIO = worker.fio;
                    if (tempWorker.KeyCode != worker.key_code)
                        tempWorker.KeyCode = worker.key_code;
                    if (tempWorker.Photo != worker.foto)
                        tempWorker.Photo = worker.foto;
                    if (tempWorker.Temporary != worker.temporary)
                        tempWorker.Temporary = worker.temporary;
                    if (tempWorker.WorkPosition == worker.work_position)
                        tempWorker.WorkPosition = worker.work_position;

                    accessDatabase.SaveChanges();
                }
            }
            accessDatabase.SaveChanges();
        }

        public void Change(Worker worker) {
            var tempWorker = accessDatabase.Workers.FirstOrDefault(w => w.IdWorker == worker.id_worker);
            if (tempWorker != null) {
                tempWorker.Active = worker.active;
                tempWorker.Temporary = worker.temporary;
                if (!string.IsNullOrEmpty(worker.date_end))
                    tempWorker.DateEnd = worker.date_end;
                if (!string.IsNullOrEmpty(worker.fio))
                    tempWorker.FIO = worker.fio;
                if (!string.IsNullOrEmpty(worker.work_position))
                    tempWorker.WorkPosition = worker.work_position;
                accessDatabase.SaveChanges();
            }
        }

        public void Delete(int id) {
            accessDatabase.Workers.Remove(accessDatabase.Workers.FirstOrDefault(w => w.IdWorker == id));
        }
    }
}