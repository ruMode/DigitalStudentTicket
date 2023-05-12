using DigitalStudentTicket.Models;
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
        ObservableCollection<ScanResultStudents> _Items = new ObservableCollection<ScanResultStudents>();

        public ScanResultStudentsListView()
        {
            InitializeComponent();


        }
        protected override void OnAppearing()
        {
            foreach (var item in Items)
            {
                var indexStudent = MainPage._lessonData.Code_student_J.IndexOf(item.Text);
                var studentName = MainPage._lessonData.studentSave.fi
            }
            MyListView.ItemsSource = _Items;

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

            //тут нужно формировать JSON с данными студента и пары или че там в запросе




            await DisplayAlert("Подтверждение", "Результаты успешно отправлены!", "Назад");
            await Navigation.PopAsync();
            Items.Clear();
        }
    }
}
