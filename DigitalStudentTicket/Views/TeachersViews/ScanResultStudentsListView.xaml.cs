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
            //foreach (var item in Items)
            //{
            //    MainPage._lessonData.pris[MainPage._lessonData.Code_student_J.IndexOf(item.Code)] = "да";
            //}

            for (int i = 0; i < Items.Count; i++)
            {
                MainPage._lessonData.Code_student_J.Add(Items[i].Code); 
                MainPage._lessonData.studentSave.Add(Items[i].Text); 
                MainPage._lessonData.coment.Add("");
                MainPage._lessonData.Rating.Add("");
                MainPage._lessonData.pris.Add("да");
            }

            var rqstContent = Newtonsoft.Json.JsonConvert.SerializeObject(MainPage._lessonData);

            var client = new RestClient("http://192.168.5.68:80/kamtk/hs/SetDataLesson");
            var request = new RestRequest() { Method = Method.Post };
            request.AddHeader("Authorization", "Basic 0JDQtNC80LjQvTo=");
            request.AddHeader("Content-Type", "text/plain");

            request.AddParameter("application/json", rqstContent, ParameterType.RequestBody);
            //0KHQsNC50YI6MQ ==
            //0JDQtNC80LjQvTo=
            
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
                    SavedLessonData data = new SavedLessonData() { ErrorMsg = response.ErrorMessage, JSONData = rqstContent };
                    await App.Database.SaveLessonData(data);
                    await DisplayAlert("Error", response.ErrorMessage.ToString(), "ok");
                    await Navigation.PopAsync();

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "ok");
                await Navigation.PopAsync();
                throw;
            }
            

            Items.Clear();

        }
    }
}
