using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YoutubeApiWrapper
{
    public static class SubscriptionUtils
    {
        public async static Task<string[]> GetMySubscriptionChannelIds()
        {
            var credential = await CredentialUtils.SetFromJsonPathAsync(ClientSercretJson.FilePath);
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
            });
            var subscriptionListRequest = youtubeService.Subscriptions.List("snippet");
            subscriptionListRequest.MaxResults = 50;
            subscriptionListRequest.Mine = true;

            var subscriptionList = new HashSet<string>();
            SubscriptionListResponse list;
            do
            {
                list = await subscriptionListRequest.ExecuteAsync();
                subscriptionList = subscriptionList.Concat(list.Items.Select(x => x.Snippet.ChannelId)).ToHashSet();
                subscriptionListRequest.PageToken = list.NextPageToken;
            } while (list.NextPageToken != null);

            return subscriptionList.ToArray();
        }
    }
}

