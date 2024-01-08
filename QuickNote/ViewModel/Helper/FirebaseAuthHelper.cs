using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using QuickNote.Model;
using QuickNote.ViewModel.Commands;

namespace QuickNote.ViewModel.Helper
{
    public class FirebaseAuthHelper
    {
        private static string FirebaseKeyApi ="AIzaSyCMs5BPw5adCRHFb3RKLoY6rVUtYYnAyU8";

        public static async Task<bool> Register(User user)
        {
            using(HttpClient client = new HttpClient())
            {
                var body = new
                {
                    email = user.UserName,
                    password = user.Password,
                    returnSecureToken = true,
                };
                var jsonObj = JsonConvert.SerializeObject(body);
                var data = new StringContent(jsonObj,Encoding.UTF8,"application/json");
                var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signUp?key={FirebaseKeyApi}", data);
                if(response.IsSuccessStatusCode)
                {
                    string resultJson = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<FirebaseResult>(resultJson);
                    App.UserId = result.localId;
                    return true;
                }
                else
                {
                    string errorJson = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<FirebaseErrorResult>(errorJson);
                    MessageBox.Show(result.error.message);
                    return false;
                }
            }
        }
        public static async Task<bool> Login(User user)
        {
            using (HttpClient client = new HttpClient())
            {
                var body = new
                {
                    email = user.UserName,
                    password = user.Password,
                    returnSecureToken = true,
                };
                var jsonObj = JsonConvert.SerializeObject(body);
                var data = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={FirebaseKeyApi}", data);
                if (response.IsSuccessStatusCode)
                {
                    string resultJson = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<FirebaseResult>(resultJson);
                    App.UserId = result.localId;
                    return true;
                }
                else
                {
                    string errorJson = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<FirebaseErrorResult>(errorJson);
                    MessageBox.Show(result.error.message);
                    return false;
                }
            }
        }
        public class FirebaseResult
        {
            public string idToken { get; set; }
            public string email { get; set; }
            public string refreshToken { get; set; }
            public string expiresIn { get; set; }
            public string localId { get; set; }
        }
        public class Error
        {
            public int code { get; set; }
            public string message { get; set; }
        }


        public class FirebaseErrorResult
        {
            public Error error { get; set; }
        }

    }
}
