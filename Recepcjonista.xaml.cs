
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Uwp_App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Recepcjonista : Page
    {
        string login, haslo;
        int id;



        public Recepcjonista()
        {
            this.InitializeComponent();
        }

        

        

        private async void Wymeldowanie_btn_Click(object sender, RoutedEventArgs e)
        {
            var nr = await MSB.PobierzNRezerwacjiAsync();

            if (nr == 0)
            {
                return;
            }

            var nrKlucza = await MSB.PobierzNrAsync("klucza");

            new Meldunek().Wymelduj(nr, nrKlucza);

        }

        private async void Zameldowanie_btn_Click(object sender, RoutedEventArgs e)
        {
            var nr = await MSB.PobierzNRezerwacjiAsync();

            if (nr == 0)
            {
                return;
            }

            var nrKlucza = await MSB.PobierzNrAsync("klucza");

            // sprawdz czy zameldowany
            using (var ctx = new DbModel())
            {
                //var ask = "SELECT " + nr + " From TMeldunek";
                //var tmp = ctx.TMeldunki.FromSql(ask);

                var tmp = ctx.TMeldunki.Where(a => a.nrKlucza == nrKlucza).ToArray();

                if (tmp.Count() != 0)
                {
                    if (tmp.FirstOrDefault().nrKlucza == nrKlucza)
                    {
                        // jestli dla danego nr rezerwacji jest wydany klujcz tzn. ze osoba jest juz zameldowana
                        await MSB.Print("Dla rezerwacji: " + nr + " wydano już klucz nr: " + nrKlucza);
                        return;
                    }
                } // wyrzuci błąd bo gdy nie ma nr klucza dla danego nr rezerwacji wartośc jest null em

                else
                {
                    var meldunek = new Meldunek { nrKlucza = nrKlucza };
                    meldunek.SprawdzPlatnosc(nr);
                }
            }
        }


       


        public Recepcjonista(string login, string haslo, int id)
        {
            this.login = login;
            this.haslo = haslo;
            this.id = id;
        }



    }
}
