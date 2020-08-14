using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Steam_Match_Machine.Models.API;

namespace Steam_Match_Machine.Models {
    public class SteamApi {
        private readonly string _steamApiUrl;
        private readonly string _accessToken;
        private readonly string _apiUrlSegment;

        public SteamApi (string steamApiUrl, string apiUrlSegment, string accessToken) {
            _steamApiUrl = steamApiUrl;
            _accessToken = accessToken;
            _apiUrlSegment = apiUrlSegment;
        }

        public FeaturedGameResponse GetFeaturedGames () {
            return CallApi<FeaturedGameResponse> (RequestType.Get, "featured");
        }

        public FeaturedCategoriesResponseWrapper GetFeaturedCategories () {
            return CallApi<FeaturedCategoriesResponseWrapper> (RequestType.Get, "featuredcategories");
        }

        public GameDetailsResponse GetGameDetails (int id) {
            Dictionary<string, GameDetailsResponse> response = CallApi<Dictionary<string, GameDetailsResponse>> (RequestType.Get, $"appdetails?appids={id}");
            return response.GetValueOrDefault (id.ToString ());
        }

        public VideoGame SetVideoGameDetails(VideoGame videoGame) {
            GameDetailsResponse gameDetailsResponse = GetGameDetails (videoGame.steam_appid);

                videoGame.name = gameDetailsResponse.data.name;

                videoGame.header_image = gameDetailsResponse.data.header_image;

                videoGame.short_description = gameDetailsResponse.data.short_description;

                videoGame.thumbnail_img = $"https://steamcdn-a.akamaihd.net//steam//apps//{videoGame.steam_appid}//capsule_184x69.jpg";

                try {
                    videoGame.final_formatted = gameDetailsResponse.data.price_overview.final_formatted;
                } catch {
                    if (videoGame.final_formatted == null) {
                        videoGame.final_formatted = "0.00";
                    }
                }

                videoGame.ProductLink = $"https://store.steampowered.com/app/{videoGame.steam_appid}";

                return videoGame;
        }

        public List<VideoGame> SetVideoGameDetails (List<VideoGame> model) {
            model?.ForEach (videoGame => {
                GameDetailsResponse gameDetailsResponse = GetGameDetails (videoGame.steam_appid);

                videoGame.name = gameDetailsResponse.data.name;

                videoGame.header_image = gameDetailsResponse.data.header_image;

                videoGame.short_description = gameDetailsResponse.data.short_description;

                videoGame.thumbnail_img = $"https://steamcdn-a.akamaihd.net//steam//apps//{videoGame.steam_appid}//capsule_184x69.jpg";

                try {
                    videoGame.final_formatted = gameDetailsResponse.data.price_overview.final_formatted;
                } catch {
                    if (videoGame.final_formatted == null) {
                        videoGame.final_formatted = "0.00";
                    }
                }

                videoGame.ProductLink = $"https://store.steampowered.com/app/{videoGame.steam_appid}";
            });

            return model;
        }

        private T CallApi<T> (RequestType requestType, string route, List<KeyValuePair<string, string>> parameters = null) {
            T result = default (T);

            HttpClientHandler handler = new HttpClientHandler ();
            handler.AllowAutoRedirect = false;
            using (HttpClient client = new HttpClient (handler, true)) {
                client.BaseAddress = new Uri (_steamApiUrl);
                client.DefaultRequestHeaders.Accept.Clear ();
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bearer", _accessToken);
                client.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));

                HttpResponseMessage response = null;

                switch (Enum.GetName (typeof (RequestType), requestType).ToUpper ()) {
                    case "GET":
                        {
                            if (parameters != null) {
                                if (!route.EndsWith ("?") && !route.EndsWith ("&")) {
                                    route += "?";
                                }

                                foreach (KeyValuePair<string, string> parameter in parameters) {
                                    route += $"{parameter.Key}={parameter.Value}&";
                                }
                            }

                            response = client.GetAsync ($"{_apiUrlSegment}/{route}").Result;
                            break;
                        }
                    case "POST":
                        {
                            FormUrlEncodedContent content = null;
                            if (parameters != null) {
                                content = new FormUrlEncodedContent (parameters);
                            }

                            response = client.PostAsync ($"{_apiUrlSegment}/{route}", content).Result;
                            break;
                        }
                    case "PUT":
                        {
                            FormUrlEncodedContent content = null;
                            if (parameters != null) {
                                content = new FormUrlEncodedContent (parameters);
                            }

                            response = client.PutAsync ($"{_apiUrlSegment}/{route}", content).Result;
                            break;
                        }
                    case "DELETE":
                        {
                            if (parameters != null) {
                                if (!route.EndsWith ("?") && !route.EndsWith ("&")) {
                                    route += "?";
                                }

                                foreach (KeyValuePair<string, string> parameter in parameters) {
                                    route += $"{parameter.Key}={parameter.Value}&";
                                }
                            }

                            response = client.DeleteAsync ($"{_apiUrlSegment}/{route}").Result;
                            break;
                        }
                }

                //This method throws an exception if the HTTP response status is an error code. 
                response.EnsureSuccessStatusCode ();

                result = response.Content.ReadAsAsync<T> ().Result;
            }

            return result;
        }
    }
}