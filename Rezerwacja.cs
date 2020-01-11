using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uwp_App
{
    public class Rezerwacja
    {
        [Key]
        public int _id { get; set; }
        public int nrPokoju { get; set; }
        public int nRezerwacji { get; set; }

        public DateTime Przyjazd { get; set; }

        public double Dlugosc { get; set; }

        public string adres { get; set; }

        public string imie { get; set; }
        public string nazwisko { get; set; }
        public int pesel { get; set; }

        public Rezerwacja() { }


        public async void DodajRezerwacje(string imie, string nazwisko, int pesel, string adres, int nRezerwacji, int nrPokoju)
        {
            // Dawne klasy klient osoba, której anie razu nie użyłęm i EntityFramework nie potrafił zrobic poprawnej migracji
            this.imie = imie;
            this.nazwisko = nazwisko;
            this.pesel = pesel;
            this.adres = adres;            
           
                // po nr rezerwacji mozna sprawdzi kto to    


            Przyjazd = await MSB.InputDate();
            double dlugosc;

            if (!double.TryParse(await MSB.Input("Podaj ilość dni"), out dlugosc))
            {
                await MSB.Print("Podaj poprawne dane");
            }

            Dlugosc = dlugosc;
            this.nRezerwacji = nRezerwacji;
            this.nrPokoju = nrPokoju;

            //baza musi zawierac nr pokojow


            using (var ctx = new DbModel())
            {

                var wolnepokoje = new List<int>();


                /*
                for (int i = 0; i < 10; i++) // sprawdza wszytskie pokoje
                {
                */
                int i = nrPokoju; // sprawdza pokoj ktory podano


                var rezerwacjewpokoju = ctx.TRezerwacja.Where(a => a.nrPokoju == i);

                int lwarunkow = 0;

                foreach (var item in rezerwacjewpokoju)
                {

                    try
                    {
                        DateTime wyjazdzpokoju = item.Przyjazd.AddDays(item.Dlugosc); // opuszczenie pokoju hotelowego

                        DateTime wyjazdgoscia = this.Przyjazd.AddDays(this.Dlugosc); // opuszczenie pokoju przez goscia

                        if (wyjazdzpokoju < this.Przyjazd)
                        {
                            // po terminie zajetego pokoju
                            lwarunkow++;
                        }
                        else if (wyjazdgoscia < item.Przyjazd)
                        {
                            // przed terminem zajetego pokoju
                            lwarunkow++;
                        }
                    }
                    catch { }
                }

                if (lwarunkow == rezerwacjewpokoju.Count())
                {
                    wolnepokoje.Add(i); // kazder rezerwacji nie przeszkadza termin
                }

                //}  //end for

                if (wolnepokoje.Count == 0)
                {
                    var res = await MSB.InputChoise("Brak wolnych pokoi\nWprowadź inny termin\n", "Szukaj dalej", "Rezygnuj");

                    if (res == Windows.UI.Xaml.Controls.ContentDialogResult.Primary)
                    {
                        //szukaj dalej
                        DodajRezerwacje(imie, nazwisko, pesel, adres, nRezerwacji, nrPokoju);
                        return;
                    }
                    else
                    {
                        //anulowano albo zrezygnowano
                        await MSB.Print("Zrezygnowano"); // zrezygnowano
                        return;
                    }
                }


                string str = "wolnego pokoju spośród: ";

                foreach (var item in wolnepokoje)
                {
                    str += item;
                    str += ", ";
                }

                while (true)
                {
                    var res = await MSB.PobierzNrAsync(str);  // zapytaj ktory pokój

                    foreach (var item in wolnepokoje)
                    {
                        if (item == res)
                        {//podano poprawny wolny nr pokoju

                            ctx.Add(this);
                            ctx.SaveChanges();
                            await MSB.Print(String.Format("Dodałem rezerwację on numerze {0} dla {1} {2} na {3}", nRezerwacji, imie, nazwisko, Przyjazd));
                            return;
                        }
                        else
                        {
                            await MSB.Print("Wybierz poprawny numer");
                        }
                    }
                }



                //}
                //catch
                //{
                //    await MSB.Print("Brak pokoi w hotelu");
                //}
            }

        }

        public async void AnulujRezerwacje(int nRezerwacji)
        {
            this.nRezerwacji = nRezerwacji;

            using (var ctx = new DbModel())
            {
                // var ask = "SELECT TRezerwacja FROM TMeldunek WHERE nRezerwacji == " + nRezerwacji;
                //  var tmp = ctx.TRezerwacja.FromSql(ask);
                var tmp = ctx.TRezerwacja.Where(a => a.nRezerwacji == nRezerwacji);
                try
                {
                    var item = tmp.FirstOrDefault();
                    //if (tmp.FirstOrDefault().nRezerwacji == nRezerwacji)
                    //{
                    Przyjazd = item.Przyjazd;
                    Dlugosc = item.Dlugosc;

                    nrPokoju = item.nrPokoju;

                   // klient = tmp.FirstOrDefault().klient;

                    // Autoryzacja Zakładam że autoryzacja z góry przebiegla pomyślnie, ponieważ przy wejściu do aplikacji to sprawdzam

                    //var ask2 = "DELETE FROM TRezerwacja WHERE nRezerwacji ==  " + nRezerwacji;

                    ctx.Remove(ctx.TRezerwacja.Single(a => a.nRezerwacji == nRezerwacji));
                    await ctx.SaveChangesAsync();


                    // ctx.TRezerwacja.FromSql(ask2);

                    var wyjazd = Przyjazd.AddDays(Dlugosc);
                    //wyjazd.AddDays(Dlugosc);

                    await MSB.Print(String.Format("Rezerwacja pokoju {0} dla {1} {2} usunięta od {3} do {4}", item.nrPokoju, item.imie, item.imie, Przyjazd.ToString(), wyjazd.ToString()));
                    //}

                } // wyrzuci błąd bo gdy nie ma nr klucza dla danego nr rezerwacji wartośc jest null em
                catch
                {
                    await MSB.Print("Rezerwacja nr: " + nRezerwacji + " nie istnieje"); // raczej nie powiino się to pojawić skoro wczesniej sprawdzano to
                }
            }

        }

        /// <summary>
        /// Sprawdza czy istneije rezerwacja o tym numerze
        /// </summary>
        /// <param name="nRezerwacji"></param>
        /// <returns></returns>
        public bool SprawdzRezerwacje(int nRezerwacji)
        {
            using (var ctx = new DbModel())
            {
                //var ask = "SELECT " + nRezerwacji + " From TRezerwacja";
                //var tmp = ctx.TRezerwacja.FromSql(ask);

                foreach (var item in ctx.TRezerwacja.ToList())
                {
                    if (item.nRezerwacji == nRezerwacji)
                    {
                        return true;
                    }
                }



                //try
                //{
                //    if (tmp.FirstOrDefault() != null) //jesli cos jest
                //    {
                //        return true;
                //    }
                //}
                //finally { }
            }
            return false;
        }


        private DateTime CombineDateAndTime(DateTime dateObj, DateTime timeObj)
        {
            //DateTime timeObj = new DateTime().Add(time);
            DateTime newDateTime;

            //newDateTime.add

            //get timespan from the date object
            TimeSpan spanInDate = dateObj.TimeOfDay;

            //subtract it to set date objects time to 0:00
            dateObj = dateObj.Subtract(spanInDate);

            //now add your newTime to date object
            newDateTime = dateObj.Add(timeObj.TimeOfDay);

            //return new value
            return newDateTime;
        }
    }
}


