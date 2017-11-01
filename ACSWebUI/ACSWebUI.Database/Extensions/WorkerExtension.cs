using System.Collections.Generic;
using System.Linq;
using ACSWebUI.Common.Entity;
using ACSWebUI.Common.Model;

namespace ACSWebUI.Database.Extensions {
    public static class WorkerExtension {
        public static Worker[] FromTables(this IEnumerable<WorkerEntity> workers) {
            return workers.Select(FromTable).ToArray();
        }

        public static Worker FromTable(this WorkerEntity worker) {
            if (worker == null)
                return null;

            return new Worker {
                active = worker.Active,
                temporary = worker.Temporary,
                id_worker = worker.IdWorker,
                fio = worker.FIO,
                work_position = worker.WorkPosition,
                date_end = worker.DateEnd,
                key_code = worker.KeyCode,
                foto = worker.Photo
            };
        }

        public static WorkerEntity[] ToTables(this IEnumerable<Worker> workers) {
            return workers.Select(ToTable).ToArray();
        }

        public static WorkerEntity ToTable(this Worker worker) {
            if (worker == null)
                return new WorkerEntity();

            return new WorkerEntity {
                Active = worker.active,
                Temporary = worker.temporary,
                IdWorker = worker.id_worker,
                FIO = worker.fio,
                WorkPosition = worker.work_position,
                DateEnd = worker.date_end,
                KeyCode = worker.key_code,
                Photo = worker.foto
            };
        }
    }
}