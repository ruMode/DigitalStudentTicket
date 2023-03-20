using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalStudentTicket.Entities
{
    public class Teachers
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Name { get; set; }
        public string Patronymic{ get; set; }

    }
}
