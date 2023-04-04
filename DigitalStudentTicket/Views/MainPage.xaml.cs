using DigitalStudentTicket.Models;
using DigitalStudentTicket.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
        public string currentDate { get; set; } = $"Сегодня: {DateTime.Now.ToShortDateString()}г. ({DateTime.Now.DayOfWeek})";
        public static string TeacherCode { get; set; }
        public MainPage()
        {
            InitializeComponent();
           
        }
        protected override void OnAppearing()
        {
            currentDateLabel.Text = currentDate;

            sheduleCV.ItemsSource = GetShedule(TeacherCode).Result;
           
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


        private async Task<List<SheduleItems>> GetShedule(string teacherCode)
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(5);
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://kamtk.ru/BaseKPK/hs/El_zurnal11?code_teacher={teacherCode}&date_lessons=2");
            request.Headers.Add("Authorization", "Basic 0KHQsNC50YI6"); //заголовки базовой авторизации

            var response = client.SendAsync(request).Result;

            var respContent = response.EnsureSuccessStatusCode().Content.ReadAsStringAsync().Result;
            try
            {
                //конвертируем полученный JSON в объекты класса SheduleItems.SheduleItem (непосредственно само расписание)
                var shedule = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SheduleItems>>(respContent);
                return shedule;

            }
            catch (Exception e)
            {
                await DisplayAlert(Title, e.Message, "ok");    
                throw;
            }
                  
            


        }

























        private void admin_Clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("///dbTables");
        }

    }
}
