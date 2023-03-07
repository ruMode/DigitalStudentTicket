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
       
        private async void ZXingScannerView_OnScanResult(ZXing.Result result)
        {

            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    studentsList.Items.Add(new Models.ScanResultStudents { Text = result.Text, Detail = "Студент" });
                
                    await Navigation.PushAsync(studentsList);
                    Navigation.RemovePage(this);
                    
                });

            }
            catch (Exception e)
            {

                await DisplayAlert("Error", e.Message, "Ok");
                await Navigation.PopAsync();
            }
        }

        private async void ZXingDefaultOverlay_FlashButtonClicked(Button sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (!_isFlashlightTurnOn)
                {
                    await Flashlight.TurnOnAsync();
                }
                else await Flashlight.TurnOffAsync();
                
            });
          
        }
    }
}