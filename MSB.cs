using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Uwp_App
{
    class MSB
    {
        public static async Task Print(string text)
        {
            var messageDialog = new MessageDialog(text);
            messageDialog.Commands.Add(new UICommand("Ok"));
            await messageDialog.ShowAsync();
        }

        public static async Task<string> Input(string Tytul)
        {
            var inputTextBox = new TextBox { AcceptsReturn = false };
            (inputTextBox as FrameworkElement).VerticalAlignment = VerticalAlignment.Bottom;
            var dialog = new ContentDialog
            {
                Content = inputTextBox,
                Title = Tytul,
                IsSecondaryButtonEnabled = false,
                PrimaryButtonText = "Ok",
                CloseButtonText = "Anuluj",
            };
            var res = await dialog.ShowAsync();

            if (res == ContentDialogResult.Primary)
            {
                return inputTextBox.Text;
            }
            return "-1"; // anulowano

        }


        public static async Task<TimeSpan> InputTime()
        {
            var inputTextBox = new TimePicker(); //new TextBox { AcceptsReturn = false };
            (inputTextBox as FrameworkElement).VerticalAlignment = VerticalAlignment.Bottom;
            var dialog = new ContentDialog
            {
                Content = inputTextBox,
                Title = "Podaj godzinę",
                IsSecondaryButtonEnabled = false,
                PrimaryButtonText = "Ok",
                CloseButtonText = "Anuluj",
            };
            var res = await dialog.ShowAsync();
            if(res == ContentDialogResult.None)
            {
                return TimeSpan.Zero;
            }
            return inputTextBox.Time;
        }

        public static async Task<DateTime> InputDate()
        {
            var inputTextBox = new DatePicker(); //new TextBox { AcceptsReturn = false };
            (inputTextBox as FrameworkElement).VerticalAlignment = VerticalAlignment.Bottom;
            var dialog = new ContentDialog
            {
                Content = inputTextBox,
                Title = "Podaj datę",
                IsSecondaryButtonEnabled = false,
                PrimaryButtonText = "Ok",
                CloseButtonText = "Anuluj",
            };
            var res = await dialog.ShowAsync();
            if (res == ContentDialogResult.None)
            {
                return DateTime.MinValue;
            }
            return inputTextBox.Date.DateTime;
        }

        public static async Task<ContentDialogResult> InputChoise(string Title, string PrimaryButton, string SecondaryButton)
        {
            var inputTextBox = new TimePicker(); //new TextBox { AcceptsReturn = false };
            (inputTextBox as FrameworkElement).VerticalAlignment = VerticalAlignment.Bottom;
            var dialog = new ContentDialog
            {
                Content = Title,
                //  Title = Title,
                IsSecondaryButtonEnabled = true,
                PrimaryButtonText = PrimaryButton,
                SecondaryButtonText = SecondaryButton,
                CloseButtonText = "Anuluj",
            };
            var res = await dialog.ShowAsync();
            return res;
        }

        public static async System.Threading.Tasks.Task<int> PobierzNRezerwacjiAsync()
        {
            return await PobierzNrAsync("rezerwacji");
        }

        public static async System.Threading.Tasks.Task<int> PobierzNrAsync(string czegonumer)
        {
            var nr = 0;
            string numer = "";
            numer = await MSB.Input("Podaj numer " + czegonumer);

            if (!Int32.TryParse(numer, out nr))
            {
                await MSB.Print("Podaj poprawne dane");
                return 0;
            }
            return nr;
        }
    }
}
