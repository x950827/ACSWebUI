namespace ACSWebUI.Common.Model {
    public class Worker {
        public int _id { get; set; }

        public int id_worker { get; set; }

        public int active { get; set; }

        public int temporary { get; set; }

        public string fio { get; set; }

        public string work_position { get; set; }

        public string date_end { get; set; }

        public string key_code { get; set; }

        public string foto { get; set; }
    }
}