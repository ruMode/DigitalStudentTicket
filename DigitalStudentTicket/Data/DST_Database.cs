using Android.Telephony;
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
            _database.CreateTableAsync<SavedLessonData>().Wait();
        }

        public Users VerifyUser(string login, string password)
        {
            //спрашиваем у базы есть ли юзер с такими данными
            Users _user = new Users();
            _user = _database.Table<Users>().Where(i => i.Login == login && i.Password == password).FirstOrDefaultAsync().Result;
            if (_user != null) return _user;
            else return default;
        }



        #region Создание (Create)
        public Task AddUser (Users user) //создание юзера
        {
            if (_database.Table<Users>().Where(i => i.Login == user.Login && i.Password == user.Password).FirstOrDefaultAsync().Result == null)
                return  _database.InsertAsync(user);

            else return null;
            
        }
        public Task<int> AddTeacher (Teachers teacher) //создание препода
        {
            if (_database.Table<Teachers>().Where(i => i.Code_teacher==teacher.Code_teacher).FirstOrDefaultAsync().Result == null)
                return _database.InsertAsync(teacher);

            else return null;
            
        }
        public Task<int> AddStudent (Students student) //создание студента
        {
            if (_database.Table<Students>().Where(i => i.Code_Student== student.Code_Student).FirstOrDefaultAsync().Result == null)
                return _database.InsertAsync(student);

            else return null;
            
        }       
        public Task<int> SaveLessonData (SavedLessonData data) //создание студента
        {
           return _database.InsertAsync(data);
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
        public List<Students> GetAllStudents() //получение списка всех студентов
        {
            return _database.Table<Students>().ToListAsync().Result;
        }
        public Users GetUser (Users user) //получение юзера
        {
            return _database.Table<Users>().Where(i=> i.Id == user.Id).FirstOrDefaultAsync().Result;
        }
        public Teachers GetTeacher (string teacherCode) //получение препода
        {
            return _database.Table<Teachers>().Where(i=> i.Code_teacher == teacherCode).FirstOrDefaultAsync().Result;
        }

        public Students GetStudent (string studentCode) //получение студента
        {
            return _database.Table<Students>().Where(i=> i.Code_Student == studentCode).FirstOrDefaultAsync().Result;
        }
        public SavedLessonData GetSavedData() //получение 
        {
            return _database.Table<SavedLessonData>().FirstOrDefaultAsync().Result;
        }


        #endregion

        #region Обновление (Update)



        #endregion


        #region Удаление (Delete)
        public void DeleteAllUsers()
        {
             _database.DropTableAsync<Users>().Wait();
            Preferences.Clear();
        }

        public void DeleteSavedData()
        {
            _database.DeleteAllAsync<SavedLessonData>();
        }
        #endregion
    }
}
