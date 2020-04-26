using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Gms.Maps;
using Android;
using Android.Gms.Maps.Model;
using Android.Gms.Location;
using Android.Support.V4.App;

namespace PathFinder
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnMapReadyCallback
    {
        GoogleMap map;
        FusedLocationProviderClient providerClient;
        Android.Locations.Location myLastLocation;
        LatLng myposition;

        readonly string[] permissionGroup = {
            Manifest.Permission.AccessFineLocation,
            Manifest.Permission.AccessCoarseLocation    
        };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            RequestPermissions(permissionGroup, 0);
            SupportMapFragment mapFragment =
                (SupportMapFragment)SupportFragmentManager
                .FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            if (grantResults.Length < 1)
            {
                return;
            }

            if(grantResults[0] == (int)Android.Content.PM.Permission.Granted)
            {
                DisplayLocation();
            }

        }

        public void OnMapReady(GoogleMap googleMap)
        {
            var mapStyle = MapStyleOptions.LoadRawResourceStyle(this, Resource.Raw.mapstyle);
            googleMap.SetMapStyle(mapStyle);
            map = googleMap;
            map.UiSettings.ZoomControlsEnabled = true;
            if (CheckPermissions())
            {
                DisplayLocation();
            }
            
        }

        private bool CheckPermissions()
        {
            bool isGranted = false;
            if (ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != Android.Content.PM.Permission.Granted &&
               ActivityCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != Android.Content.PM.Permission.Granted)
            {
                isGranted = false;
            }
            else isGranted = true;
            return isGranted;
        }

        private async void DisplayLocation()
        {
            if (providerClient == null)
            {
                providerClient = LocationServices.GetFusedLocationProviderClient(this);
            }
            myLastLocation = await providerClient.GetLastLocationAsync();
            if (myLastLocation != null)
            {
                myposition = new LatLng(myLastLocation.Latitude, myLastLocation.Longitude);
                map.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(myposition, 15));
            }
        }
    }
}