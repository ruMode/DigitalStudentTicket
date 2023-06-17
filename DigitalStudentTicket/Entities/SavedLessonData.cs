using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalStudentTicket.Entities
{
    public class SavedLessonData
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string ErrorMsg { get; set; }
        public string JSONData { get; set; }
    }
}
