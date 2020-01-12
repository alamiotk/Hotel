using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uwp_App
{
    public class Meldunek
    {
        [Key]
        public int nRezerwacji { get; set; }
        public int nrKlucza { get; set; }
        public bool czyZaplacono { get; set; }


        public async void Zamelduj(int nRezerwacji, int nrKlucza)
        {
            this.nrKlucza = nrKlucza;            //klient zamedowany
            this.nRezerwacji = nRezerwacji;
            this.czyZaplacono = true;


            await MSB.Print("Wydaj klucz do pokokju nr: " + nrKlucza);            // wydano klucz

            using (var ctx = new DbModel())
            {
                ctx.TMeldunki.Add(this);
                ctx.SaveChanges();
                await MSB.Print("Wpisałem w baze");
            }
        }

        public async void Wymelduj(int nRezerwacji, int nrKlucza)
        {
            using (var ctx = new DbModel())
            {
                //var ask = "SELECT nrKlucza FROM TMeldunek WHERE nRezerwacji == " + nrKlucza;
                //var tmp = ctx.TMeldunki.FromSql(ask);
                var tmp = ctx.TMeldunki.Where(a => a.nrKlucza == nrKlucza).ToArray();

                if(tmp.Count() == 0)
                {
                    await MSB.Print("Nie wydano klucza nr: " + nrKlucza);
                }
                else
                {
                    if (nRezerwacji != tmp.FirstOrDefault().nRezerwacji)
                    {
                        await MSB.Print("Dla rezerwacji nr: " + nRezerwacji + " nie wydano klucza nr: " + nrKlucza);
                        return;
                    }
                    ctx.Remove(tmp[0]);
                    ctx.SaveChanges();
                    await MSB.Print("Wypisałem z bazy");
                }
            }
        }

        public async void SprawdzPlatnosc(int nRezerwacji) // skoro ma zwracac void a -> to nieważne co tu się stanie, ale na sequencediagram_meldunek zwracany typ to void a w zależności od wyniku tej fukcji ma się wykonać część kodu mawet na diagramie jest to: zwraca void a a ma po bool u działać potem ??
        {
            var payment = new Platnosc();
            var res = await payment.ZaplacAsync(nRezerwacji);
            if (res)
            {
                //zaplacono
                czyZaplacono = true; // zaznacz platnosc w systemie
                Zamelduj(nRezerwacji, nrKlucza);
            }
            else
            {
                czyZaplacono = false; // skoro nie zaplacil to sie nie zamleowal czyli nie bd zapisaywal tej informacji nawet
            }
        }

    }
}
