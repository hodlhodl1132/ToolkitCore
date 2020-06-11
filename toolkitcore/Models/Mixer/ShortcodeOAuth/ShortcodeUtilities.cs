using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ToolkitCore.Models.Mixer.ShortcodeOAuth
{
    public static class ShortcodeUtilities
    {
        internal static OAuthShortcodeResponse OAuthShortcodeResponse { get; set; }

        internal static OAuthShortcodeCheckResponse OAuthShortcodeCheckResponse { get; set; }

        internal static OAuthTokenResponse OAuthTokenResponse { get; set; }

        public static async Task<bool> GetShortcode()
        {
            try
            {
                WebClient webClient = new WebClient();
                OAuthShortcodeRequest request = new OAuthShortcodeRequest();
                string jsonRequest = JsonConvert.SerializeObject(request);
                string jsonResponse = await webClient.UploadStringTaskAsync($"{MixerWrapper.MixerApiBaseUrl}oauth/shortcode", jsonRequest);
                OAuthShortcodeResponse response = JsonConvert.DeserializeObject<OAuthShortcodeResponse>(jsonResponse);
                if (response.code != null)
                {
                    OAuthShortcodeResponse = response;
                    return true;
                }
            }
            catch (WebException e)
            {
                Log.Error($"Error getting shortcode. {e.Message}");
            }

            return false;
        }

        public static async Task<bool> CheckShortcode()
        {
            Log.Message("Verifying oauth Shortcode");

            try
            {
                WebClient webClient = new WebClient();
                string jsonResponse = await webClient.DownloadStringTaskAsync($"{MixerWrapper.MixerApiBaseUrl}oauth/shortcode/check/{OAuthShortcodeResponse.handle}");

                if (jsonResponse == string.Empty)
                {
                    return false;
                }

                OAuthShortcodeCheckResponse response = JsonConvert.DeserializeObject<OAuthShortcodeCheckResponse>(jsonResponse);
                if (response.code != null)
                {
                    OAuthShortcodeCheckResponse = response;
                    return true;
                }
            }
            catch (WebException e)
            {
                Log.Error($"Error checking shortcode. {e.Message}");
            }

            return false;
        }

        public static async Task<bool> GetOAuthToken()
        {
            try
            {
                WebClient webClient = new WebClient();
                OAuthTokenRequest request = new OAuthTokenRequest(false, OAuthShortcodeCheckResponse.code);
                string jsonRequest = JsonConvert.SerializeObject(request, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                string jsonResponse = await webClient.UploadStringTaskAsync($"{MixerWrapper.MixerApiBaseUrl}oauth/token", jsonRequest);
                OAuthTokenResponse response = JsonConvert.DeserializeObject<OAuthTokenResponse>(jsonResponse);
                if (response.access_token != null && response.refresh_token != null)
                {
                    OAuthTokenResponse = response;
                    return true;
                }                
            }
            catch (WebException e)
            {
                Log.Error($"Error retrieving OAuth Token. {e.Message}");
            }

            return false;
        }
    }
}
