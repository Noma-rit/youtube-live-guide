using System;
using System.IO;
namespace YoutubeApiWrapper
{
    public static class ClientSercretJson
    {
        public static string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "client_secrets.json");
    }
}

