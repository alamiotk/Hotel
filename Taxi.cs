using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Uwp_App
{
    public class Taxi
    {
        [Key]
        public int _id { get; set; }
        public int nRezerwacji { get; set; }
        public TimeSpan godzina { get; set; }

        public async void zamowTaxi(int nRezerwacji)
        {
            this.nRezerwacji = nRezerwacji;
            godzina = await MSB.InputTime(); // wprowadz godzine


            if (godzina <= TimeSpan.Zero) return;

            //if(godzina <= new TimeSpan(0, 0, 0))
            //{
            //    await MSB.Print("Podaj godzine");
            //    return;
            //}

            // zadzwon po taxi
            var odpowiedz = true; // uzyskaj odpowiedz

            var text = "";
            if(odpowiedz)
            {
                // zarezerwuj taxi
                using (var db = new DbModel())// zapisz do db rezerwacje
                {
                    db.TTaxi.Add(this);
                    db.SaveChanges();
                }    
                text = "Zarezerwowano przejazd na numer: " + this.nRezerwacji + " na godzinę: " + godzina.ToString();
            }
            else
            {
                text = "Brak dostępnych złotów";// zrezygnuj               
            }

            await MSB.Print(text); 

        }
    }
}
