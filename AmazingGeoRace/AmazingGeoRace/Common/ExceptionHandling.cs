using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace AmazingGeoRace.Common
{
    public static class ExceptionHandling
    {
        public static async Task HandleException(Action methodToHandle) {
            try {
                methodToHandle();
            }
            catch (Exception ex) {
                var dialog = new MessageDialog(ex.Message, "Error");
                await dialog.ShowAsync();
            }
        }

        public static async Task HandleExceptionForAsyncMethod(Func<Task> methodToHandle)
        {
            try
            {
                await methodToHandle();
            }
            catch (Exception ex)
            {
                var dialog = new MessageDialog(ex.Message, "Error");
                await dialog.ShowAsync();
            }
        }
    }
}
