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
    public partial class WebView : ContentPage
    {
        public WebView()
        {
            InitializeComponent();
        }

        private void Go_Back(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("///StartPage");
        }
    }
}