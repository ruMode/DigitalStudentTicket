using DigitalStudentTicket.Models;
using DigitalStudentTicket.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile;
using ZXing.Net.Mobile.Forms;

namespace DigitalStudentTicket
{
    public partial class MainPage : ContentPage
    {
        public static ScannerPageView scannerPage = new ScannerPageView();
        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            currentDateLabel.Text = "Сегодня: "+ DateTime.Now.ToShortDateString() +"г. (" + DateTime.Now.DayOfWeek.ToString() +")";

            int techerCode = 000000050;
            sheduleCV.ItemsSource = GetShedule(techerCode);
           
        }
        private async void sheduleCV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Navigation.PushAsync(scannerPage);
            
        }

        private void logoutBtn_Clicked(object sender, EventArgs e)
        {
            Preferences.Clear();
            Shell.Current.GoToAsync("///loginPage");
        }

        private void admin_Clicked(object sender, EventArgs e)
        {
            //App.Database.DeleteAllUsers();
            Shell.Current.GoToAsync("///dbTables");
        }

        private List<SheduleItems.SheduleItem> GetShedule(int teacherCode)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://kamtk.ru/BaseKPK/hs/El_zurnal11?code_teacher={teacherCode}&date_lessons=2");
            request.Headers.Add("Authorization", "Basic 0KHQsNC50YI6"); //заголовки базовой авторизации
            var response = client.SendAsync(request).Result;
            var respContent = response.EnsureSuccessStatusCode().Content.ReadAsStringAsync().Result; //получаем строку с ответом сервера
            
            try
            {
                var shedule = Newtonsoft.Json.JsonConvert.DeserializeObject<SheduleItems>(respContent);
                return shedule.Data.ToList();

            }
            catch (Exception e)
            {
                DisplayAlert("", e.Message, "ok");
                throw;
            }

        }
    }
}
