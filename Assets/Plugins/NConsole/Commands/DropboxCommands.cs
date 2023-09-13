using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Olimpik.NConsole.Attributes;
using UnityEngine;

namespace Olimpik.NConsole.Commands
{
    public class DropboxCommands
    {
        //example setting
        private const string appKey = "";
        private const string appSecret = "";

        [NConsoleMethod("dropbox_open_auth_token", "Open url link for get AuthCode")]
        [NConsoleReturn("url")]
        public static string OpenAuthLink()
        {
            var url = $"https://www.dropbox.com/oauth2/authorize?client_id={appKey}&response_type=code&token_access_type=offline";
            Application.OpenURL(url);
            return url;
        }

        [NConsoleMethod("dropbox_get_refresh_token", "Generate refresh token from AppKey, AppSecret and AuthCode")]
        [NConsoleArg("AuthCode", "")]
        [NConsoleReturn("RefreshToken (paste to plist)")]
        public static string GetRefreshToken(string argument)
        {
            var base64Authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{appKey}:{appSecret}"));

            var web = new WebClient();
            web.Headers.Add("Authorization", $"Basic {base64Authorization}");

            var uriBuilder = new UriBuilder("https://api.dropbox.com/oauth2/token");
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["code"] = argument;
            query["grant_type"] = "authorization_code";
            uriBuilder.Query = query.ToString();

            string response = null;

            try
            {
                response = web.UploadString(uriBuilder.Uri, string.Empty);
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    var code = (int)((HttpWebResponse)e.Response).StatusCode;
                    using var reader = new StreamReader(e.Response.GetResponseStream());
                    response = $"Error code: {code}\n{reader.ReadToEnd()}";
                }
            }

            return response;
        }
    }
}
