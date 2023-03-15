using DigitalStudentTicket.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigitalStudentTicket
{
    public partial class AppShell : Shell
    {
        public  AppShell()
        {
            InitializeComponent();
            this.FlyoutBehavior = FlyoutBehavior.Disabled;
        }
    }
}