using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Uwp_App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {


        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.GetForCurrentView().SetPreferredMinSize( new Size(450,200));

            //// Delete this !!
            //txtUser.Text = "Default";
            //txtPassword.Password = "Default";
            //txtId.Text = "1";
            ////            
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new DbModel())// zapisz do db rezerwacje
            {
                int id = -1;
                if(! int.TryParse(txtId.Text, out id))
                {
                    await MSB.Print("Podaj poprawne dane");
                    return;
                }
                var usr = txtUser.Text;
                var psw = txtPassword.Password;

                if(db.TRecepcjonista.ToArray().Length == 0)
                {
                    db.TRecepcjonista.Add(new Users{id = 0, login="admin", haslo = "admin"}); // konto admina
                    db.SaveChanges();
                }

                foreach (var item in db.TRecepcjonista.ToArray())
                {                    
                    if (item.id == id)
                    {
                        if (item.login == usr)
                        {
                            if(item.haslo == psw)
                            {
                                //autoryzacja
                                var Recepcjonista = new Recepcjonista(usr, psw, id);
                                this.Frame.Navigate(typeof(Recepcjonista));
                            }                            
                        }

                    }
                    else
                    {
                        await MSB.Print("Brak autoryzacji");
                    }
                }
                //db.TRecepcjonista
            }

           // var RecepcjonistaDev = new Recepcjonista("Default", "Default", 1);

            //this.Frame.Navigate(typeof(Recepcjonista)); //delete this!!
        }
    }
}
