using DigitalStudentTicket.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.QrCode.Internal;
using static System.Net.WebRequestMethods;

namespace DigitalStudentTicket.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPageView : ContentPage
    {
        public LoginPageView()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            mainSL.IsEnabled = false; logInBtn.BackgroundColor = Color.Gray; //блокируем вьюшку
            //автологин при перезапуске приложения
            if (Preferences.ContainsKey("user_login")&&Preferences.ContainsKey("user_password")) //ищем в найстроках ключи логина и пароля
            {
                loginEntry.Text = Preferences.Get("user_login", "").ToString(); //вставляем логин
                passEntry.Text= Preferences.Get("user_password", "").ToString(); //пароль
                Login(App.Database.VerifyUser(loginEntry.Text, passEntry.Text)); //логинимся
               
            }

            mainSL.IsEnabled = true; logInBtn.BackgroundColor = Color.FromHex("#005a97"); //разблокируем вьюшку
        }

        private async void logInBtn_Clicked(object sender, EventArgs e)
        {
            //блокировка вьюшки
            mainSL.IsEnabled = false; logInBtn.BackgroundColor = Color.FromHex("#005a97");

            //проверяем поля ввода на пустоту
            if (string.IsNullOrEmpty(loginEntry.Text) || string.IsNullOrEmpty(passEntry.Text))
            { 
                await DisplayAlert("Ошибка авторизации!", "Данные не введены. Попробуйте снова", "Ок"); 
                mainSL.IsEnabled = true; logInBtn.BackgroundColor = Color.FromHex("#005a97");
            }
            else
            {
                Users _user = App.Database.VerifyUser(loginEntry.Text, passEntry.Text); //проверяем юзера в базе и копируем его данные
                if (_user != null)
                {
                    Login(_user); //передаваем  юзера 
                }
                else
                {
                    Users user1c = CopyUserFrom1C(IsUserExist1C(loginEntry.Text, passEntry.Text));
                    //проверка в 1с
                    if ( user1c != null)
                    {
                        //логин
                        Login(user1c); //передаем юзера
                    }
                    else await DisplayAlert("Ошибка авторизации", "Неправильный логин или пароль!", "Ок"); 
                    mainSL.IsEnabled = true; logInBtn.BackgroundColor = Color.FromHex("#005a97");
                }
            }         
        }

        private string IsUserExist1C(string login, string pass)
        {
            //проверяем
            //{
            // хттп запрос к базе 1с 
           
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(5);
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://kamtk.ru/BaseKPK/hs/El_zurnal7?login={login}&password={pass}");
            request.Headers.Add("Authorization", "Basic 0KHQsNC50YI6"); //заголовки базовой авторизации

            var response = client.SendAsync(request).Result;

            var respContent = response.EnsureSuccessStatusCode().Content.ReadAsStringAsync().Result; //получаем строку с ответом сервера
             
            return respContent; //возвращаем полученный ответ от сервера
            
            //}
           
        }

        private Users Login( Users _user) //надо принимать экземпляр класса юзера
        {
            Preferences.Set("user_login", _user.Login); //записываем в настройки приложения для автовхода логин юзера
            Preferences.Set("user_password", _user.Password); //пароль
            
            //проверяем роль юзера
            if (_user.Role == "Teacher") 
            {
                MainPage.TeacherCode = _user.Code; //записываем код препода, для вывода расписания
                Shell.Current.GoToAsync("///shedulePage"); //переходим на страницу с расписанием для препода
            }
            else if (_user.Role == "Student")
            {
                Shell.Current.GoToAsync("///studentsMainPage"); //переходим на страницу для студента
            }
            else DisplayAlert("Error", "Unexpected error occurred!", "Ok").Wait();
           
            return _user; //возвращаю юзера, мб в будущем пригодится
        }

        private Users CopyUserFrom1C(string respContent) //копируем юзера из базы 1с в локальную, для ускорения работы
        {
            Users user = new Users();
            string code = "";
            string role = "";

            //проверяем не пришел ли с сервера пустой json
            if (respContent != "[]")
            {
                //смотрим кто нам пришел - препод или студент
                if (respContent.Contains("true")) 
                {
                    //работаем с преподом
                    var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Teachers>>(respContent);
                    code = jsonObject.First().Code_teacher;
                    role = "Teacher";
                    MainPage.TeacherCode = code;
                    App.Database.AddTeacher(new Teachers
                    {
                        Code_teacher = code,
                        Name_teacher=jsonObject.First().Name_teacher,
                        Group_info = jsonObject.First().Group_info,
                    });
                }
                else
                {
                    //со студентом
                    var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Students>>(respContent);
                    code = jsonObject.First().Code_Student;
                    role = "Student";
                    App.Database.AddStudent(new Students 
                    { 
                        Code_Student = jsonObject.First().Code_Student,   
                        Code_group = jsonObject.First().Code_group,   
                        Name_group = jsonObject.First().Name_group,
                        Name_Student = jsonObject.First().Name_Student
                    });
                }

                //записываем данные юзера в локальную базу 
                user.Login = loginEntry.Text;
                user.Password = passEntry.Text;
                user.Code = code;
                user.Role = role;
                App.Database.AddUser(user);
               
                return user; //возвращаем получившегося юзера
            }
            else return null;
          
        }

    }
}