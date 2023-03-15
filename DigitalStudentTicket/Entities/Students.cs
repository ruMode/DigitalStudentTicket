using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalStudentTicket.Entities
{
    public class Students
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public int GroupID { get; set; }
        public DateTime Birthday { get; set; }
        public string QRCode { get; set; }
        public int MissesCount { get; set; } //количество пропусков
    }
}
