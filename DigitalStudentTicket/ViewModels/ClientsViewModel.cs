using DigitalStudentTicket.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DigitalStudentTicket.ViewModels
{
    public class ClientsViewModel
    {
        public ObservableCollection<ScanResultStudents> Items { get; set; } = new ObservableCollection<ScanResultStudents>(); //= new List<ScanResultStudents>() { new ScanResultStudents { Text="Test item", Detail="detail"} };

        public ClientsViewModel()
        {
            
        }
    }
}
