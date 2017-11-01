using System.ComponentModel.DataAnnotations;

namespace ACSWebUI.Common.Entity {
    public class WorkerEntity {
        [Key]
        public int Id { get; set; }

        public int IdWorker { get; set; }

        public int Active { get; set; }

        public int Temporary { get; set; }

        public string FIO { get; set; }

        public string WorkPosition { get; set; }

        public string DateEnd { get; set; }

        public string KeyCode { get; set; }

        public string Photo { get; set; }
    }
}