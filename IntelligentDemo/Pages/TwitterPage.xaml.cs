using IntelligentDemo.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Tweetinvi;
using Tweetinvi.Events;
using Tweetinvi.Models;

namespace IntelligentDemo.Pages
{
    /// <summary>
    /// Interaction logic for TwitterPage.xaml
    /// </summary>
    public partial class TwitterPage : UserControl
    {
        private const string HASHTAG = "#demotest";

        private static readonly Dictionary<string, IEnumerable<NoteCommand>> _percussionLines = InitializePercussionLines();
        private EmotionService _emotionService = new EmotionService();
        private SongController _controller;
        private int? _currentIndex;
        private int? _nextIndex;

        public TwitterPage(SongController controller)
        {
            Auth.SetUserCredentials(App.Secrets.Twitter.ConsumerKey, App.Secrets.Twitter.ConsumerSecret, App.Secrets.Twitter.UserAccessToken, App.Secrets.Twitter.UserAccessSecret);

            InitializeComponent();
            TweetList.ItemsSource = Tweets;
            _controller = controller;
            controller.BarStarted += Controller_BarStarted;
        }

        public ObservableCollection<Tweet> Tweets = new ObservableCollection<Tweet>();

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadRecentTweets();
            await ConnectToTwitterStream();
        }

        private async Task LoadRecentTweets()
        {
            var search = Search.SearchTweets(HASHTAG);
            foreach (var tweet in search)
            {
                await Process(tweet);
            }
        }

        private async Task ConnectToTwitterStream()
        {
            var stream = Stream.CreateFilteredStream();
            stream.AddTrack(HASHTAG);
            stream.MatchingTweetReceived += async (s, e) => await Process(e.Tweet);
            await stream.StartStreamMatchingAllConditionsAsync();
        }

        private async Task Process(ITweet tweet)
        {
            if (tweet.Media.Count != 0 && tweet.Media[0].MediaType == "photo" && tweet.IsRetweet == false && tweet.InReplyToUserId == null)
            {
                var imgUrl = tweet.Entities.Medias[0].MediaURL;
                var emotion = await _emotionService.DetectEmotionFromUrl(imgUrl);
                var result = new Tweet
                {
                    ImageUrl = imgUrl,
                    Emotion = emotion,
                    Text = tweet.Text
                };

                Current.Dispatcher.Invoke(() => Tweets.Add(result));
            }
        }

        private void Controller_BarStarted(object sender, BarStartedEventArgs e)
        {
            if (e.BarNumber % 4 == 1)
            {
                if (_nextIndex != null)
                {
                    Current.DataContext = Tweets[_nextIndex.Value];
                    _currentIndex = _nextIndex;
                    _nextIndex = null;
                }
            }

            if (e.BarNumber % 4 == 0)
            {
                var next = _currentIndex.HasValue ?
                    Tweets.Skip(_currentIndex.Value + 1).Where(i => _percussionLines.ContainsKey(i.Emotion)).FirstOrDefault()
                        ?? Tweets.Take(_currentIndex.Value).Where(i => _percussionLines.ContainsKey(i.Emotion)).FirstOrDefault()
                        ?? Tweets.Where(i => _percussionLines.ContainsKey(i.Emotion)).FirstOrDefault()
                    : Tweets.Where(i => _percussionLines.ContainsKey(i.Emotion)).FirstOrDefault();

                if (next != null)
                {
                    _nextIndex = Tweets.IndexOf(next);
                    _controller.SetNextPercussionBar(_percussionLines[next.Emotion]);
                }
            }
        }

        private static Dictionary<string, IEnumerable<NoteCommand>> InitializePercussionLines()
        {
            var result = new Dictionary<string, IEnumerable<NoteCommand>>();

            result["anger"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 36, Duration = 16, Velocity = 127, Position = 1},
            };

            result["contempt"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 100, Position = 5},
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 127, Position = 9},
                new NoteCommand{ Note = 36, Duration = 4, Velocity = 100, Position = 13},
            };

            result["disgust"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 40, Duration = 4, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 38, Duration = 4, Velocity = 127, Position = 5},
                new NoteCommand{ Note = 36, Duration = 8, Velocity = 127, Position = 9},
            };

            result["fear"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 1},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 3},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 5},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 7},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 9},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 11},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 13},
                new NoteCommand{ Note = 48, Duration = 2, Velocity = 100, Position = 15},
            };

            result["happiness"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 48, Duration = 4, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 50, Duration = 4, Velocity = 127, Position = 5},
                new NoteCommand{ Note = 48, Duration = 4, Velocity = 127, Position = 9},
                new NoteCommand{ Note = 50, Duration = 4, Velocity = 127, Position = 13},
            };

            result["neutral"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 48, Duration = 8, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 48, Duration = 8, Velocity = 127, Position = 9},
            };

            result["sadness"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 38, Duration = 8, Velocity = 100, Position = 1},
                new NoteCommand{ Note = 36, Duration = 8, Velocity = 100, Position = 9},
            };

            result["surprise"] = new List<NoteCommand>
            {
                new NoteCommand{ Note = 38, Duration = 4, Velocity = 127, Position = 1},
                new NoteCommand{ Note = 38, Duration = 4, Velocity = 127, Position = 5},
                new NoteCommand{ Note = 60, Duration = 8, Velocity = 127, Position = 9},
            };

            return result;
        }

        public class Tweet
        {
            public string ImageUrl { get; set; }
            public string Emotion { get; set; }
            public string Text { get; set; }
        }
    }
}
