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
                 if (ScanResultStudentsListView.Items.FirstOrDefault(i => i.Code == result.Text) != null)
                 {
                     //выдаем ошибку если студент был отсканирован повторно
                     errFrame.IsVisible=true;                     
                 }
                 else
                 {
                     //создаем список отсканированный студентов
                     Models.ScanResultStudents newItem = new Models.ScanResultStudents() 
                     { 
                         Text = MainPage.studentsData.studentSave[MainPage.studentsData.Code_student_J.IndexOf(result.Text)], 
                         Code=result.Text, 
                         ScanDate = DateTime.Now.ToLocalTime() 
                     };
                     ScanResultStudentsListView.Items.Add(newItem);
                     ScanResultStudentsListView.scansCount++;
                     await Navigation.PushAsync(studentsList);
                     
                 }
             });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            errFrame.IsVisible = false;
            ZXingScannerView.IsScanning = true; ZXingScannerView.IsAnalyzing = true;

        }

        private async void EndScan_Clicked(object sender, EventArgs e)
        {
            ZXingScannerView.IsScanning = true; ZXingScannerView.IsAnalyzing = true;
            errFrame.IsVisible = false;
            await Navigation.PushAsync(studentsList);
        }
    }
}