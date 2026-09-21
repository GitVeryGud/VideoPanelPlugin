using MusicBeePlugin.Saved_Data_Classes;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;

namespace MusicBeePlugin.Forms_and_Controls
{
    public partial class SetVideoSettings : Form
    {
        private MusicBeeApiInterface mbApiInterface;
        private VideoPanel _video_panel;
        private VideoType _video_type;
        private string _now_playing_uri;
        private bool _was_playing = false;

        public SetVideoSettings(MusicBeeApiInterface mbApiInterface, VideoPanel video_panel, string uri)
        {
            InitializeComponent();

            // Pauses video to ensure that the correct track is edited.
            if (mbApiInterface.Player_GetPlayState() == PlayState.Playing)
            {
                _was_playing = true;
                mbApiInterface.Player_PlayPause();
            }

            _video_panel = video_panel;
            this.mbApiInterface = mbApiInterface;
            _now_playing_uri = uri;

            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            var default_tooltip = new System.Windows.Forms.ToolTip();
            var force_artwork_tooltip = new System.Windows.Forms.ToolTip();
            var custom_video_tooltip = new System.Windows.Forms.ToolTip();
            var custom_tooltip = new System.Windows.Forms.ToolTip();
            default_tooltip.SetToolTip(default_tooltip_icon, "Default state for VIDEO files, shows the track's video on the player");
            force_artwork_tooltip.SetToolTip(force_artwork_tooltip_icon, "Default state for AUDIO files, shows the primary artwork on the player");
            custom_video_tooltip.SetToolTip(custom_video_tooltip_icon, "Choose a custom video to play on the player, loops and never pauses (intended for animated covers)");
            custom_tooltip.SetToolTip(custom_tooltip_icon, "Changes which Custom tag you use to save and read video settings. " +
                "APPLIES TO ALL VIDEOS, SO ONLY CHANGE IF NECESSARY SINCE YOU LOSE OTHER VIDEO'S SETTINGS (default value = 18)");

            // Populate combo box with custom tag numbers
            for (int i = 1; i <= 20; i++)
            {
                custom_combo_box.Items.Add(i);
            }

            // custom tag follows customX format with X being the number, so doing these operations results in the custom index
            custom_combo_box.SelectedIndex = int.Parse(_video_panel.user_data.custom_tag.ToString().Substring(6)) - 1;

            custom_video_button.Enabled = false;
            custom_video_text.Enabled = false;

            colorToSkin();

            _video_type = _video_panel.GetCurrentVideoType();

            switch (_video_type)
            {
                case VideoType.Default:
                    default_checkbox.Checked = true;
                    break;

                case VideoType.ForceArtwork:
                    force_artwork_checkbox.Checked = true;
                    break;

                case VideoType.CustomVideo:
                    custom_video_checkbox.Checked = true;
                    string tag = mbApiInterface.NowPlaying_GetFileTag(_video_panel.user_data.custom_tag);
                    int index = tag.IndexOf(";");
                    var videoUri = tag.Substring(index + 1);

                    custom_video_text.Text = videoUri;
                    break;
            }

            close.Text = "Close";

            FormClosed += (s, e) =>
            {
                if (_was_playing)
                {
                    mbApiInterface.Player_PlayPause();
                }
            };
        }

        private void colorToSkin()
        {
            // Form colors.
            Color background_skin_color = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
            Color foreground_skin_color = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputPanel, ElementState.ElementStateDefault, ElementComponent.ComponentForeground));

            // Input/Icon colors.
            Color background_input_skin_color = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
            Color foreground_input_skin_color = foreground_skin_color;
            Color border_input_skin_color = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinInputControl, ElementState.ElementStateDefault, ElementComponent.ComponentBorder));

            // Button colors.
            Color background_button_skin_color = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinButton, ElementState.ElementStateDefault, ElementComponent.ComponentBackground));
            Color backgroundButtonHighlightSkinColor = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinButton, ElementState.ElementStateHighlight, ElementComponent.ComponentBackground));
            Color foregroundButtonSkinColor = foreground_skin_color;
            Color borderButtonSkinColor = Color.FromArgb(mbApiInterface.Setting_GetSkinElementColour(SkinElement.SkinButton, ElementState.ElementStateDefault, ElementComponent.ComponentBorder));

            // Button recolor.
            custom_video_button.BackColor = background_button_skin_color;
            custom_video_button.ForeColor = foregroundButtonSkinColor;
            custom_video_button.FlatStyle = FlatStyle.Flat;
            custom_video_button.FlatAppearance.BorderColor = borderButtonSkinColor;
            custom_video_button.FlatAppearance.MouseOverBackColor = backgroundButtonHighlightSkinColor;
            custom_video_button.FlatAppearance.BorderSize = 1;
            apply.BackColor = background_button_skin_color;
            apply.ForeColor = foregroundButtonSkinColor;
            apply.FlatStyle = FlatStyle.Flat;
            apply.FlatAppearance.BorderColor = borderButtonSkinColor;
            apply.FlatAppearance.MouseOverBackColor = backgroundButtonHighlightSkinColor;
            apply.FlatAppearance.BorderSize = 1;
            close.BackColor = background_button_skin_color;
            close.ForeColor = foregroundButtonSkinColor;
            close.FlatStyle = FlatStyle.Flat;
            close.FlatAppearance.BorderColor = borderButtonSkinColor;
            close.FlatAppearance.MouseOverBackColor = backgroundButtonHighlightSkinColor;
            close.FlatAppearance.BorderSize = 1;

            // Checkbox recolor
            default_checkbox.ForeColor = foreground_input_skin_color;

            // Form recolor.
            BackColor = background_skin_color;
            ForeColor = foreground_skin_color;

            // Picturebox recolor.
            Bitmap tooltipIcon = (Bitmap)default_tooltip_icon.Image;
            Bitmap recolored = CreateRecoloredIcon(tooltipIcon,
                Color.FromArgb(255, 0, 0), background_input_skin_color,   // background
                Color.FromArgb(0, 255, 0), foreground_input_skin_color,   // icon
                Color.FromArgb(0, 0, 255), border_input_skin_color);  // border
            var old = default_tooltip_icon.Image;
            default_tooltip_icon.Image = recolored;
            force_artwork_tooltip_icon.Image = recolored;
            custom_video_tooltip_icon.Image = recolored;
            old?.Dispose();

            // Textbox recolor.
            custom_video_text.BackColor = background_input_skin_color;
            custom_video_text.ForeColor = foreground_input_skin_color;
        }

        // Recolors the tooltip icon to skin colors.
        private Bitmap CreateRecoloredIcon(Bitmap template_icon,
        Color old_color1, Color new_color1,
        Color old_color2, Color new_color2,
        Color old_color3, Color new_color3)
        {
            Bitmap result = new Bitmap(template_icon.Width, template_icon.Height, PixelFormat.Format32bppArgb);

            ColorMap[] colorMap = new ColorMap[3];
            colorMap[0] = new ColorMap { OldColor = old_color1, NewColor = new_color1 };
            colorMap[1] = new ColorMap { OldColor = old_color2, NewColor = new_color2 };
            colorMap[2] = new ColorMap { OldColor = old_color3, NewColor = new_color3 };

            using (Graphics g = Graphics.FromImage(result))
            using (var attributes = new ImageAttributes())
            {
                attributes.SetRemapTable(colorMap, ColorAdjustType.Bitmap);
                g.DrawImage(template_icon,
                    new Rectangle(0, 0, template_icon.Width, template_icon.Height),
                    0, 0, template_icon.Width, template_icon.Height,
                    GraphicsUnit.Pixel, attributes);
            }

            return result;
        }

        private void default_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (default_checkbox.Checked)
            {
                custom_video_checkbox.Checked = false;
                force_artwork_checkbox.Checked = false;
                close.Text = "Cancel";
                _video_type = VideoType.Default;
            }
        }

        private void force_artwork_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (force_artwork_checkbox.Checked)
            {
                custom_video_checkbox.Checked = false;
                default_checkbox.Checked = false;
                close.Text = "Cancel";
                _video_type = VideoType.ForceArtwork;
            }
        }

        private void custom_video_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (custom_video_checkbox.Checked)
            {
                default_checkbox.Checked = false;
                force_artwork_checkbox.Checked = false;
                custom_video_button.Enabled = true;
                custom_video_text.Enabled = true;
                close.Text = "Cancel";
                _video_type = VideoType.CustomVideo;
            }

            else
            {
                custom_video_button.Enabled = false;
                custom_video_text.Enabled = false;
            }
        }

        private void custom_video_button_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Video Files|*.mp4;*.mkv;*.avi;*.mov;*.wmv;*.flv;*.webm;*.m4v;*.mpg;*.mpeg;*.ts" +
                                        "|All Files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the path of specified file
                    var filePath = openFileDialog.FileName;

                    custom_video_text.Text = filePath;
                    close.Text = "Cancel";
                }
            }
        }

        private void apply_Click(object sender, EventArgs e)
        {
            // If parse fails, stop apply.
            if (!Enum.TryParse("Custom" + custom_combo_box.SelectedItem, out MetaDataType new_tag)) return;

            _video_panel.user_data.custom_tag = new_tag;
            _video_panel.user_data.WriteUserData();

            switch (_video_type)
            {
                case VideoType.Default:
                    mbApiInterface.Library_SetFileTag(_now_playing_uri, new_tag, "Default;");
                    mbApiInterface.Library_CommitTagsToFile(_now_playing_uri);
                    break;

                case VideoType.ForceArtwork:
                    mbApiInterface.Library_SetFileTag(_now_playing_uri, new_tag, "ForceArtwork;");
                    mbApiInterface.Library_CommitTagsToFile(_now_playing_uri);
                    break;

                case VideoType.CustomVideo:
                    string[] values = { "CustomVideo", custom_video_text.Text};

                    mbApiInterface.Library_SetFileTag(_now_playing_uri, new_tag, string.Join(";", values));

                    mbApiInterface.Library_CommitTagsToFile(_now_playing_uri);
                    break;
            }

            _video_panel.SetCurrentVideoType(_video_type);
            // Sets video following the chosen type.
            _video_panel.SetVideoForSettings(_now_playing_uri, custom_video_text.Text);

            close.Text = "Close";
        }

        private void close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
