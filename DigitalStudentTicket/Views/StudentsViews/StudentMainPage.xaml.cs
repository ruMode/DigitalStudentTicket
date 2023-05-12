using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigitalStudentTicket.Views.StudentsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentMainPage : TabbedPage
    {
        public StudentMainPage()
        {
            InitializeComponent();        
        }


        private void logoutBtn_Clicked(object sender, EventArgs e)
        {
            Preferences.Clear();
            Shell.Current.GoToAsync("///loginPage");
        }
    }
}