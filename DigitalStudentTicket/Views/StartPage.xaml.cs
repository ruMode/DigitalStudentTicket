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
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void GoTo_MobileSite(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("///SitePage"); 
        }

        private void GoTo_Login(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("///loginPage");
        }

        private void GoTo_BiblioLogin(object sender, EventArgs e)
        {
            Shell.Current.DisplayAlert("Приложение КПК", "Данный раздел находится в разработке", "OK");
        }

        private void GoTo_Courses(object sender, EventArgs e)
        {
            Shell.Current.DisplayAlert("Приложение КПК", "Данный раздел находится в разработке", "OK");
        }
    }
}