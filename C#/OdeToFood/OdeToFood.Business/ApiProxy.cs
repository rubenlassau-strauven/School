using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Business
{
    public class ApiProxy : IApiProxy
    {
        private HttpClient _httpClient { get;}

        public ApiProxy(string baseUrl)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl)};
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
            var response = _httpClient.PostAsJsonAsync(registerUrl, dummyUser).Result;
            var token = GetToken(dummyUser.Email, dummyUser.Password);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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

            var response = _httpClient.PostAsync("/Token", content).Result;
            var jsonString = response.Content.ReadAsStringAsync().Result;

            var tokenDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
            return tokenDictionary["access_token"];
        }

        public async Task<IEnumerable<Review>> GetReviewsAsync()
        {
            var reviewsUrl = "api/Reviews";

            var response = await _httpClient.GetAsync(reviewsUrl);
            if (response.IsSuccessStatusCode)
            {
                var reviews = await response.Content.ReadAsAsync<IEnumerable<Review>>();
                return reviews;
            }
            //return null;
            throw new HttpException(response.ReasonPhrase);
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(int id)
        {
            var restaurantUrl = $"api/Restaurants/{id}";

            var response = await _httpClient.GetAsync(restaurantUrl);
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

            var response = await _httpClient.GetAsync(reviewUrl);
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

            var response = await _httpClient.PutAsJsonAsync(reviewUrl, review);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PostReviewAsync(Review review)
        {
            var reviewUrl = $"api/Reviews";

            var response = await _httpClient.PostAsJsonAsync(reviewUrl, review);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteReviewAsync(int id)
        { 
            var reviewUrl = $"api/Reviews/{id}";

            var response = await _httpClient.DeleteAsync(reviewUrl);
            return response.IsSuccessStatusCode;
        }
    }
}
