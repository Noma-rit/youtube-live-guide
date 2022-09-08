using System;

namespace YoutubeApiWrapper
{
    public class LiveInfo
    {
        public string ChannelName { get; set; }
        public string Title { get; set; }
        public DateTime? StartTime { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
