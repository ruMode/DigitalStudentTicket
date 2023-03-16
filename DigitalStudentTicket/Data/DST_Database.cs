using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DigitalStudentTicket.Entities;
using SQLite;



namespace DigitalStudentTicket.Data
{
    internal class DST_Database
    {
        readonly SQLiteAsyncConnection db;

        public DST_Database(string connectionString)
        {
            db = new SQLiteAsyncConnection(connectionString);

            db.CreateTablesAsync<Users, Roles, Students, Groups>().Wait();
           

        }
    }
}
