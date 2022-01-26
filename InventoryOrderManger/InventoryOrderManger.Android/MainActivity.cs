using System.IO;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;

namespace InventoryOrderManger.Droid
{
    [Activity(Label = "IO Manager 2", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App()
            {
                MyFolder = GetMyFolder()
            });
        }

        private string GetMyFolder()
        {
            var picturesFolder = Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures);
            var ioManagerFolder = Path.Combine(picturesFolder.AbsolutePath, "IO Manager");
            if (!Directory.Exists(ioManagerFolder))
            {
                Directory.CreateDirectory(ioManagerFolder);
            }

            return ioManagerFolder;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}