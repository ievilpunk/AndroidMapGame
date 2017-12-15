using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Widget;
using Android.OS;
using Android.Locations;

namespace AndroidMapGame
{
    [Activity(Label = "AndroidMapGame"/*, MainLauncher = true*/)]
    public class MainActivity : Activity, IOnMapReadyCallback, ILocationListener
    {
        private static LatLng current = new LatLng(42.6382576, -73.73495911);
        private static CameraUpdate cameraUpdate;
        private static int zoomLevel = 17;
        private static int tiltLevel = 45;
        private GoogleMap _map;
        private MapFragment _mapFragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            MapInit();
        }

        protected override void OnResume()
        {
            base.OnResume();
            SetupMapIfNeeded();
        }

        private void MapInit()
        {
            _mapFragment = FragmentManager.FindFragmentByTag("map") as MapFragment;
            if (_mapFragment == null)
            {
                GoogleMapOptions mapOptions = new GoogleMapOptions()
                    .InvokeMapType(GoogleMap.MapTypeNormal)
                    .InvokeZoomControlsEnabled(false)
                    .InvokeCompassEnabled(true);

                FragmentTransaction fragTx = FragmentManager.BeginTransaction();
                _mapFragment = MapFragment.NewInstance(mapOptions);
                fragTx.Add(Resource.Id.map, _mapFragment, "map");
                fragTx.Commit();
            }
            _mapFragment.GetMapAsync(this);
        }

        private void SetupMapIfNeeded()
        {
            if (_map != null)
            {
                MarkerOptions markerOpt1 = new MarkerOptions();
                markerOpt1.SetPosition(current);
                markerOpt1.SetTitle("Home");
                markerOpt1.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueCyan));
                _map.AddMarker(markerOpt1);

                // We create an instance of CameraUpdate, and move the map to it.\
                CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
                builder.Target(current);
                builder.Tilt(tiltLevel);
                builder.Zoom(zoomLevel);
                builder.Bearing(155);
                CameraPosition cameraPosition = builder.Build();
                cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);


                //cameraUpdate = CameraUpdateFactory.NewLatLngZoom(current, zoomLevel);

                _map.MoveCamera(cameraUpdate);
            }
        }

        public void OnLocationChanged(Android.Locations.Location location)
        {
             current = new LatLng(location.Latitude, location.Longitude);
             cameraUpdate = CameraUpdateFactory.NewLatLngZoom(current, zoomLevel);
             _map.MoveCamera(cameraUpdate);
        }

        public void OnProviderDisabled(string provider)
        {
            //Log.Debug(tag, provider + " disabled by user");
        }
        public void OnProviderEnabled(string provider)
        {
            //Log.Debug(tag, provider + " enabled by user");
        }
        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            //Log.Debug(tag, provider + " availability has changed to " + status.ToString());
        }

        public void OnMapReady(GoogleMap map)
        {
            _map = map;
            SetupMapIfNeeded();
        }

    }
}

