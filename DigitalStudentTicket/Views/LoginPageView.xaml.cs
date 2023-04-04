using DigitalStudentTicket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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
            //автологин при перезапуске приложения
            if(Preferences.ContainsKey("user_login")&&Preferences.ContainsKey("user_password")) //ищем в найстроках ключи логина и пароля
            {
                loginEntry.Text = Preferences.Get("user_login", "").ToString(); //вставляем логин
                passEntry.Text= Preferences.Get("user_password", "").ToString(); //пароль
                MainPage.TeacherCode = Preferences.Get("user_code", "").ToString(); //код препода
                Login();
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
                //проверка юзера в нашей базе
                if (App.Database.VerifyUser(loginEntry.Text, passEntry.Text))
                {
                    Login(); //пока воид, но потом туда надо будет передавать роль юзера
                }
                else
                {
                    //проверка в 1с
                    if (IsUserExist1C(loginEntry.Text, passEntry.Text))
                    {
                        //логин
                        Login(); //пока воид, но потом туда надо будет передавать экземпляр класса юзера
                    }
                    else await DisplayAlert("Ошибка авторизации", "Неправильный логин или пароль!", "Ок"); 
                    mainSL.IsEnabled = true; logInBtn.BackgroundColor = Color.FromHex("#005a97");
                }
            }         
        }

        private bool IsUserExist1C(string login, string pass)
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
            if (respContent != "[]")
            {
               var jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Teachers>>(respContent);
                
                App.Database.AddUser(new Users
                {
                    Login = login,
                    Password = pass,
                    Role = jsonObject.First().Code_teacher,
                }) ; 
                //MainPage.TeacherCode = jsonObject.First().Code_teacher;
                CopyUserFrom1C(respContent); //копируем данные 
                return true; //юзер существует, значит можно залогиниться
            }
            else return false; //юзера нет, сообщение об ошибке
            
            //}
           
        }
        private void Login() //надо принимать экземпляр класса юзера
        {
            //здесь нужно в зависимости от роли юзера (препод, студент) перейти на соотвествующую вьюшку и передать туда данные юзера

            Preferences.Set("user_login", loginEntry.Text);
            Preferences.Set("user_password", passEntry.Text);
            Preferences.Set("user_code", MainPage.TeacherCode);
            Shell.Current.GoToAsync("///shedulePage"); //логин

        }
        private void CopyUserFrom1C(string data) 
        {
            //копируем сущность из 1с и записываем данные в свою базу
            var userData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Teachers>>(data);
            MainPage.TeacherCode = userData.First().Code_teacher;
            App.Database.AddTeacher(userData.First());
            
        }

    }
}