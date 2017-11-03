using System.ComponentModel.DataAnnotations;

namespace ACSWebUI.Common.Entity {
    public class PassageEntity {
        [Key]
        public int id { get; set; }

        public int SkipId { get; set; }

        public string Date { get; set; }
    }
}