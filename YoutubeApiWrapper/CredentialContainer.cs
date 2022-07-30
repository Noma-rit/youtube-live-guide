using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YoutubeApiWrapper
{
    public static class CredentialUtils
    {
        public static async Task<UserCredential> SetFromJsonPathAsync(string secretClientJsonFilePath)
        {
            UserCredential credential;
            using (var stream = new FileStream(secretClientJsonFilePath, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    new[] { YouTubeService.Scope.YoutubeReadonly },
                    "user",
                    CancellationToken.None
                );
            }
            return credential;
        }
    }
}

