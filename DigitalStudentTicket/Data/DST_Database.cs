using DigitalStudentTicket.Entities;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace DigitalStudentTicket.Data
{
    public class DST_Database
    {
        readonly SQLiteAsyncConnection _database;

        public DST_Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath); //соединяемся с базой

            _database.CreateTablesAsync<Users, Teachers, Students>().Wait(); //создаем таблицы, если они не были созданы ранее
        }

        public bool VerifyUser(string login, string password)
        {
            //спрашиваем у базы есть ли юзер с такими данными
            if (_database.Table<Users>().Where(i => i.Login == login && i.Password == password).FirstOrDefaultAsync().Result != null) return true;
            else return false;
        }



        #region Создание (Create)
        public Task AddUser (Users user) //создание юзера
        {
            if (_database.Table<Users>().Where(i => i.Login == user.Login && i.Password == user.Password).FirstOrDefaultAsync().Result == null)
                return  _database.InsertAsync(user);

            else return null;
            
        }
        public Task<int> AddTeacher (Teachers teacher) //создание юзера
        {
            if (_database.Table<Teachers>().Where(i => i.Code_teacher==teacher.Code_teacher).FirstOrDefaultAsync().Result == null)
                return _database.InsertAsync(teacher);

            else return null;
            
        }


      


        #endregion

        #region Чтение (Read)
        public List<Users> GetAllUsers () //получение списка всех юзеров
        {
            return _database.Table<Users>().ToListAsync().Result;
        }
        public List<Teachers> GetAllTeachers() //получение списка всех преподов
        {
            return _database.Table<Teachers>().ToListAsync().Result;
        }
        public Users GetUser (Users user) //получение юзера по айди
        {
            return _database.Table<Users>().Where(i=> i.Id == user.Id).FirstOrDefaultAsync().Result;
        }
        public Teachers GetTeacher (string teacherCode) //получение юзера по айди
        {
            return _database.Table<Teachers>().Where(i=> i.Code_teacher == teacherCode).FirstOrDefaultAsync().Result;
        }


        #endregion

        #region Обновление (Update)



        #endregion


        #region Удаление (Delete)
        public  async void DeleteAllUsers()
        {
            await _database.DeleteAllAsync<Users>();
            await _database.DeleteAllAsync<Teachers>();
            Preferences.Clear();
        }
        #endregion
    }
}
