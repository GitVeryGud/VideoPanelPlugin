using LibVLCSharp.Shared;
using LibVLCSharp.WinForms;
using MusicBeePlugin.Saved_Data_Classes;
using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin
{
    public partial class VideoPanel : UserControl
    {
        private LibVLC _libVlc;
        private VideoView _videoView;
        // Not yet implemented, see how to do this shit
        private CancellationToken _cancelation_token;
        public MusicBeeApiInterface mbApiInterface;
        public Control panel;
        public NowLoadingPanel loading_panel;
        private CancellationTokenSource _media_load_cts;
        private bool _debug = false;
        // User Data.
        public UserData user_data;
        // Disposes of the first 2 syncs after the play event, that's because the first 2 syncs have an offset when playing a song with chapters.
        // Now hell if I know why this happens, the API just gives funky values for the song position depending on when or on what situation you call.
        private int _sync_dispose_max = 2;
        private int _sync_dispose = 0;
        private long _last_paused_position = -1;
        private string _current_song_uri = "";
        private VideoType _current_video_type = VideoType.Default;
        // Offset in milliseconds from the start of the song (PlaybackStartTime in MetaDataType).
        private long _start_offset = 0;
        // Here in case I want to add a "sync off" option later on.
        public bool can_sync = true;
        private bool _is_tag_changing = false;

        public static async Task<VideoPanel> CreateVideoPanel(MusicBeeApiInterface beeInterface,Control panel, NowLoadingPanel loading_panel, UserData user_data, CancellationToken cancelation_token)
        {
            var videoPanel = new VideoPanel(beeInterface, panel, loading_panel, user_data, cancelation_token);
            try
            {
                await videoPanel.InitVideoPanelAsync();
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Video panel initialization was canceled.");
            }

            return videoPanel;
        }

        private VideoPanel(MusicBeeApiInterface beeInterface, Control panel, NowLoadingPanel loading_panel, UserData user_data, CancellationToken cancelation_token)
        {
            mbApiInterface = beeInterface;
            this.user_data = user_data;
            this.panel = panel;
            this.loading_panel = loading_panel;
            _cancelation_token = cancelation_token;

            InitializeComponent();
        }

        private async Task InitVideoPanelAsync()
        {
#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            Dock = DockStyle.Fill;

            // Adds loading panel at the front
            panel.Controls.Add(loading_panel);

            Core.Initialize();
            _libVlc = await Task.Run(() => new LibVLC(enableDebugLogs: false));
#if DEBUG
            stopwatch.Stop();
            Console.WriteLine("Time in ms to load libVlc: " + stopwatch.ElapsedMilliseconds);
#endif
            // Stops the initialization it the loading process is canceled
            _cancelation_token.ThrowIfCancellationRequested();

            Utilities.debugPrint("NOT CANCELED");

            // Make VideoView control
            _videoView = new VideoView()
            {
                MediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_libVlc),
                Dock = DockStyle.Fill
            };

            // Add it to the form
            Controls.Add(_videoView);
            panel.Controls.Add(this);

            SetVideo();

            // Very important setup to allow video-audio sync
            SetSyncEvent();

            // Whenever the videos plays, it changes position to music position + _offset_click, generally this is because the user clicked in the position bar
            SetPlayEvent();

            // Event to make non-default VideoTypes repeat infinitely.
            SetEndEvent();
        }

        // Sets video type depending on the saved tag in the file.
        private void SetVideoType() {
            try
            {
                string tag = mbApiInterface.NowPlaying_GetFileTag(MetaDataType.Custom18);
                string kind = mbApiInterface.NowPlaying_GetFileProperty(FilePropertyType.Kind);

                // Audio files default to forcing artwork.
                if (kind.Contains("audio"))
                {
                    _current_video_type = VideoType.ForceArtwork;
                    return;
                }

                // Finds separator (if there is none throws error and goes back to default)
                int index = tag.IndexOf(";");

                if (Enum.TryParse(tag.Substring(0, index), out VideoType type))
                {
                    _current_video_type = type;
                }

                else
                {
                    _current_video_type = VideoType.Default;
                }
            }

            // If nothing is found just sets it to Default.
            catch (Exception ex)
            {
                Utilities.debugPrint("SetVideoType error: " + ex.Message);
                _current_video_type = VideoType.Default;
            }
        }

        // Sets media to be played on the MediaPlayer.
        public void SetVideo()
        {
            var videoUri = mbApiInterface.NowPlaying_GetFileUrl();

            if (videoUri == _current_song_uri)
            {
                Utilities.debugPrint("Didn't set video because uri didn't change");
                return;
            }

            if (videoUri == null)
            {
                // Stops last video from continuing to play (deals with non-default VideoTypes);
                _videoView.MediaPlayer.Stop();
                Utilities.debugPrint("Didn't set video because uri is null");
                return;
            }

            SetVideoType();

            // Requests a cancellation and creates new source for the next token.
            _media_load_cts?.Cancel();
            _media_load_cts?.Dispose();
            _media_load_cts = new CancellationTokenSource();

            // Whenever video changes the offset needs to be reset.
            _start_offset = 0;

            _current_song_uri = videoUri;

            switch (_current_video_type)
            {
                case VideoType.Default:
                    // Calls from the same threadpool as the parent panel.
                    panel.Invoke((MethodInvoker)(async () =>
                    {
                        SetVideoAsync(videoUri);
                    }));
                    break;

                case VideoType.ForceArtwork:
                    videoUri = mbApiInterface.NowPlaying_GetArtworkUrl();

                    if (videoUri == null)
                    {
                        _videoView.MediaPlayer.Stop();
                        return;
                    }

                    // Calls from the same threadpool as the parent panel.
                    panel.Invoke((MethodInvoker)(async () =>
                    {
                        SetVideoAsyncForceArtwork(videoUri);
                    }));
                    break;

                case VideoType.CustomVideo:
                    string tag = mbApiInterface.NowPlaying_GetFileTag(MetaDataType.Custom18);
                    // Finds separator
                    int index = tag.IndexOf(";");

                    // This video type follows the format VideoType;VideoURL in the tag.
                    videoUri = tag.Substring(index + 1);

                    // If null (unlikely) or file doesn't exist.
                    if (videoUri == null || !File.Exists(videoUri))
                    {
                        _videoView.MediaPlayer.Stop();
                        return;
                    }

                    // Calls from the same threadpool as the parent panel.
                    panel.Invoke((MethodInvoker)(async () =>
                    {
                        SetVideoAsyncCustomVideo(videoUri);
                    }));
                    break;
            } 
        }

#if DEBUG
        // For debugging purposes, if there is a need to diferentiate between the async and normal version.
        private void SetVideoNoAsync(string videoUri)
        {
            try
            {
                Console.WriteLine("Normally doing stuff");
                var uri = new Uri(videoUri);
                // Use command line options as Options for media playback (https://wiki.videolan.org/VLC_command-line_help/)
                var media = new Media(_libVlc, uri, "no-audio");
                _videoView.MediaPlayer.Media = media;
                _last_paused_position = -1;

                media.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting video: {ex.Message}");
            }
        }
#endif

        private async void SetVideoAsync(string videoUri)
        {
            // Assigns token to this async method so that even when media_load_cts is disposed, its token can cancel this method.
            var token = _media_load_cts.Token;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            loading_panel.Show();

            try
            {
                string start_time_string = mbApiInterface.NowPlaying_GetFileTag(MetaDataType.PlaybackStartTime);

                long start_time_seconds = Utilities.ParseStartTime(start_time_string);
                // offset is in milliseconds.
                _start_offset = start_time_seconds * 1000;

                var uri = new Uri(videoUri);
                // Use command line options as Options for media playback (https://wiki.videolan.org/VLC_command-line_help/)
                var media = await Task.Run(() => new Media(_libVlc, uri, "no-audio", $"start-time={start_time_seconds}"));
                // Stops media from being inserted on the MediaPlayer if a cancellation request was made before the media finished loading.
                token.ThrowIfCancellationRequested();
                // Stops media from being inserted on the MediaPlayer if MediaPlayer is null (generally from disposing of the plugin panel before loading is done)
                if (_videoView.MediaPlayer == null) throw new ArgumentNullException("_videoView.MediaPlayer cannot be null");
                _videoView.MediaPlayer.Media = media;
                _last_paused_position = -1;
                // In case the panel is created while a song is already playing, so that the video can start.
                if (mbApiInterface.Player_GetPlayState() == PlayState.Playing)
                {
                    Play();
                }

                media.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting video: {ex.Message}");
            }

            finally 
            {
                // Stops the loading screen from being hidden while the user is changing songs but the last song didn't finish loading (eg: was cancelled)
                if (!token.IsCancellationRequested) loading_panel.Hide();

                stopwatch.Stop();
                Utilities.debugPrint("Time in ms to load media: " + stopwatch.ElapsedMilliseconds);
            }
        }

        private async void SetVideoAsyncForceArtwork(string videoUri)
        {
            // Assigns token to this async method so that even when media_load_cts is disposed, its token can cancel this method.
            var token = _media_load_cts.Token;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            loading_panel.Show();

            try
            {
                var uri = new Uri(videoUri);
                // Use command line options as Options for media playback (https://wiki.videolan.org/VLC_command-line_help/)
                var media = await Task.Run(() => new Media(_libVlc, uri, "no-audio"));
                // Stops media from being inserted on the MediaPlayer if a cancellation request was made before the media finished loading.
                token.ThrowIfCancellationRequested();
                // Stops media from being inserted on the MediaPlayer if MediaPlayer is null (generally from disposing of the plugin panel before loading is done)
                if (_videoView.MediaPlayer == null) throw new ArgumentNullException("_videoView.MediaPlayer cannot be null");
                _videoView.MediaPlayer.Media = media;
                // Always play regardless, since it's independant of the track.
                _videoView.MediaPlayer.Play();
                _last_paused_position = -1;

                media.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting video: {ex.Message}");
            }

            finally
            {
                // Stops the loading screen from being hidden while the user is changing songs but the last song didn't finish loading (eg: was cancelled)
                if (!token.IsCancellationRequested) loading_panel.Hide();

                stopwatch.Stop();
                Utilities.debugPrint("Time in ms to load media: " + stopwatch.ElapsedMilliseconds);
            }
        }

        private async void SetVideoAsyncCustomVideo(string videoUri)
        {
            // Assigns token to this async method so that even when media_load_cts is disposed, its token can cancel this method.
            var token = _media_load_cts.Token;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            loading_panel.Show();

            try
            {
                var uri = new Uri(videoUri);
                // Use command line options as Options for media playback (https://wiki.videolan.org/VLC_command-line_help/)
                var media = await Task.Run(() => new Media(_libVlc, uri, "no-audio", "input-repeat=65535"));
                // Stops media from being inserted on the MediaPlayer if a cancellation request was made before the media finished loading.
                token.ThrowIfCancellationRequested();
                // Stops media from being inserted on the MediaPlayer if MediaPlayer is null (generally from disposing of the plugin panel before loading is done)
                if (_videoView.MediaPlayer == null) throw new ArgumentNullException("_videoView.MediaPlayer cannot be null");
                _videoView.MediaPlayer.Media = media;
                _last_paused_position = -1;
                // Always play regardless, since it's independant of the track.
                _videoView.MediaPlayer.Play();

                media.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting video: {ex.Message}");
            }

            finally
            {
                // Stops the loading screen from being hidden while the user is changing songs but the last song didn't finish loading (eg: was cancelled)
                if (!token.IsCancellationRequested) loading_panel.Hide();

                stopwatch.Stop();
                Utilities.debugPrint("Time in ms to load media: " + stopwatch.ElapsedMilliseconds);
            }
        }

        private void SetSyncEvent()
        {
            // Only fires the event when the video time changed, aka: when it's playing.
            // Is already naturally throttled by VLC, fires off about once every 200ms.
            _videoView.MediaPlayer.TimeChanged += (s, e) =>
            {
                // Only sync if can interact with video and the PlayState is playing.
                if (!CannotInteractWithVideo())
                {
                    if (_sync_dispose > 0)
                    {
                        _sync_dispose--;
                        Utilities.debugPrint("Sync disposed");
                        return;
                    }

                    long videotime = e.Time;
                    // Music time is in relation to start time, so we need to readd the offset so that the video
                    // can properly sync.
                    long musicTime = mbApiInterface.Player_GetPosition() + _start_offset;
                    long deviation = videotime - musicTime;

                    Utilities.debugPrint(deviation.ToString(), _debug);

                    // Syncs video to music if the deviation surpasses the constraints.
                    if (deviation > user_data.sync_settings_data.video_delay + user_data.sync_settings_data.constraints 
                    || deviation < user_data.sync_settings_data.video_delay - user_data.sync_settings_data.constraints)
                    {
                        Utilities.debugPrint("Syncing");
                        //Utilities.debugPrint(videotime.ToString());
                        //Utilities.debugPrint(musicTime.ToString());
                        Utilities.debugPrint("pre-sync deviation: " + deviation.ToString());
                        ChangePosition(mbApiInterface.Player_GetPosition(), user_data.sync_settings_data.video_delay);
                    }
                }
            };
        }

        private void SetPlayEvent()
        {
            _videoView.MediaPlayer.Playing += (s, e) =>
            {
                // Just to deal with the track queue changed situation
                if (_sync_dispose < 0) _sync_dispose = 0;
                else _sync_dispose = _sync_dispose_max;

                Utilities.debugPrint("Video Playing click");
                ChangePosition(mbApiInterface.Player_GetPosition(), user_data.sync_settings_data.video_click_delay);
            };
        }

        private void SetEndEvent()
        {
            _videoView.MediaPlayer.EndReached += (sender, args) =>
            {
                // Only do this if the VideoType is non-interactible.
                if (CannotInteractWithVideo())
                {
                    // Called from a threadpool to avoid deadlocks.
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        _videoView.MediaPlayer.Play(_videoView.MediaPlayer.Media); // restart playback
                    });

                    Utilities.debugPrint("loop restart");
                }
            };
        }

        private void ChangePosition(long track_position, int offset)
        {
            // If is close to last position (eg: paused and unpaused) || There's no media || Video type is something else (they generally just loop)
            if (IsOnLastPausedPosition(track_position, 150) || _videoView.MediaPlayer.Media == null || CannotInteractWithVideo()) return;
            _videoView.MediaPlayer.Position = (float)(track_position + offset + _start_offset) / _videoView.MediaPlayer.Media.Duration;
            Utilities.debugPrint("Changed Position");
        }

        public void Play()
        {
            if (CannotInteractWithVideo()) return;

            _videoView.MediaPlayer.Play();
        }

        // Apparently, MediaPlayer.Pause() is a toggle, it does explain in the description, but talk about unintuitive design
        // I wouldn't know about that :^)
        public void PlayToggle()
        {
            _videoView.MediaPlayer.Pause();
        }

        public void Pause()
        {
            if (CannotInteractWithVideo()) return;

            _last_paused_position = mbApiInterface.Player_GetPosition();
            _videoView.MediaPlayer.SetPause(true);
        }

        // Put here all conditions that might make the user unable to interact with the video through pausing and changing position.
        private bool CannotInteractWithVideo()
        {
            return _current_video_type != VideoType.Default;
        }

        // MusicBee saves tags after stop, before the next track loads, so you need to free up the file
        // if you're clicking on the same song, thus allowing the tag to get saved without any sharing issue (most of the time).
        public void Stop()
        {
            // Deals with edge case in which you go from a song to a video (like mkv, that opens a new window and doesn't really interact a lot with MusicBee)
            // Also saves the _last_paused_position for when you double click the song you were already listening to so that the video stutters a bit less.
            if (!CannotInteractWithVideo())
            {
                _videoView.MediaPlayer.Stop();
            }

            Utilities.debugPrint("Stopped");

            if (_videoView.MediaPlayer.Media != null && _is_tag_changing)
            {
                Utilities.debugPrint("Video cleanup on the way");
                _videoView.MediaPlayer.Media.Dispose();
                _videoView.MediaPlayer.Media = null;
                _current_song_uri = "";
            }
        }

        public void End()
        {
            _videoView.MediaPlayer.SetPause(true);
            Utilities.debugPrint("Ended");
            // End of playlist, clear MediaPlayer to avoid video from continuing.
            _last_paused_position = -1;
            _current_song_uri = "";
            _videoView.MediaPlayer.Media.Dispose();
            _videoView.MediaPlayer.Media = null;
        }

        /// <summary>
        /// Checks if the last paused position is "offset" distance away from the current position, from pause to unpause it generally deviates 0-100ms.
        /// </summary>
        /// <returns></returns>
        public bool IsOnLastPausedPosition(long pos, int offset)
        {
            // Can't be negative because pausing through the button never rolls back the position.
            return (pos - _last_paused_position <= offset && pos - _last_paused_position >= 0);
        }

        private void OnDispose(object sender, EventArgs e)
        {
            Console.WriteLine("Dispose Video");
            _media_load_cts?.Dispose();
            _media_load_cts = null;
            _libVlc.Dispose();
            _libVlc = null;
            _videoView.MediaPlayer.Media?.Dispose();
            _videoView.MediaPlayer.Media = null;
            _videoView.MediaPlayer?.Dispose();
            _videoView.MediaPlayer = null;
            _videoView.Dispose();
            loading_panel.Dispose();
            loading_panel = null;
        }

        public Media GetMedia()
        {
            return _videoView.MediaPlayer.Media;
        }

        private void VideoPanel_Load(object sender, EventArgs e)
        {
            Disposed += OnDispose;
        }

        public void ToggleDebug()
        {
            _debug = !_debug;
        }

        public void SetIsTagChanging(bool is_tag_changing)
        {
            _is_tag_changing = is_tag_changing;
        }

        /// <summary>
        /// Only call when you need to make the next play event not dispose of any syncs.
        /// </summary>
        /// <returns></returns>
        public void SyncDisposeNegative()
        {
            _sync_dispose = -_sync_dispose_max;
        }
    }

    public enum VideoType
    {
        Default = 0,
        ForceArtwork = 1,
        CustomVideo = 2
    }
}
