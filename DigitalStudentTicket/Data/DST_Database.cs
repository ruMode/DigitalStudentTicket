using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DigitalStudentTicket.Entities;
using SQLite;



namespace DigitalStudentTicket.Data
{
    public class DST_Database
    {
        readonly SQLiteAsyncConnection _database;

        public DST_Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);

            _database.CreateTablesAsync<Users, Roles>().Wait();
            _database.CreateTablesAsync<Students, Groups>().Wait();
            _database.CreateTablesAsync<Subjects, Teachers>().Wait();
           

        }

        public bool VerifyUser(string login, string password)
        {
            if (_database.Table<Users>().Where(i => i.Login == login & i.Password == password).FirstOrDefaultAsync() != null) return true;
            else return false;
        }

        public Task<int> AddUser (Users user)
        {
            if (_database.Table<Users>().Where(i => i.Id == user.Id).FirstOrDefaultAsync() == null)
                return _database.InsertAsync(user);

            else return null;
            
        }
        public Task<List<Users>> GetAllUsers () 
        {
            return _database.Table<Users>().ToListAsync();
        }
    }
}
