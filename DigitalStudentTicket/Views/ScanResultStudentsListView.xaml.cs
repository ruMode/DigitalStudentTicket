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
       public ObservableCollection<ScanResultStudents> Items = new ObservableCollection<ScanResultStudents>();

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
            await DisplayAlert("Подтверждение", "Результаты успешно отправлены!", "Назад");
            await Navigation.PopAsync();
            Items.Clear();
        }
    }
}
