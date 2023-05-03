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
        //public string Role { get; set; }
       // public static Users CurrentUser { get; set; }
        public LoginPageView()
        {
            InitializeComponent();

        }
        protected override void OnAppearing()
        {
            //автологин при перезапуске приложения
            if(Preferences.ContainsKey("user_login")&&Preferences.ContainsKey("user_password")) //ищем в найстроках ключи логина и пароля
            {
                loginEntry.Text = Preferences.Get("user_login", "").ToString(); //вставляем логин
                passEntry.Text= Preferences.Get("user_password", "").ToString(); //пароль
                Login(Preferences.Get("user_role", "")); //код препода
               
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
                Users _user = App.Database.VerifyUser(loginEntry.Text, passEntry.Text);
                //проверка юзера в нашей базе
                if (_user != null)
                {
                    //App.Database.GetUser(_user);
                    //Role = _user.Role;
                   // CurrentUser=_user;
                    Login(_user.Role); //пока воид, но потом туда надо будет передавать роль юзера
                }
                else
                {
                    Users user1c = CopyUserFrom1C(IsUserExist1C(loginEntry.Text, passEntry.Text));
                    //проверка в 1с
                    if ( user1c != null)
                    {
                        //логин
                        //Role = user1c.Role;
                        //CurrentUser = user1c;
                        Login(user1c.Role); //пока воид, но потом туда надо будет передавать экземпляр класса юзера
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
            
                       
                //CopyUserFrom1C(respContent); //копируем данные 
            return respContent; //юзер существует, значит можно залогиниться
            
             //юзера нет, сообщение об ошибке
            
            //}
           
        }
        private void Login(string role) //надо принимать экземпляр класса юзера
        {
            //здесь нужно в зависимости от роли юзера (препод, студент) перейти на соотвествующую вьюшку и передать туда данные юзера
            Users _user = App.Database.VerifyUser(loginEntry.Text, passEntry.Text);
            //CurrentUser = _user;
            Preferences.Set("user_login", _user.Login);
            Preferences.Set("user_password", _user.Password);
            Preferences.Set("user_role", _user.Role);

            if (role == "Teacher")
            {
                MainPage.TeacherCode = _user.Code;
                //Preferences.Set("user_code", MainPage.TeacherCode);
                Shell.Current.GoToAsync("///shedulePage"); //логин
            }
            else if (role == "Student")
            {
                Shell.Current.GoToAsync("///studentsMainPage");//логин 
            }
            else DisplayAlert("Error", "Unexpected error occurred!", "Ok").Wait();
            //Shell.Current.GoToAsync("///shedulePage"); //логин
          
        }
        private Users CopyUserFrom1C(string respContent) 
        {
            Users user = new Users();
            string code = "";
            string role = "";
            if (respContent != "[]")
            {
                if (respContent.Contains("true"))
                {
                    var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Teachers>>(respContent);
                    code = jsonObject.First().Code_teacher;
                    role = "Teacher";
                    MainPage.TeacherCode = code;
                }
                else
                {
                    var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Students>>(respContent);
                    code = jsonObject.First().Code_Student;
                    role = "Student";
                }

                user.Login = loginEntry.Text;
                user.Password = passEntry.Text;
                user.Code = code;
                user.Role = role;
                App.Database.AddUser(user);
                //CurrentUser = user;
                return user;
            }
            else return null;
          
        }

    }
}