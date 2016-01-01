using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace AmazingGeoRace.Common
{
    public static class MessageBoxWrapper
    {
        public static async Task ShowAsync(string message) {
            var d = new MessageDialog(message);
            await d.ShowAsync();
        }

        public static async Task ShowOkAsync(string message)
        {
            var d = new MessageDialog(message);
            d.Commands.Add(new UICommand("OK") {
                Id = 0
            });
            await d.ShowAsync();
        }


        public static async Task ShowAsync(string message, string title) {
            var d = new MessageDialog(message, title);
            await d.ShowAsync();
        }
    }
}