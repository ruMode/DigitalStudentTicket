using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalStudentTicket.Models
{
    public class LessonData //нужен для отправки данных на сервер с отметками о посещаемости на паре 
    {
        public string date { get; set; }
        public int para { get; set; } //номер пары
        public string index_predmet { get; set; } //код по ОГСЭ SheduleItems.code_predmet
        public string name_predmet { get; set; }
        public string theme { get; set; } //можно пустой
        public string teacher_code { get; set; }
        public string code_group { get; set; }
        public string group_name { get; set; }
        public List<string> Rating { get; set; } = new List<string>(); //оценки студентов
        public List<string> studentSave { get; set; } = new List<string>(); //имена студентов
        public List<string> Code_student_J { get; set; } = new List<string>(); //коды студентов
        public List<string> pris { get; set; } = new List<string>(); //отметки о присутствии (да/нет), по-умолчанию "да" 
        public List<string> coment { get; set; } = new List<string>();


    }
    
}
