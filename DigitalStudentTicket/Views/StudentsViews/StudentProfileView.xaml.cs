using DigitalStudentTicket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;
using ZXing.QrCode;

namespace DigitalStudentTicket.Views.StudentsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class StudentProfileView : ContentPage
    {
        public static string StudentCode { get; set; }
        public StudentProfileView()
        {
            InitializeComponent();

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
             
            if (StudentCode != null )
            {
                var st = App.Database.GetStudent(StudentCode);
                BindingContext = st;
                QRImage.BarcodeValue = GenerateQR(st.Code_Student).BarcodeValue;
                
               // QRImage = GenerateQR(st.Code_Student);
            }
            else
            {
                var st = App.Database.GetAllStudents().First();
                BindingContext = st;
                QRImage.BarcodeValue = GenerateQR(st.Code_Student).BarcodeValue;
            }

        }
        ZXingBarcodeImageView GenerateQR(string studentCode)
        {
            var qrCode = new ZXingBarcodeImageView
            {
                BarcodeFormat = BarcodeFormat.QR_CODE,
                BarcodeOptions = new QrCodeEncodingOptions
                {
                    Height = 250,
                    Width = 250,
                    PureBarcode = true,
                    QrVersion = 2
                },
                BarcodeValue = studentCode,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,

            };
            // Workaround for iOS
            qrCode.WidthRequest = 250;
            qrCode.HeightRequest = 250;
            return qrCode;
        }

    }
}