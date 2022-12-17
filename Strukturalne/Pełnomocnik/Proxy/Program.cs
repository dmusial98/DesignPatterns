namespace Proxy
{
    internal class Program
    {
        // Klasa GUI która dotychczas współpracowała bezpośrednio z
        // obiektem oferującym usługę pozostaje niezmieniona, o ile
        // będzie współpracować z obiektem usługi poprzez interfejs.
        // Możemy śmiało przekazać obiekt pośrednika zamiast obiektu
        // usługi ponieważ implementują one ten sam interfejs.
        static void Main(string[] args)
        {
            YouTubeServer server = new();
            ThirdPartyYoutubeLib normalLib = new(server);
            CachedYouTubeClass cachedLib = new(normalLib, server);

            Console.WriteLine("normal YT library:");
            Console.WriteLine(normalLib.ListVideos());
            for (int a = 0; a < 2; a++)
                for (int i = 0; i < 4; i++)
                {
                    normalLib.GetVideoInfo(i);
                    normalLib.DownloadVideo(i);
                }
            Console.WriteLine("\n\n");

            Console.WriteLine("cached YT library:");
            Console.WriteLine(normalLib.ListVideos());
            for (int a = 0; a < 2; a++)
                for (int i = 0; i < 4; i++)
                {
                    cachedLib.GetVideoInfo(i);
                    cachedLib.DownloadVideo(i);
                }
        }

        public class VideoClip : IEquatable<VideoClip>
        {
            private static int _index = 0;
            public int id { get; }
            public string Name { get; }
            public string AuthorNick { get; }

            public VideoClip(string name, string author)
            {
                id = _index++;
                Name = name;
                AuthorNick = author;
            }

            public override string ToString()
            {
                return $"Video id: {id}, name: {Name}, author: {AuthorNick}\n";
            }

            public override bool Equals(object? obj)
            {
                if (obj is null)
                    return false;
                else
                {
                    VideoClip objAsVideo = obj as VideoClip;
                    return this.Equals(objAsVideo);
                }
            }

            public bool Equals(VideoClip other)
            {
                if (other == null)
                    return false;
                else if (this.id == other.id)
                    return true;

                return false;
            }
        }

        public class YouTubeServer
        {
            public List<VideoClip> _clips = new List<VideoClip>()
            {
              new VideoClip("name1", "author1"),
              new VideoClip("name2", "author2"),
              new VideoClip("name3", "author3"),
              new VideoClip("name4", "author1")
            };

            public List<VideoClip> GetVideos() => _clips;

            public VideoClip GetVideo(int id) => _clips.Where(v => v.id == id).First();
        }

        public interface IThirdPartyYouTubeLib
        {
            public string ListVideos();
            public string GetVideoInfo(int id);
            public VideoClip DownloadVideo(int id);
        }

        // Konkretna implementacja łącza do usługi. Metody tej klasy
        // mogą żądać informacji z YouTube. Szybkość realizacji żądania
        // zależy od połączenia internetowego użytkownika oraz od samego
        // YouTube. Aplikacja będzie działać wolniej, jeśli wiele żądań
        // zostanie wysłanych jednocześnie, nawet jeśli wszystkie
        // żądania dotyczą tych samych danych.
        public class ThirdPartyYoutubeLib : IThirdPartyYouTubeLib
        {
            private YouTubeServer _server;

            public ThirdPartyYoutubeLib(YouTubeServer server) => _server = server;

            public string ListVideos()
            {
                var videos = _server.GetVideos();
                return String.Join(' ', videos);
            }

            public string GetVideoInfo(int id)
            {
                Console.WriteLine($"Get info of video with id {id} from YT server");
                return _server.GetVideo(id).ToString();
            }

            public VideoClip DownloadVideo(int id)
            {
                var video = _server.GetVideo(id);
                Console.WriteLine($"Downloaded video {video.ToString()} from YT server");
                return video;
            }
        }

        // Aby zaoszczędzić nieco przepustowości, możemy stworzyć pamięć
        // podręczną do przechowywania pobranych danych i przechowywać
        // je jakiś czas. Jednak umieszczenie takiego kodu bezpośrednio
        // w klasie usługi może być niemożliwe, jeśli na przykład
        // stanowi część biblioteki od zewnętrznego dostawcy i/lub jest
        // zdefiniowana jako `final`. Dlatego też umieszczamy kod
        // pamięci podręcznej w odrębnej klasie pośredniczącej która
        // implementuje taki sam interfejs co klasa usługi. Nowo
        // powstała klasa deleguje żądania faktycznemu obiektowi usługi
        // tylko wtedy gdy faktycznie trzeba przesłać żądanie przez
        // sieć.
        public class CachedYouTubeClass : IThirdPartyYouTubeLib
        {
            YouTubeServer _server;
            IThirdPartyYouTubeLib _service;
            List<VideoClip> cachedVideosList = new();

            public CachedYouTubeClass(IThirdPartyYouTubeLib s, YouTubeServer server)
            {
                this._service = s;
                this._server = server;
            }

            public string ListVideos()
            {
                var videos = _server.GetVideos();
                return String.Join(' ', videos);
            }

            public string GetVideoInfo(int id)
            {
                if (cachedVideosList.Count != 0 && cachedVideosList.Where(v => v.id == id).FirstOrDefault() != null)
                {
                    Console.WriteLine($"Get info of video with id {id} from cache");
                    return cachedVideosList.Where(v => v.id == id).First().ToString();
                }
                else
                {
                    Console.WriteLine($"Get info of video with id {id} downloaded from YT server");
                    return _server.GetVideo(id).ToString();
                }

            }

            public VideoClip DownloadVideo(int id)
            {
                if (cachedVideosList.Count != 0 && cachedVideosList.Where(v => v.id == id).FirstOrDefault() != null)
                {
                    Console.WriteLine($"Get video with id {id} from cache");
                    return cachedVideosList.Where(v => v.id == id).First();
                }
                else
                {
                    Console.WriteLine($"Get video with id {id} downloaded from YT server");
                    var video = _server.GetVideo(id);
                    cachedVideosList.Add(video);
                    return video;
                }
            }
        }
    }
}