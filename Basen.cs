using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uwp_App
{
    public class Basen
    {
        Atrakcja atrakcja;
        public Basen(Atrakcja atrakcja)
        {
            this.atrakcja = atrakcja;
        }

        public async void ZajmijMiejsce(int nRezerwacji)
        {
            atrakcja.nRezerwacji = nRezerwacji;
            //Nierozuiem sensu
            using (var ctx = new DbModel())
            {
                ctx.TAtrakcja.Add(atrakcja); // dodaj, zarezerwuj
                ctx.SaveChanges();
            }

            await MSB.Print("Basen zarezerwowany na " + atrakcja.data.ToString());
        }
    }
}
