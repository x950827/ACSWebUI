using System.Data.Entity;
using ACSWebUI.Common.Entity;

namespace ACSWebUI.Database {
    public class PassageDatabase : DbContext {
        public PassageDatabase() : base("Passages") {
            System.Data.Entity.Database.SetInitializer(new DropCreateDatabaseIfModelChanges<PassageDatabase>());
        }

        public DbSet<PassageEntity> Passages { get; set; }
    }
}