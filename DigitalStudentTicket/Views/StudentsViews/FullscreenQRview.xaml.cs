using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace DigitalStudentTicket.Views.StudentsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FullscreenQRview : ContentPage
    {
       // private ZXingBarcodeImageView qRImage;

        public FullscreenQRview()
        {
            InitializeComponent();
        }
        public FullscreenQRview(ZXingBarcodeImageView qRImage)
        {
           this.Content = qRImage;
            
        }
    }
}