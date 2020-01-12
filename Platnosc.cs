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
            // można by zapisać informacje o platnosci do bazy, ale sprawdz platnosc ma byc voidem czyli nie weryfikuje się tego, rozwniez z tego powodu nie ma mozliwosci przedplaty

            // mozna w do...while dac ta funkcje i sprawdzac jak nacisnieto rezygnacje z platnosci warunek zeby wyjsc z petli

            var res = await MSB.InputChoise("Wybierz metodę płatności", "Karta", "Gotówka");
            // gdy jednoczenie wejdzie sie tutaj wyrzuci wyjatek ze peracja asnchronicznie nie została poprawnie rozpoczeta, // moj blad przypadkowy dwa razy wywolalem new meldunek.sprawdzplatnosc

            switch (res)
            {
                // nieosiaglany kod: break;
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

            /*
             * Najpierw firma płatnosci
             * Obijasz kwotę na terminalie
             * On placi
             * Potem w komputerze w oknie klikasz
             * Ale jeszcze musisz zapytać paragon czy fakrura
             * 
             * Wg. kolezanki
             */
        }

    }
}
