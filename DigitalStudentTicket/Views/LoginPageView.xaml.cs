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
            if(Preferences.ContainsKey("user_login")&&Preferences.ContainsKey("user_password")) 
            {
                loginEntry.Text= Preferences.Get("user_login", "").ToString();
                passEntry.Text= Preferences.Get("user_password", "").ToString();
                Login();
            }

            mainSL.IsEnabled = true; logInBtn.BackgroundColor = Color.FromHex("#005a97");
        }

        private async void logInBtn_Clicked(object sender, EventArgs e)
        {
            //блокировка вьюшки
            mainSL.IsEnabled = false;
            logInBtn.BackgroundColor = Color.FromHex("#005a97");

            //проверяем поля ввода на пустоту
            if (string.IsNullOrEmpty(loginEntry.Text) || string.IsNullOrEmpty(passEntry.Text))
            { 
                await DisplayAlert("Ошибка авторизации!", "Данные не введены. Попробуйте снова", "Ок"); 
                mainSL.IsEnabled = true;
                logInBtn.BackgroundColor = Color.FromHex("#005a97");
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
                        //копирование данных в нашу
                        //CopyUserFrom1C();

                        //логин
                        Login(); //пока воид, но потом туда надо будет передавать экземпляр класса юзера
                    }
                    else await DisplayAlert("Ошибка авторизации", "Неправильный логин или пароль!", "Ок"); 
                    mainSL.IsEnabled = true; logInBtn.BackgroundColor = Color.FromHex("#005a97");
                }
            }         
        }

        private bool IsUserExistSQL(string login, string pass)
        {
            //проверяем
            
            return false; //пока так, но потом надо будет возвращать экземпляр класса юзера
        } 
        private bool IsUserExist1C(string login, string pass)
        {
            //проверяем
            //{
                // хттп запрос к базе 1с 
                       
            var client = new HttpClient(); 
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://kamtk.ru/BaseKPK/hs/El_zurnal7?login={login}&password={pass}");
            request.Headers.Add("Authorization", "Basic 0KHQsNC50YI6"); //заголовки базовой авторизации

            var response = client.SendAsync(request).Result;

            var respContent = response.EnsureSuccessStatusCode().Content.ReadAsStringAsync().Result; //получаем строку с ответом сервера
            if (respContent != "[]")
            {
                App.Database.AddUser(new Entities.Users { Login = login, Password = pass }); //записываем данные юзера в нашу базу 
                CopyUserFrom1C(respContent); //копируем данные 
                return true; //юзер существуюет, значит можно залогиниться
            }
            else return false; //юзера нет, сообщение об ошибке
            
            //}
           
        }
        private void Login() //надо принимать экземпляр класса юзера
        {
            //здесь нужно в зависимости от роли юзера (препод, студент) перейти на соотвествующую вьюшку и передать туда данные юзера
           
                Preferences.Set("user_login", loginEntry.Text);
                Preferences.Set("user_password", passEntry.Text);

           
            Shell.Current.GoToAsync("///shedulePage"); //логин

        }
        private void CopyUserFrom1C(string data) 
        {
            //копируем сущность из 1с и записываем данные в свою базу
        }

    }
}