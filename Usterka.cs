using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uwp_App
{
    public class Usterka
    {
        [Key]
        public int _id { get; set; }
        public int nrUsterki { get; set; }
        public int NrRezerwacji { get; set; }
        public string opisUsterki { get; set; }
        public bool CzyUsunieto  { get; set; }

  

        public async void DodajUsterke(int NrRezerwacji, int NrUsterki, string OpisUsterki)
        {
            CzyUsunieto = false;
            this.NrRezerwacji = NrRezerwacji;
            this.nrUsterki = NrUsterki;
            this.opisUsterki = OpisUsterki;

            using (var ctx = new DbModel())
            {
                var ask = "SELECT opisUsterki From TUsterka WHERE nrUsterki == '" + opisUsterki + "'";
                //   var odp = ctx.TUsterka.FromSql(ask);

                var odp = ctx.TUsterka.Where(a => a.nrUsterki == nrUsterki);

                try
                {
                    var odp2 = odp.FirstOrDefault<Usterka>().opisUsterki;

                    if (String.Compare(odp2, opisUsterki) == 0)
                    {
                        CzyUsunieto = SprawdzStatus(nrUsterki);
                        if (CzyUsunieto) //Usterka o takim opisie została juz zgłoszona oraz została usunieta, tj. pojawiła się znowu -> dodaj znow
                        {
                            this.CzyUsunieto = false;
                        }
                        else
                        {
                            await MSB.Print("Taka usterka jest w naprawie");
                            //Usterka o takim opisie została juz zgłoszona oraz nie została zakonczona, tj. work in progress
                            return;
                        }
                    }
                }
                catch
                {
                    // usterka taka nie istnieje
                }
                ctx.TUsterka.Add(this);
                ctx.SaveChanges();
                await MSB.Print("Dodno usterke");
            }



           


        }

        public bool SprawdzStatus(int NrUsterki)
        {
            using (var ctx = new DbModel())
            {
                //  var ask = "SELECT CzyUsunieto From usterka WHERE nrUsterki == " + NrUsterki;
                //   var odp = ctx.TUsterka.FromSql(ask);

                var odp = ctx.TUsterka.Where(a => a.nrUsterki == NrUsterki);

                if (odp.FirstOrDefault<Usterka>().CzyUsunieto)
                {
                    return true;
                }      
                else
                {
                    return false;
                }
            }
        }

    }
}
