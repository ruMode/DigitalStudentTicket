using DigitalStudentTicket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigitalStudentTicket.Views.StudentsViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudentProfileView : ContentPage
    {
        public StudentProfileView()
        {
            InitializeComponent();

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            List<Students> students = new List<Students>();
            List<Students> _students = App.Database.GetAllStudents();
            
            foreach (var item in _students)
            {
                students.Add(item);
            }
            //LV.ItemsSource = students;
        }


    }
}