
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

      

     


        private async void Spr_Rezerwazje_btn_Click(object sender, RoutedEventArgs e)
        {
            var odp = await MSB.InputChoise("Rezerwacja: ", "Dodaj nową rezerwację", "Sprawdź już istniejącą");

            switch (odp)
            {
                case ContentDialogResult.Primary:
                    {
                        var nr = await MSB.PobierzNRezerwacjiAsync();
                        if (nr == 0)
                        {
                            return;
                        }

                        var rezerwacja = new Rezerwacja();


                        var imie = await MSB.Input("Podaj imię");
                        var nazwisko = await MSB.Input("Podaj nazwisko");
                        var pesel = await MSB.PobierzNrAsync("pesel");

                        var nrPokoju = await MSB.PobierzNrAsync("pokoju");

                        var adres = await MSB.Input("Podaj adres");

                        rezerwacja.DodajRezerwacje(imie, nazwisko, pesel, adres, nr, nrPokoju); // po co podawac nr pokoju ? skoro mam sprawdzac po wszytskich pokojach
                    }
                    break;
                case ContentDialogResult.Secondary:
                    {
                        var nr = await MSB.PobierzNRezerwacjiAsync();
                        if (nr == 0)
                        {
                            return;
                        }

                        var rezerwacja = new Rezerwacja();

                        if (rezerwacja.SprawdzRezerwacje(nr))
                        {
                            //wyswietl anuluj
                            var res = await MSB.InputChoise("Rezerwacja nr: " + nr + " znajduje się w systemie", "Wyświetl szczegóły", "Anuluj rezerwacje");

                            switch (res)
                            {
                                case ContentDialogResult.None:
                                    {
                                        //anulowano
                                        break;
                                    }
                                case ContentDialogResult.Primary:
                                    {
                                        // wyswietl

                                        using (var ctx = new DbModel())
                                        {
                                            //  var ask = "SELECT * From TRezerwacja WHERE nRezerwacji==" + nr;


                                            try
                                            {
                                                // var tmp = ctx.TRezerwacja.FromSql(ask).FirstOrDefault();

                                                var tmp = ctx.TRezerwacja.Where(a => a.nRezerwacji == nr).FirstOrDefault();

                                                var str = string.Format("Dla numeru rezerwacji {0} zarejestrono osobe {1} {2}, pesel: {3}, adres: {4}", tmp.nRezerwacji, tmp.imie, tmp.nazwisko, tmp.pesel, tmp.adres);

                                                await MSB.Print(str);
                                            }
                                            catch
                                            {
                                            }
                                        }
                                        break;
                                    }
                                case ContentDialogResult.Secondary:
                                    {
                                        // anuluj rezerwacje
                                        rezerwacja.AnulujRezerwacje(nr);
                                        break;
                                    }

                            }
                        }
                        else
                        {
                            await MSB.Print("Rezerwacja o numerze: " + nr + "\nnie istnieje w systemie.");
                        }
                    }
                    break;
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
