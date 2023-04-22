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
                 Models.ScanResultStudents newItem = new Models.ScanResultStudents();
                 newItem.Text = result.Text; newItem.Detail = "Cтудент";

                 if (studentsList.Items.Contains(newItem))//не работает
                 {
                     studentsList.Items.Remove(newItem);
                     await DisplayAlert("", "jib,rf", "gg");
                 }
                 else
                 {
                     studentsList.Items.Add(new Models.ScanResultStudents() { Text = result.Text, Detail = "Студент" });
                     await Navigation.PushAsync(studentsList);
                 }
             });
        }

        private async void ZXingDefaultOverlay_FlashButtonClicked(Button sender, EventArgs e)
        {
            
            if (!_isFlashlightTurnOn)
            {
                await Flashlight.TurnOnAsync();
            }
            else await Flashlight.TurnOffAsync();
           
          
        }

    }
}