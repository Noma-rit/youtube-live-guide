using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using static Google.Apis.YouTube.v3.SearchResource.ListRequest;
namespace YoutubeApiWrapper
{
    public static class LiveInfoUtils
    {
        public static async Task<List<LiveInfo>> GetLiveInfo(string channelId)
        {
            var upcomingLiveInfo = GetLiveInfo(channelId, EventTypeEnum.Upcoming);
            var onAirLiveInfo = GetLiveInfo(channelId, EventTypeEnum.Live);
            
            return (await upcomingLiveInfo).Concat(await onAirLiveInfo).ToList();
        }

        private static async Task<List<LiveInfo>> GetLiveInfo(string channelId, EventTypeEnum eventType)
        {
            var credential = await CredentialUtils.SetFromJsonPathAsync(ClientSercretJson.FilePath);
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
            });
            var searchRequest = youtubeService.Search.List("snippet");
            searchRequest.ChannelId = channelId;
            searchRequest.EventType = eventType;
            searchRequest.Type = "video";
            var searchResults = await searchRequest.ExecuteAsync();

            var liveInfoList = new List<LiveInfo>();
            foreach(var searchResult in searchResults.Items)
            {
                var liveId = searchResult.Id.VideoId;
                var broadcastRequest = youtubeService.LiveBroadcasts.List("snippet");
                broadcastRequest.Id = liveId;
                var broadcastResult = await broadcastRequest.ExecuteAsync();

                if (broadcastResult.Items.Count == 0) continue;
                var liveInfo = new LiveInfo()
                {
                    ChannelName = searchResult.Snippet.ChannelTitle,
                    Title = broadcastResult.Items[0].Snippet.Title,
                    StartTime = broadcastResult.Items[0].Snippet.ScheduledStartTime,
                    Description = broadcastResult.Items[0].Snippet.Description,
                    ThumbnailUrl = broadcastResult.Items[0].Snippet.Thumbnails.Medium.Url,
                };
                liveInfoList.Add(liveInfo);
            }

            return liveInfoList;
        }
    }
}
