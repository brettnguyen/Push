using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Support.V4.App;

using Xamarin.Essentials;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Android;

namespace Push.Droid
{
    [Activity(Label = "Push", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private const int CameraPermissionCode = 100;
      

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            // Check for and request camera permission here
            CheckCameraPermission();
        }

        // Check for camera permission and request it if not granted
        private void CheckCameraPermission()
        {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) != Permission.Granted)
            {
                // Permission is not granted, request it
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.Camera }, CameraPermissionCode);
            }
            else
            {
                // Permission is already granted, proceed with camera operations
                // Start your camera-related code here
            }
        }

        // Handle the result of the permission request
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == CameraPermissionCode)
            {
                if (grantResults.Length > 0 && grantResults[0] == Permission.Granted)
                {
                    // Permission granted, you can now access the camera
                    // Start your camera-related code here
                }
                else
                {
                    // Permission denied, handle accordingly (e.g., show a message)
                }
            }

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

