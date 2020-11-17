using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace InventoryOrderManger.Common
{
    public class Utils
    {
        public static void MakeSureDirectoryExists(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        public static ImageSource GetDefaultImage()
        {
            return ImageSource.FromResource("StockManager.Images.DefaultImage.png", typeof(Utils));
        }

        public static string GetDateTimeFileName(string extension = "")
        {
            return $"{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss_fff")}{extension}";
        }
    }
}
