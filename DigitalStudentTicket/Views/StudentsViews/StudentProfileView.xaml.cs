﻿using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

            if (StudentCode != null ) //проверка на пустоту кода студента
            {
                var st = App.Database.GetStudent(StudentCode); //ищем в базе нужного студента
                BindingContext = st;
                //формируем QR-код
                QRImage.BarcodeValue = st.Code_Student;
                QRImage.HeightRequest = 150;
                QRImage.WidthRequest = 150;
                QRImage.BarcodeOptions.Height = 150;
                QRImage.BarcodeOptions.Width = 150;

            }
            else //если код студента пустой, то выбираем первого студента из базы
            {
                var st = App.Database.GetAllStudents().First();
                BindingContext = st;
                //формируем QR-код
                QRImage.BarcodeValue = st.Code_Student;
                QRImage.HeightRequest = 150;
                QRImage.WidthRequest = 150;
                QRImage.BarcodeOptions.Height = 150;
                QRImage.BarcodeOptions.Width = 150;
            }

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new FullscreenQRview(QRImage));
            
        }
    }
}