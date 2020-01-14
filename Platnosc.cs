using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uwp_App
{
    class Platnosc
    {
        public async Task<bool> ZaplacAsync(int nRezerwacji)
        {
            

            var res = await MSB.InputChoise("Wybierz metodę płatności", "Karta", "Gotówka");
           

            switch (res)
            {
               
                case Windows.UI.Xaml.Controls.ContentDialogResult.None:
                    {
                        return false;
                    }
                case Windows.UI.Xaml.Controls.ContentDialogResult.Primary:
                    {
                        await Card();
                        return true;
                    }
                case Windows.UI.Xaml.Controls.ContentDialogResult.Secondary:
                    {
                        await Cash();
                        return true;
                    }
            }


            return true; //zakladam że zapłacił
        }

        private async Task Card()
        {
            await MSB.Print("Postępuj według terminala"); // jakoś tak piszą.. na kasach
        }

        private async Task Cash()
        {
            await MSB.Print("Przyjmij gotówkę");

         
        }

    }
}
