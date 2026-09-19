using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using MusicBeePlugin.Saved_Data_Classes;

namespace MusicBeePlugin
{
    public partial class Plugin
    {
        private MusicBeeApiInterface mbApiInterface;
        private PluginInfo about = new PluginInfo();
        public VideoPanel video_panel = null;
        public bool is_tag_changing = false;
        public CancellationTokenSource cts;
        public SyncSettingsForm sync_form;
        public UserData user_data;

        public PluginInfo Initialise(IntPtr apiInterfacePtr)
        {
            mbApiInterface = new MusicBeeApiInterface();
            mbApiInterface.Initialise(apiInterfacePtr);
            about.PluginInfoVersion = PluginInfoVersion;
            about.Name = "Video Panel";
            about.Description = "Embeds a VLC video player on music bee";
            about.Author = "Nermon";
            about.TargetApplication = "Video Panel";   //  the name of a Plugin Storage device or panel header for a dockable panel
            about.Type = PluginType.VideoPlayer;
            about.VersionMajor = 1;  // your plugin version
            about.VersionMinor = 0;
            about.Revision = 0;
            about.MinInterfaceVersion = MinInterfaceVersion;
            about.MinApiRevision = MinApiRevision;
            about.ReceiveNotifications = (ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.TagEvents);
            about.ConfigurationPanelHeight = 0;   // height in pixels that musicbee should reserve in a panel for config settings. When set, a handle to an empty panel will be passed to the Configure function     
            user_data = new UserData(mbApiInterface.Setting_GetPersistentStoragePath());  
            return about;
        }

        public bool Configure(IntPtr panelHandle)
        {
            // save any persistent settings in a sub-folder of this path
            string dataPath = mbApiInterface.Setting_GetPersistentStoragePath();
            // panelHandle will only be set if you set about.ConfigurationPanelHeight to a non-zero value
            // keep in mind the panel width is scaled according to the font the user has selected
            // if about.ConfigurationPanelHeight is set to 0, you can display your own popup window
            if (panelHandle != IntPtr.Zero)
            {
                Panel configPanel = (Panel)Panel.FromHandle(panelHandle);
                Label prompt = new Label();
                prompt.AutoSize = true;
                prompt.Location = new Point(0, 0);
                prompt.Text = "prompt:";
                TextBox textBox = new TextBox();
                textBox.Bounds = new Rectangle(60, 0, 100, textBox.Height);
                configPanel.Controls.AddRange(new Control[] { prompt, textBox });
            }
            return false;
        }

        // called by MusicBee when the user clicks Apply or Save in the MusicBee Preferences screen.
        // its up to you to figure out whether anything has changed and needs updating
        //public void SaveSettings()
        //{
        //    // save any persistent settings in a sub-folder of this path
        //    string dataPath = mbApiInterface.Setting_GetPersistentStoragePath();
        //}

        // MusicBee is closing the plugin (plugin is being disabled by user or MusicBee is shutting down)
        public void Close(PluginCloseReason reason)
        {
            video_panel?.panel.Dispose();
            video_panel = null;
            sync_form?.Dispose();
            sync_form = null;
        }

        // uninstall this plugin - clean up any persisted files
        public void Uninstall()
        {
            video_panel?.panel.Dispose();
            video_panel = null;
            sync_form?.Dispose();
            sync_form = null;


            string directoryPath = Path.Combine(mbApiInterface.Setting_GetPersistentStoragePath(), "mb_VideoPanel");
            string dataPath = Path.Combine(directoryPath, "data.json");

            if (File.Exists(dataPath)) File.Delete(dataPath);
            if (Directory.Exists(directoryPath)) Directory.Delete(directoryPath);
        }

        // receive event notifications from MusicBee
        // you need to set about.ReceiveNotificationFlags = PlayerEvents to receive all notifications, and not just the startup event
        public void ReceiveNotification(string sourceFileUrl, NotificationType type)
        {
            Utilities.debugPrint(type + ": " + mbApiInterface.Player_GetPlayState(), type.ToString(), type == NotificationType.PlayStateChanged);

            // perform some action depending on the notification type
            switch (type)
            {
                case NotificationType.PluginStartup:
                    cts = new CancellationTokenSource();
                    break;
                case NotificationType.TrackChanging:
                    break;
                case NotificationType.TrackChanged:
                    var uri = mbApiInterface.NowPlaying_GetFileUrl();
                    video_panel?.SetVideo(uri);
                    break;
                case NotificationType.PlayingTracksQueueChanged:
                    // This notification only plays if a track changed normally (the next song in the playlist was autoplayed).
                    video_panel?.SyncDisposeNegative();
                    break;
                case NotificationType.TagsChanging:
                    is_tag_changing = true;
                    break;
                case NotificationType.TagsChanged:
                    is_tag_changing = false;
                    break;
                case NotificationType.PlayStateChanged:
                    var state = mbApiInterface.Player_GetPlayState();
                    switch (state)
                    {
                        case PlayState.Playing:
                            video_panel?.Play();
                            break;
                        case PlayState.Paused:
                            video_panel?.Pause();
                            break;
                        case PlayState.Stopped:
                            // Having this on the main plugin deals with the edge case where the user sets a tag for the song, opens the panel and then reopens the song,
                            // which would lead to the video panel not having the proper _isTagChanging state and thus not cleaning up the media properly.
                            video_panel?.SetIsTagChanging(is_tag_changing);
                            video_panel?.Stop();
                            break;
                    }
                    break;
                case NotificationType.NowPlayingListEnded:
                    video_panel?.End();
                    break;
            }
        }

        //  presence of this function indicates to MusicBee that this plugin has a dockable panel. MusicBee will create the control and pass it as the panel parameter
        //  you can add your own controls to the panel if needed
        //  you can control the scrollable area of the panel using the mbApiInterface.MB_SetPanelScrollableArea function
        //  to set a MusicBee header for the panel, set about.TargetApplication in the Initialise function above to the panel header text
        public int OnDockablePanelCreated(Control panel)
        {
            cts = new CancellationTokenSource();

            panel.MinimumSize = new Size(50, 50);

            NowLoadingPanel loading = null;

            panel.Invoke((MethodInvoker)(async () =>
            {
                loading = new NowLoadingPanel
                {
                    Dock = DockStyle.Fill,
                    //Same color as empty video, so that there isn't any color clash
                    BackColor = Color.Black
                };

                video_panel = await VideoPanel.CreateVideoPanel(mbApiInterface, panel, loading, user_data, cts.Token);
            }));

            panel.Disposed += (s, e) =>
            {
                cts.Cancel();
                cts.Dispose();
                video_panel = null;
            };

            return 0;
        }

        //  presence of this function indicates to MusicBee that the dockable panel created above will show menu items when the panel header is clicked
        // return the list of ToolStripMenuItems that will be displayed
        public List<ToolStripItem> GetMenuItems()
        {
            List<ToolStripItem> list = new List<ToolStripItem>();

#if DEBUG
            var debug = new ToolStripMenuItem("Debug Mode");

            debug.Click += (s, e) =>
            {
                video_panel?.ToggleDebug();
            };

            var debugPlay = new ToolStripMenuItem("Debug Play Toggle");

            debugPlay.Click += (s, e) =>
            {
                video_panel?.PlayToggle();
            };

            var debugDispose = new ToolStripMenuItem("Debug Dispose Current VideoView");

            debugDispose.Click += (s, e) =>
            {
                video_panel?.Dispose();
                video_panel = null;
            };

            var debugToggleLoadingPanel = new ToolStripMenuItem("Debug Toggle Loading Screen Panel");

            debugToggleLoadingPanel.Click += (s, e) =>
            {
                if ((bool)(video_panel?.loading_panel.Visible)) video_panel?.loading_panel.Hide();
                else video_panel?.loading_panel.Show();
            };

            var debugToggleSync = new ToolStripMenuItem("Debug Toggle Video Sync");

            debugToggleSync.Click += (s, e) =>
            {
                if (video_panel == null) return;
                video_panel.can_sync = !video_panel.can_sync;
            };

            list.Add(debug);
            list.Add(debugPlay);
            list.Add(debugDispose);
            list.Add(debugToggleLoadingPanel);
            list.Add(debugToggleSync);
#endif

            var syncSettings = new ToolStripMenuItem("Sync delay settings");

            syncSettings.Click += (s, e) =>
            {
                if (video_panel == null)
                {
                    MessageBox.Show("Wait for Video Panel player to load", "Player didn't load",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Pretty much impossible to call Dispose since it's a Dialog, but you never know.
                sync_form?.Dispose();
                sync_form = new SyncSettingsForm(mbApiInterface, video_panel);
                sync_form.ShowDialog();
            };

            list.Add(syncSettings);

            return list;
        }
    }
}