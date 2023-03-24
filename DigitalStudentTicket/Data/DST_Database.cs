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


        #region Создание (Create)
        public Task<int> AddUser (Users user) //создание юзера
        {
            if (_database.Table<Users>().Where(i => i.Id == user.Id).FirstOrDefaultAsync() == null)
                return _database.InsertAsync(user);

            else return null;
            
        }

        public Task<int> AddUserRole (Roles role) //создание роли
        {
            if (_database.Table<Roles>().Where(i => i.Id == role.Id).FirstOrDefaultAsync() == null)
                return _database.InsertAsync(role);

            else return null;
            
        }
        public Task<int> AddStudent(Students student) //создание студента
        {
            if (_database.Table<Students>().Where(i => i.ID == student.ID).FirstOrDefaultAsync() == null)
                return _database.InsertAsync(student);

            else return null;

        }





        #endregion

        #region Чтение (Read)
        public Task<List<Users>> GetAllUsers () //получение списка всех юзеров
        {
            return _database.Table<Users>().ToListAsync();
        }
        public Task<Users> GetUser (Users user) //получение юзера по айди
        {
            return _database.Table<Users>().Where(i=> i.Id == user.Id).FirstOrDefaultAsync();
        }


        #endregion

        #region Обновление (Update)



        #endregion


        #region Удаление (Delete)

        #endregion
    }
}
