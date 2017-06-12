using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Business
{
    public class ApiProxy : IApiProxy
    {
        private HttpClient _client;

        public ApiProxy(string baseUrl)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        class User
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
                Password = "P@ssw0rd11",
                ConfirmPassword = "P@ssw0rd11"
            };

            var response = _client.PostAsJsonAsync("/api/account/register", dummyUser).Result;
            var token = GetToken(dummyUser.Email, dummyUser.Password);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
        }

        private string GetToken(string username, string password)
        {
            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type","password"),
                new KeyValuePair<string, string>("username",username),
                new KeyValuePair<string, string>("Password",password)
            };

            var content = new FormUrlEncodedContent(pairs);
            var response = _client.PostAsync("/Token", content).Result;

            var tokenDictionary = response.Content.ReadAsAsync<Dictionary<string, string>>().Result;
            return tokenDictionary["access_token"];
        }

        public async Task<IEnumerable<Review>> GetReviewsAsync()
        {
            var response = await _client.GetAsync("/api/Reviews");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<IEnumerable<Review>>();
                return result;
            }
            return null;
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(int id)
        {
            var response = await _client.GetAsync($"/api/Restaurants/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<Restaurant>();
                return result;
            }
            return null;
        }

        public async Task<Review> GetReviewByIdAsync(int id)
        {
            var response = await _client.GetAsync($"/api/Reviews/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsAsync<Review>();
                return result;
            }
            return null;
        }

        public async Task<bool> PutReviewAsync(int id, Review review)
        {
            var response = await _client.PutAsJsonAsync($"/api/Reviews/{id}",review);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PostReviewAsync(Review review)
        {
            var response = await _client.PostAsJsonAsync("/api/Reviews", review);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteReviewAsync(int id)
        {
            var response = await _client.DeleteAsync($"/api/Reviews/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
