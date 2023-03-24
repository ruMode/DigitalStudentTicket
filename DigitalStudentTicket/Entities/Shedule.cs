using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalStudentTicket.Entities
{
    public class Shedule
    {
        [PrimaryKey, AutoIncrement] public int Id { get; set; }
        public List<Subjects> Subjects { get; set; }
        public DateTime Date { get; set; }

    }
}
