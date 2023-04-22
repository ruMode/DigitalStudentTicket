using DigitalStudentTicket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace DigitalStudentTicket.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScannerPageView : ContentPage 
    {
        ScanResultStudentsListView studentsList = new ScanResultStudentsListView();
        public bool _isFlashlightTurnOn { get; set; } = false;
        public ScannerPageView()
        {
            InitializeComponent();
        }
        protected override void OnDisappearing()
        {
            Navigation.RemovePage(this);
            base.OnDisappearing();

        }
        private void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
             {
                 
                 Models.ScanResultStudents newItem = new Models.ScanResultStudents() { Text = result.Text, ScanDate = DateTime.Now.ToLocalTime()};
                 

                 if (ScanResultStudentsListView.Items.FirstOrDefault(i=> i.Text==newItem.Text)!= null)//не работает //работает
                 {
                     ScanResultStudentsListView.Items.Remove(newItem);
                     await DisplayAlert("Ошибка!", "Такой студент уже был отсканирован!", "Ок");
                 }
                 else
                 {
                     ScanResultStudentsListView.Items.Add(newItem);
                     await Navigation.PushAsync(studentsList);
                 }
             });
        }

 

    }
}