using DigitalStudentTicket.Views;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigitalStudentTicket
{
    public partial class AppShell : Shell
    {
        private Stack<ShellNavigationState> Uri { get; set; } // Navigation stack.  
        public  AppShell()
        {
            InitializeComponent();
            this.FlyoutBehavior = FlyoutBehavior.Disabled;
            Uri = new Stack<ShellNavigationState>();
        }
        //protected override void OnNavigated(ShellNavigatedEventArgs args)
        //{
        //    base.OnNavigated(args);
        //    if (Uri != null && args.Previous != null)
        //    {
        //        if (temp == null || temp != args.Previous)
        //        {
        //            Uri.Push(args.Previous);
        //            temp = args.Current;
        //        }
        //    }
        //}
        //protected override bool OnBackButtonPressed()
        //{
        //    if (Uri.Count > 0)
        //    {
        //        Shell.Current.GoToAsync(Uri.Pop());
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}