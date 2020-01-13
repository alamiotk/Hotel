using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Uwp_App
{
    public class Atrakcja
    {
        // Zakaładam że pojęcie "dwa miejsca" to dwie godziny czy;i czas trwania atrakcji

        [Key]
        public int _id { get; set; }
        public int nRezerwacji { get; set; }
        public string rodzaj { get; set; }
        public DateTime data { get; set; }

        public Atrakcja()
        {
            data = DateTime.Now;
        }

        public async void ZarezerwujAtrakcje(int nRezerwacji, string Rodzaj)
        {
            this.nRezerwacji = nRezerwacji;
            this.rodzaj = Rodzaj;

            data = await MSB.InputDate(); // podaj date
            var time = await MSB.InputTime(); // podaj godzine

            try
            {
                data = CombineDateAndTime(data, time);
            }
            catch { await MSB.Print("Podaj poprawne dane"); return; }

            if (Sprawdz(rodzaj))
            {
                var res = await MSB.InputChoise(rodzaj + " jest zajęte\nWybierz typ rezerwacji lub inny czas", "Basen", "Spa");

                if (res == ContentDialogResult.Primary)
                {
                    rodzaj = "Basen";
                }
                else if (res == ContentDialogResult.Secondary)
                {
                    rodzaj = "Spa";
                }
                else
                {
                    await MSB.Print("Dokonaj wyboru");
                }

                ZarezerwujAtrakcje(nRezerwacji, rodzaj);

            }
            else
            {
                
                if (rodzaj == "Basen")
                {
                    new Basen(this).ZajmijMiejsce(nRezerwacji);
                }
                else
                {
                    new Spa(this).ZajmijMiejsce(nRezerwacji);
                }                
            }
        }

        /// <summary>
        /// Zwraza Czy zajęta atkrakcja o czasie podanym w klasie
        /// </summary>
        /// <param name="rodzaj">Rodzaj atrakcji</param>
        /// <returns> true - zajęte </returns>
        public bool Sprawdz(string rodzaj)
        {
            using (var ctx = new DbModel())
            {
                //  var ask = "SELECT data From TAtrakcja WHERE rodzaj == '" + rodzaj + "'";
                //  var tmp = ctx.TAtrakcja.FromSql(ask);

                var tmp = ctx.TAtrakcja.Where(a => a.rodzaj == rodzaj);

                if(tmp.Count() == 0)
                {
                    return false;
                }
                try
                {
                    foreach (var item in tmp.ToArray())
                    {

                        if ((item.data + new TimeSpan(2, 0, 0)) <= data) // trwa rezerwacj i chce sie zapisac  // koniec rezerwacji jest mniejszy od poczatku nowej
                        {
                            return true;
                        }

                        if ((data + new TimeSpan(2, 0, 0)) >= item.data) // podczas trwania atrakcji ktos zaczyna swoja // koniec nowej rezerwacj wiekszy od pocz rezerweacj
                        {
                            return true;
                        }
                    }
                }
                finally
                {
                    //jeśli 2 godziny po ropoczęciu poprzedniej atrakcji to zapraszam lub jeśli nie ma wartości w bazie tj pusta atrakcja                   
                }
                return false;
            }
        }

        private DateTime CombineDateAndTime(DateTime dateObj, TimeSpan time)
        {
            

                DateTime timeObj = new DateTime().Add(time);
                DateTime newDateTime;

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
