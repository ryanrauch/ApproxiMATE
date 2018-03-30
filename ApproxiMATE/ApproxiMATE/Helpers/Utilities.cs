using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ApproxiMATE
{
    public static class Utilities
    {
        public static async Task<Position> GetCurrentGeolocationAsync()
        {
            if (!CrossGeolocator.IsSupported || !CrossGeolocator.Current.IsGeolocationEnabled || !CrossGeolocator.Current.IsGeolocationAvailable)
            {
                await Application.Current?.MainPage?.DisplayAlert("Location Fault", "Location Services unable to return current location", "OK");
                return new Position(30, -97);
            }
            return await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(5));
        }
        public static async Task<PermissionStatus> CheckPermissionsAsync(Permission permission)
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
            bool request = false;
            if (permissionStatus == PermissionStatus.Denied)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    String title = String.Format("{0} Permission", permission),
                           question = String.Format("To use this application the {0} permission is required. Please go into Settings and turn on {0} for this app.", permission),
                           positive = "Settings",
                           negative = "Maybe Later";
                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                    if (task == null)
                        return permissionStatus;
                    var result = await task;
                    if (result)
                        CrossPermissions.Current.OpenAppSettings();
                    return permissionStatus;                   
                }
                request = true;
            }
            if (request || permissionStatus != PermissionStatus.Granted)
            {
                var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                if (!newStatus.ContainsKey(permission))
                    return permissionStatus;
                permissionStatus = newStatus[permission];
                if(newStatus[permission] != PermissionStatus.Granted)
                {
                    permissionStatus = newStatus[permission];
                    String title = String.Format("{0} Permission", permission),
                           question = String.Format("To use this application the {0} permission is required.", permission),
                           positive = "Settings",
                           negative = "Maybe Later";
                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                    var result = await task;
                    if (result)
                        CrossPermissions.Current.OpenAppSettings();
                    return permissionStatus;
                }
            }
            return permissionStatus;
        }
    }
}
