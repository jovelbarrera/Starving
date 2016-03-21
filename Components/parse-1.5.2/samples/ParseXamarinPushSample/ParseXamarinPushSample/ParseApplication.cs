using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Parse;

namespace ParseXamarinPushSample {
  [Application(Name = "parsexamarinpushsample.ParseApplication")]
  class ParseApplication : Application {
    public ParseApplication(IntPtr handle, JniHandleOwnership ownerShip)
      : base(handle, ownerShip) {
    }

    public override void OnCreate() {
      base.OnCreate();

      // PARSE INTERNAL
      // To properly scrub this project for public release, the following changes must be made:
      // 1. This region should be replaced with a template Parse.Initialize call.
      // 2. The project must stop requesting intranet permission.
      // 3. The project may not access Parse.snk, or it will retain access to Parse internals.
      // 4. We must verify that Parse.snk is never copied to this project directory by msbuild.
      //ParseClient.HostName = new Uri("http://parse-local:3000/")
      ParseClient.Initialize("ZIqbEBf3PXXSzpbQbvz5BcmVcK54DjSmKwNxCah1", "EY4UfiJn0xGhvgvNhBRrY6kggn4nv9zGGk5klQQo");
      // END PARSE INTERNAL
      ParsePush.ParsePushNotificationReceived += ParsePush.DefaultParsePushNotificationReceivedHandler;
    }
  }
}