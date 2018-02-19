using System;
using System.Threading.Tasks;
using Firebase.Xamarin.Database;
using Firebase.Xamarin.Database.Query;
using Starving.Models;
using Xamarin.Forms;

namespace Starving.Services
{
    public class FirebaseService
    {
        private static FirebaseService _instance;

        public static FirebaseService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FirebaseService();
                }
                return _instance;
            }
        }

        public FirebaseClient _firebase
        {
            get
            {
                return new FirebaseClient(Application.Current.Resources["FirebaseBaseUrl"].ToString());
            }
        }

        public static string Token
        {
            get
            {
                return "";
            }
        }

        public async Task Read()
        {
            var items = await _firebase
                .Child("review")
                .OrderByKey()
                .WithAuth(Token)
                .LimitToFirst(2)
                .OnceAsync<Review>();

            foreach (var item in items)
            {
                Console.WriteLine($"{item.Key} name is {item.Object.Comment}");
            }
        }

        public async Task Write()
        {
            var item = await _firebase
                .Child("review")
                .WithAuth(Token)
                .PostAsync(new Review { Comment = "test" });

            // note that there is another overload for the PostAsync method which delegates the new key generation to the client

            Console.WriteLine($"Key for the new item: {item.Key}");

            //// add new item directly to the specified location (this will overwrite whatever data already exists at that location)
            //var item = await firebase
            //.Child("yourentity")
            //.Child("Ricardo")
            ////.WithAuth("<Authentication Token>") // <-- Add Auth token if required. Auth instructions further down in readme.
            //.PutAsync(new YourObject());
        }
    }
}