using DigitalStudentTicket.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigitalStudentTicket.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TeachersListView : ContentPage
    {
        List<Teachers> _items = App.Database.GetAllTeachers();
        ObservableCollection<Teachers> users = new ObservableCollection<Teachers>();

        public TeachersListView()
        {
            InitializeComponent();
            
            foreach (Teachers item in _items)
            {
                users.Add(item); 
            }
         
            usersLV.ItemsSource = users;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", $"", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            App.Database.DeleteAllUsers();
            users.Clear();
            _items.Clear();
            //Items.Clear(); 
        }
    }
}
