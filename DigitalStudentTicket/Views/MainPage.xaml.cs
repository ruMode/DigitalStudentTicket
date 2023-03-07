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
        public static ScannerPageView scannerPage = new ScannerPageView();
        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            currentDateLabel.Text = DateTime.Now.ToShortDateString();

            #region CollectionView Add items

            List<SheduleItems> shedule = new List<SheduleItems>();
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
            sheduleCV.ItemsSource = shedule;
            #endregion
        }
        private async void sheduleCV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Navigation.PushAsync(scannerPage);
            
        }
    }
}
