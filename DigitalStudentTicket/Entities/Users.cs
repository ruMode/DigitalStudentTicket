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
        public string Role { get; set; }

        //public class Teacher
        //{
        //    public string _bool { get; set; }
        //    public string DL { get; set; }
        //    public string Code_teacher { get; set; }
        //    public string Name_teacher { get; set; }
        //    public string Group_info { get; set; }

        //}


        //public class Student
        //{
        //    public string _bool { get; set; }
        //    public string Code_group { get; set; }
        //    public string Name_group { get; set; }
        //    public string Code_Student { get; set; }
        //    public string Name_Student { get; set; }

        //}
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
