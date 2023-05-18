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
            //for (int i = scansCount; i < scansCount+1; i++)
            //{
            //    var indexStudent = MainPage._lessonData.Code_student_J.IndexOf(Items[i].Text);
            //    var studentName = MainPage._lessonData.studentSave.GetRange(indexStudent, 1);
            //    var stdCode = Items[i].Text;
            //    if(!_Items.Contains(Items.FirstOrDefault(x=> x.Code==stdCode))) {
            //        _Items.Add(new ScanResultStudents() { Text = studentName.First(), Code = Items[i].Text, ScanDate = DateTime.Now });
            //        MainPage._lessonData.pris[indexStudent] = "да";
            //    }
            //    else continue;
            //}
           
            MyListView.ItemsSource = Items;
        }
        protected override void OnDisappearing()
        {
            Navigation.RemovePage(this);
            base.OnDisappearing();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private async void scanNextBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(MainPage.scannerPage);
            //await Navigation.PopAsync();
            
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
            RestResponse response = client.Execute(request);

            await DisplayAlert("Подтверждение", "Результаты успешно отправлены!", "Назад");
            await Navigation.PopAsync();
            Items.Clear(); 
        }
    }
}
