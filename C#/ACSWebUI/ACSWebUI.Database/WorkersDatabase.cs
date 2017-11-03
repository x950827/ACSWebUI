using System.Data.Entity;
using ACSWebUI.Common.Entity;

namespace ACSWebUI.Database {
    public class WorkersDatabase : DbContext {
        public WorkersDatabase() : base("Workers") {
            System.Data.Entity.Database.SetInitializer(new DropCreateDatabaseIfModelChanges<WorkersDatabase>());
        }

        public DbSet<WorkerEntity> Workers { get; set; }
    }
}