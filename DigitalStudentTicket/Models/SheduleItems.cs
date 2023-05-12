using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalStudentTicket.Models
{
    public class SheduleItems
    {
        public string para { get; set; } //+
        public string name_predmet { get; set; }//+
        public string code_predmet { get; set; } //index_predmet
        public string group_name { get; set; }//+
        public string group_code { get; set; }//+
        public string teacher_code { get; set; }//+
        public int number_Lesson { get; set; }
        public string date { get; set; }//+

        
    }
}
