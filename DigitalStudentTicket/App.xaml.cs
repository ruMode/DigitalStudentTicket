using DigitalStudentTicket.Data;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigitalStudentTicket
{
    public partial class App : Application
    {
        private static DST_Database database;
        public static DST_Database Database
        {
            get
            {
                if (database == null)
                    database = new DST_Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DST_database.db3"));
                return database;
            }
            
        }
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            Database.AddUser(new Entities.Users { Login = "admin", Password = "1" });
            

            //MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
