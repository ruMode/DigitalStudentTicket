using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalStudentTicket.Entities
{
    public class Subjects
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public int Teacher { get; set; }
    }
}
