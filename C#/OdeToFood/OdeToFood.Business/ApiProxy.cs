using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Newtonsoft.Json;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Business
{
    public class ApiProxy : IApiProxy
    {
        private HttpClient _client { get;}

        public ApiProxy(string baseUrl)
        {
            _client = new HttpClient { BaseAddress = new Uri(baseUrl)};
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private class User
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
        }

        public void RegisterAsDummyUserAndUseBearerToken()
        {
            var dummyUser = new User
            {
                Email = "username@test.be",
                Password = "password",
                ConfirmPassword = "password"
                
            };

            var registerUrl = "api/account/register";
            var response = _client.PostAsJsonAsync(registerUrl, dummyUser).Result;
            var token = GetToken(dummyUser.Email, dummyUser.Password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private string GetToken(string username, string password)
        {
            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type","password"),
                new KeyValuePair<string, string>("username",username),
                new KeyValuePair<string, string>("password",password)
            };

            var content = new FormUrlEncodedContent(pairs);

            var response = _client.PostAsync("/Token", content).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;

            var tokenDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
            return tokenDictionary["access_token"];
        }

        public async Task<IEnumerable<Review>> GetReviewsAsync()
        {
            var reviewsUrl = "api/Reviews";

            var response = await _client.GetAsync(reviewsUrl);
            if (response.IsSuccessStatusCode)
            {
                var reviews = await response.Content.ReadAsAsync<IEnumerable<Review>>();
                return reviews;
            }
            return null;
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(int id)
        {
            var restaurantUrl = $"api/Restaurants/{id}";

            var response = await _client.GetAsync(restaurantUrl);
            if (response.IsSuccessStatusCode)
            {
                var restaurant = await response.Content.ReadAsAsync<Restaurant>();
                return restaurant;
            }
            return null;
        }

        public async Task<Review> GetReviewByIdAsync(int id)
        {
            var reviewUrl = $"api/Reviews/{id}";

            var response = await _client.GetAsync(reviewUrl);
            if (response.IsSuccessStatusCode)
            {
                var review = await response.Content.ReadAsAsync<Review>();
                return review;
            }
            return null;

        }

        public async Task<bool> PutReviewAsync(int id, Review review)
        {
            var reviewUrl = $"api/Reviews/{id}";

            var response = await _client.PutAsJsonAsync(reviewUrl, review);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PostReviewAsync(Review review)
        {
            var reviewUrl = $"api/Reviews";

            var response = await _client.PostAsJsonAsync(reviewUrl, review);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteReviewAsync(int id)
        { 
            var reviewUrl = $"api/Reviews/{id}";

            var response = await _client.DeleteAsync(reviewUrl);
            return response.IsSuccessStatusCode;
        }
    }
}
