using DigitalStudentTicket.Entities;
using DigitalStudentTicket.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigitalStudentTicket.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanResultStudentsListView : ContentPage
    {
        public static ObservableCollection<ScanResultStudents> Items = new ObservableCollection<ScanResultStudents>();
        public static int scansCount { get; set; } = -1;
        public ScanResultStudentsListView()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            MyListView.ItemsSource = Items;
        }
        protected override void OnDisappearing()
        {
            Navigation.RemovePage(this);
            base.OnDisappearing();
        }

        private async void scanNextBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(MainPage.scannerPage);
        }

        private async void aproveResultsBtn_Clicked(object sender, EventArgs e)
        {
            //заполняем присутствие
            foreach (var item in Items)
            {
                MainPage._lessonData.pris[MainPage._lessonData.Code_student_J.IndexOf(item.Code)] = "да";
            }

            var rqstContent = Newtonsoft.Json.JsonConvert.SerializeObject(MainPage._lessonData);

            var client = new RestClient("http://localhost/kamtk/hs/SetDataLesson");
            var request = new RestRequest() { Method = Method.Post };
            request.AddHeader("Authorization", "Basic 0KHQsNC50YI6");
            request.AddHeader("Content-Type", "text/plain");

            request.AddParameter("application/json", rqstContent, ParameterType.RequestBody);

            try
            {
                RestResponse response = client.Execute(request);
                if (response.IsSuccessful)
                {
                    await DisplayAlert("Подтверждение", "Результаты успешно отправлены!", "Назад");
                    await Navigation.PopAsync();
                }
                else
                {
                    SavedLessonData data = new SavedLessonData() { ErrorMsg = response.ErrorMessage, JSONData = MainPage._lessonData.ToString() };
                    await App.Database.SaveLessonData(data);
                    await DisplayAlert("Error", response.ErrorException.ToString(), "ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "ok");
                throw;
            }
            

            Items.Clear();

        }
    }
}
