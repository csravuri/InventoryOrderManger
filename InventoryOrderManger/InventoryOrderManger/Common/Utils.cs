using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace InventoryOrderManger.Common
{
    public static class Utils
    {
        private static ImageSource defaultImage = null;
        public static void MakeSureDirectoryExists(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        public static ImageSource GetDefaultImage()
        {
            if (defaultImage == null)
            {
                defaultImage = ImageSource.FromResource("InventoryOrderManger.Images.DefaultImage.png", typeof(Utils));
            }

            return defaultImage;
        }

        public static string GetDateTimeFileName(string extension = "")
        {
            return $"{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss_fff")}{extension}";
        }

        public static bool IsNumber(string text, bool allowNullOrEmpty = true)
        {
            if (string.IsNullOrEmpty(text))
            {
                return allowNullOrEmpty;
            }

            if (text.All(x => (x >= 48 || x == 46) && (x <= 57 || x == 46)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static decimal ToDecimal(string text, decimal defaultValue = 0)
        {
            decimal outValue;
            if (decimal.TryParse(text, out outValue))
            {
                return outValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static string ToString(decimal decimalValue, string defaultValue = null)
        {
            if (decimalValue == 0)
            {
                return defaultValue;
            }
            else
            {
                return decimalValue.ToString();
            }
        }
        public static string ToString(string text, string defaultValue = null)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return defaultValue;
            }
            else
            {
                return text.ToString();
            }
        }
        public static void AddRange<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> list)
        {
            if (list == null) { throw new ArgumentNullException(nameof(list)); }
            
            list.ForEach(x => observableCollection.Add(x));
        }
    }
}
