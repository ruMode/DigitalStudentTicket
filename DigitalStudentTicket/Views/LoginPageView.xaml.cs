using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
        }

        private async void logInBtn_Clicked(object sender, EventArgs e)
        {
            //блокировка вьюшки
            mainSL.IsEnabled = false;


            //проверка юзера в нашей базе
            if (string.IsNullOrEmpty(loginEntry.Text) & string.IsNullOrEmpty(passEntry.Text)) await DisplayAlert("Ошибка авторизации!", "Данные не введены. Попробуйте снова", "Ок");
            else
            {


                if (App.Database.VerifyUser(loginEntry.Text.Trim(), passEntry.Text.Trim()))
                {
                    Login(); //пока воид, но потом туда надо будет передавать экземпляр класса юзера

                }
                else
                {
                    //проверка в 1с
                    if (IsUserExist1C(loginEntry.Text.Trim(), passEntry.Text.Trim()))
                    {
                        //копирование данных в нашу
                        CopyUserFrom1C();

                        //логин
                        Login(); //пока воид, но потом туда надо будет передавать экземпляр класса юзера
                    }
                    else DisplayAlert("Ошибка авторизации", "Неправильный логин или пароль!", "Ок"); mainSL.IsEnabled = true;
                }
            }         
        }

        private bool IsUserExistSQL(string login, string pass)
        {
            //проверяем
            
            return true; //пока так, но потом надо будет возвращать экземпляр класса юзера
        } 
        private bool IsUserExist1C(string login, string pass)
        {
            //проверяем
            //{
            // хттп запрос к базе 1с и возврат полной сущности чтобы записать в свою базу
            //}
            return true; //пока так, но потом надо будет возвращать экземпляр класса юзера
        }
        private void Login() //надо принимать экземпляр класса юзера
        {
            //здесь нужно в зависимости от роли юзера (препод, студент) перейти на соотвествующую вьюшку и передать туда данные юзера

            Shell.Current.GoToAsync("///shedulePage"); //логин

        }
        private void CopyUserFrom1C() 
        {
            //копируем сущность из 1с и записываем данные в свою базу
        }

    }
}