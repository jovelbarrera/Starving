// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Starving.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings {
			get {
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private const string SettingsKey = "settings_key";
		private static readonly string SettingsDefault = string.Empty;

		private const string FacebookTokenStringKey = "facebook_key";
		private static readonly string FacebookTokenStringDefault = null;

		private const string FacebookUserIdKey = "facebook_key";
		private static readonly string FacebookUserIdDefault = null;

		#endregion


		public static string GeneralSettings {
			get {
				return AppSettings.GetValueOrDefault (SettingsKey, SettingsDefault);
			}
			set {
				AppSettings.AddOrUpdateValue (SettingsKey, value);
			}
		}

		public static string FacebookTokenString {
			get {
				return AppSettings.GetValueOrDefault (FacebookTokenStringKey, FacebookTokenStringDefault);
			}
			set {
				AppSettings.AddOrUpdateValue (FacebookTokenStringKey, value);
			}
		}

		public static string FacebookUserId {
			get {
				return AppSettings.GetValueOrDefault (FacebookUserIdKey, FacebookUserIdDefault);
			}
			set {
				AppSettings.AddOrUpdateValue (FacebookUserIdKey, value);
			}
		}

	}
}