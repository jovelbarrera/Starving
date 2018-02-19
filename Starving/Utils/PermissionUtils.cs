using System;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Services;

namespace Starving.Utils
{
    public static class PermissionUtils
    {
        public static async Task<bool> ManagePermissions(IPageDialogService dialogService, Permission permission, string reason)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
            if (status != PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(permission))
                {
                    //TODO use resources rather than strings
                    await dialogService.DisplayAlertAsync("Permission Needed", reason, "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                //Best practice to always check that the key exists
                if (results.ContainsKey(permission))
                    status = results[permission];
            }

            if (status == PermissionStatus.Granted)
            {
                return true;
            }
            else if (status != PermissionStatus.Unknown)
            {
                //TODO use resources rather than strings
                await dialogService.DisplayAlertAsync("Permission Denied", "Can not continue, try again.", "OK");
            }
            return false;
        }
    }
}
