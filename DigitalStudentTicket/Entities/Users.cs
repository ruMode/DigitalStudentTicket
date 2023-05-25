using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalStudentTicket.Entities
{
    public class Users
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        [Column("Code")]
        [MaxLength(9)]
        public string Code { get; set; } 
        
        public string Role { get; set; }


        public class Student
        {
            public string name_group { get; set; }
            public string code_student { get; set; }
            public string name_student { get; set; }

        }
            //public string DefineRole(object user)
            //{
            //    if (user.GetType() == typeof(Teachers))
            //    {
            //        Teachers _user = new Teachers();
            //        _user = (Teachers)user;
            //        return Role = _user.Code_teacher;
            //    }
            //    else if (user.GetType() == typeof(Student))
            //    {
            //        Students _user = new Students();
            //        _user = (Students)user;
            //        return Role = _user.Code_Student;
            //    }
            //    else return Role = default;

            //}
        }


}
