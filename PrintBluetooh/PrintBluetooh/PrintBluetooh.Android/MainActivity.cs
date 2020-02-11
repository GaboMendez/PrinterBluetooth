using Android.App;
using Android.Content.PM;
using Android.OS;
using PrintBluetooh.Droid.Services;
using PrintBluetooh.Services;
using Prism;
using Prism.Ioc;

namespace PrintBluetooh.Droid
{
    [Activity(Label = "PrinterApp", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        static PrismBluetoothService bluetoothService = new PrismBluetoothService();
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));
        }

        public class AndroidInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IContainerRegistry containerRegistry)
            {
                // Register any platform specific implementations
                containerRegistry.RegisterInstance<IBluetoothService>(bluetoothService);
            }
        }
    }
}

