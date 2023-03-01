using DigitalStudentTicket.Models;
using DigitalStudentTicket.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile;
using ZXing.Net.Mobile.Forms;

namespace DigitalStudentTicket
{
    public partial class MainPage : ContentPage
    {
        ZXingScannerPage scannerPage = new ZXingScannerPage()
        {
            IsAnalyzing = true,
            IsScanning = true,
            DefaultOverlayShowFlashButton = true,
            DefaultOverlayTopText = "Наведите камеру на QR-код студента"
        };
        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            currentDateLabel.Text = DateTime.Now.ToShortDateString();
            scannerPage.OnScanResult += ScannerPage_OnScanResult;

            

            #region CollectionView Add items

            List<SheduleItems> shedule= new List<SheduleItems>();
            shedule.AddRange(new SheduleItems[]
            {
                new SheduleItems()
                {
                    Id= 1,
                    Subject="Пара 1",
                    GroupName="ИС-407"
                },
                new SheduleItems()
                {
                    Id= 2,
                    Subject="Пара 2",
                    GroupName="ИС-407"
                },
                new SheduleItems()
                {
                    Id= 3,
                    Subject="Пара 3",
                    GroupName="ИС-407"
                },
                new SheduleItems()
                {
                    Id= 4,
                    Subject="Пара 4",
                    GroupName="ИС-407"
                },
                new SheduleItems()
                {
                    Id= 5,
                    Subject="Пара 5",
                    GroupName="ИС-407"
                },
            });
            sheduleCV.ItemsSource= shedule;
            #endregion
        }
        private async void sheduleCV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Navigation.PushAsync(scannerPage);
        }

        private async void ScannerPage_OnScanResult(ZXing.Result result)
        {
            scannerPage.IsAnalyzing = false; scannerPage.IsScanning = false;
            
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    
                    var dialogResult= await DisplayAlert("Результат сканирования", result.Text, "Сканировать ещё", "Назад", FlowDirection.LeftToRight);

                    if (dialogResult ==true)
                    {
                        ShowDisplayAlert(result);
                    
                    }
                    else
                    {
                        scanResultLabel.Text = result.Text;
                        await Navigation.PopAsync();

                    }
                });
            
            }
            catch (Exception e)
            {

                await DisplayAlert("Error", e.Message, "Ok");
                await Navigation.PopAsync();
            }
          
        }
        public async void ShowDisplayAlert(ZXing.Result result)
        {
            await DisplayAlert("Результат сканирования", result.Text, "Сканировать ещё", "Назад", FlowDirection.LeftToRight);
            scanResultLabel.Text = result.Text;
        }
    }
}
