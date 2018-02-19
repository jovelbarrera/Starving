// Helpers/Settings.cs This file was automatically added when you installed the Settings Plugin. If you are not using a PCL then comment this file back in to use it.
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Starving.Droid.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string CurrentUserKey = "CurrentUserKey";
        private static readonly string CurrentUserDefault = string.Empty;

        private const string RefreshAuthTokenKey = "RefreshAuthTokenKey";
        private static readonly string RefreshAuthTokenDefault = string.Empty;

        private const string AuthTokenKey = "AuthTokenKey";
        private static readonly string AuthTokenDefault = string.Empty;
        #endregion


        public static string CurrentUser
        {
            get
            {
                return AppSettings.GetValueOrDefault(CurrentUserKey, CurrentUserDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(CurrentUserKey, value);
            }
        }

        public static string RefreshAuthToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(RefreshAuthTokenKey, RefreshAuthTokenDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(RefreshAuthTokenKey, value);
            }
        }

        public static string AuthToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(AuthTokenKey, AuthTokenDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AuthTokenKey, value);
            }
        }

    }
}