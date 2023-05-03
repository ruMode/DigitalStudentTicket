using DigitalStudentTicket.Entities;
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
        ObservableCollection<SheduleItems> _shedule = new ObservableCollection<SheduleItems>();
        private bool _isSheduleExist { get; set; } 
        public MainPage()
        {
            InitializeComponent();
           _isSheduleExist = false;
        }
        protected override void OnAppearing()
        {
            currentDateLabel.Text = currentDate;
            
            if (!_isSheduleExist) { sheduleCV.ItemsSource = GetShedule(TeacherCode).Result; _isSheduleExist = true; }
            else return;
            
        }
        private async void sheduleCV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (DateTime.Now.Hour >=10 && DateTime.Now.Minute >= 5 && (e.CurrentSelection[0] as SheduleItems).para=="1")
            {
                await DisplayAlert("Ошибка!", "Пара уже закончилась. \n Отметки посещаемости недоступны", "Ок");  
            }
            else if(DateTime.Now.Hour >=11 && DateTime.Now.Minute >= 50 && (e.CurrentSelection[1] as SheduleItems).para == "2")
            {
                await DisplayAlert("Ошибка!", "Пара уже закончилась. \n Отметки посещаемости недоступны", "Ок");
            }
            else if (DateTime.Now.Hour >= 13 && DateTime.Now.Minute >= 55 && (e.CurrentSelection[2] as SheduleItems).para == "3")
            {
                await DisplayAlert("Ошибка!", "Пара уже закончилась. \n Отметки посещаемости недоступны", "Ок");
            }
            else if(DateTime.Now.Hour >= 15 && DateTime.Now.Minute >= 40 && (e.CurrentSelection[3] as SheduleItems).para == "4")
            {
                await DisplayAlert("Ошибка!", "Пара уже закончилась. \n Отметки посещаемости недоступны", "Ок");
            }
            else if(DateTime.Now.Hour >= 16 && DateTime.Now.Minute >= 50 && (e.CurrentSelection[4] as SheduleItems).para == "5")
            {
                await DisplayAlert("Ошибка!", "Пара уже закончилась. \n Отметки посещаемости недоступны", "Ок");
            }
            else await Navigation.PushAsync(scannerPage); 

            
        }

        private void logoutBtn_Clicked(object sender, EventArgs e)
        {
            Preferences.Clear();
            Shell.Current.GoToAsync("///loginPage");
        }


        private async Task<ObservableCollection<SheduleItems>> GetShedule(string teacherCode)
        {
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(5);
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://kamtk.ru/BaseKPK/hs/El_zurnal11?code_teacher={teacherCode}&date_lessons=2");
            request.Headers.Add("Authorization", "Basic 0KHQsNC50YI6"); //заголовки базовой авторизации

            var response = client.SendAsync(request).Result;

            var respContent = response.EnsureSuccessStatusCode().Content.ReadAsStringAsync().Result;
            try
            {
                //конвертируем полученный JSON в объекты класса SheduleItems (непосредственно само расписание)
                var shedule = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SheduleItems>>(respContent);
                _shedule.Clear();
                foreach (var item in shedule)
                {
                    _shedule.Add(item);
                   
                }

                return _shedule;

            }
            catch (Exception e)
            {
                await DisplayAlert(Title, e.Message, "ok");    
                throw;
            }
        }


        private async void UpdateSheduleBnt_Clicked(object sender, EventArgs e)
        {
            if ( await GetShedule(TeacherCode) != null)
            {
                await DisplayAlert("Обновление расписания", "Расписание успешно обновлено!", "Ок");
            }
            else await DisplayAlert("Обновление расписания", "Не удалось обновить расписание :( \n Повторите попытку...", "Ок");

        }























        private void admin_Clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("///dbTables");
        }

    }
}
