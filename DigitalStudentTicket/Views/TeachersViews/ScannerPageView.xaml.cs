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
        protected override void OnAppearing()
        {
            ZXingScannerView.IsScanning = true; 
            base.OnAppearing();
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
                 //ZXingScannerView.IsScanning = false;

                 


                 if (ScanResultStudentsListView.Items.FirstOrDefault(i => i.Text == result.Text) != null)//не работает //работает //не работает
                 {
                     errFrame.IsVisible=true;
                    // await DisplayAlert("Ошибка!", "Такой студент уже был отсканирован!", "Ок"); 
                     
                 }
                 else
                 {
                     Models.ScanResultStudents newItem = new Models.ScanResultStudents() 
                     { 
                         Text = MainPage._lessonData.studentSave[MainPage._lessonData.Code_student_J.IndexOf(result.Text)], 
                         Code=result.Text, 
                         ScanDate = DateTime.Now.ToLocalTime() 
                     };
                     ScanResultStudentsListView.Items.Add(newItem);
                     ScanResultStudentsListView.scansCount++;
                     //newItem = null;
                     await Navigation.PushAsync(studentsList);
                     
                 }
             });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            errFrame.IsVisible = false;
            ZXingScannerView.IsScanning = true; ZXingScannerView.IsAnalyzing = true;

        }
    }
}