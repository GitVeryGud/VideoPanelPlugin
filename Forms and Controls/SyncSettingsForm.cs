using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MusicBeePlugin.Plugin;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MusicBeePlugin
{
    public partial class SyncSettingsForm : Form
    {
        public MusicBeeApiInterface mbApiInterface;
        // Unlikely the user will need more than 4 digits (9999ms is already insane given that most delays don't surpass 500ms);
        private int text_max_length = 4;
        private SyncSettingsData _sync_data;
        private VideoPanel _video_panel;

        public SyncSettingsForm(MusicBeeApiInterface mbApiInterface, VideoPanel video_panel)
        {
            InitializeComponent();

            _video_panel = video_panel;
            this.mbApiInterface = mbApiInterface;

            var video_delay_tooltip = new System.Windows.Forms.ToolTip();
            var video_click_delay_tooltip = new System.Windows.Forms.ToolTip();
            var constraints_tooltip = new System.Windows.Forms.ToolTip();
            video_delay_tooltip.SetToolTip(video_delay_tooltip_icon, "Most important variable, sets the target delay (in milliseconds) in relation to audio (default value = -500ms)");
            video_click_delay_tooltip.SetToolTip(video_click_delay_tooltip_icon, "Sets delay (in milliseconds) when clicking on the progress bar, " +
                "change this only if it normally stutters when clicking the bar (default value = VideoDelay + 300)");
            constraints_tooltip.SetToolTip(constraints_tooltip_icon, "Maximum deviation value from Video Delay, " +
                "lower numbers means video tries to sync more often and stutters more, " +
                "higher numbers means video tries to sync less but can deviate too much from audio (default value = 200)");

            MaximizeBox = false;
            MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            colorToSkin();

            _sync_data = SyncSettingsData.ReadSyncSettings(mbApiInterface.Setting_GetPersistentStoragePath());

            video_delay.KeyPress += integerKeyPressNegative;
            video_click_delay.KeyPress += integerKeyPressNegative;
            constraints.KeyPress += integerKeyPressPositive;
            video_delay.Text = _sync_data.video_delay.ToString();
            video_click_delay.Text = _sync_data.video_click_delay.ToString();
            constraints.Text = _sync_data.constraints.ToString();
        }

        // Colors all objects to the specific skin.
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
            reset.BackColor = background_button_skin_color;
            reset.ForeColor = foregroundButtonSkinColor;
            reset.FlatStyle = FlatStyle.Flat;
            reset.FlatAppearance.BorderColor = borderButtonSkinColor;
            reset.FlatAppearance.MouseOverBackColor = backgroundButtonHighlightSkinColor;
            reset.FlatAppearance.BorderSize = 1;
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


            // Form recolor.
            BackColor = background_skin_color;
            ForeColor = foreground_skin_color;

            // Picturebox recolor.
            Bitmap tooltipIcon = (Bitmap)video_delay_tooltip_icon.Image;
            Bitmap recolored = CreateRecoloredIcon(tooltipIcon,
                Color.FromArgb(255, 0, 0), background_input_skin_color,   // background
                Color.FromArgb(0, 255, 0), foreground_input_skin_color,   // icon
                Color.FromArgb(0, 0, 255), border_input_skin_color);  // border
            var old = video_delay_tooltip_icon.Image;
            video_delay_tooltip_icon.Image = recolored;
            video_click_delay_tooltip_icon.Image = recolored;
            constraints_tooltip_icon.Image = recolored;
            old?.Dispose();

            // Textbox recolor.
            video_delay.BackColor = background_input_skin_color;
            video_delay.ForeColor = foreground_input_skin_color;
            video_click_delay.BackColor = background_input_skin_color;
            video_click_delay.ForeColor = foreground_input_skin_color;
            constraints.BackColor = background_input_skin_color;
            constraints.ForeColor = foreground_input_skin_color;
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

        //Allow only positive and negative integers to be typed in the text box.
        private void integerKeyPressNegative(object sender, KeyPressEventArgs e)
        {
            var text_box = (System.Windows.Forms.TextBox) sender;

            // Stops any key other than digits from going through.
            bool isNotDigit = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
            // Stops keys from going through if the number has 4 digits (regardless of being a positive or negative number).
            bool isMaxLength = text_box.Text.Length == text_max_length + 1 || (text_box.Text.Length == text_max_length && text_box.Text.IndexOf('-') != 0);

            // Stops text from being written.
            if (isNotDigit || isMaxLength)
            {
                e.Handled = true;
            }

            // Only allow a negative sign at the start.
            bool isNegative = e.KeyChar == '-' && text_box.SelectionStart == 0 && text_box.Text.IndexOf('-') != 0;
            // Allow the backspace (U+0008) "char" to go through.
            bool isBackSpace = e.KeyChar == '';
            // Allow to write a number if the text is being selected.
            bool isSelecting = text_box.SelectionLength > 0;

            // Allow text to be written.
            if (isNegative || isBackSpace || (isSelecting && !isNotDigit))
            {
                e.Handled = false;
                close.Text = "Cancel";
            }
        }

        // Allow only positive integers to be typed in the text box.
        private void integerKeyPressPositive(object sender, KeyPressEventArgs e)
        {
            var text_box = (System.Windows.Forms.TextBox)sender;

            // Stops any key other than digits from going through.
            bool isNotDigit = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
            // Stops keys from going through if the number has 4 digits (regardless of being a positive or negative number).
            bool isMaxLength = text_box.Text.Length == text_max_length;

            if (isNotDigit || isMaxLength)
            {
                // If handled, the keystroke doesn't go though.
                e.Handled = true;
            }

            // Allow the backspace (U+0008) "char" to go through.
            bool isBackSpace = e.KeyChar == '';
            // Allow to write a number if the text is being selected.
            bool isSelecting = text_box.SelectionLength > 0;

            if (isBackSpace || (isSelecting && !isNotDigit))
            {
                // If NOT handled, the keystroke GOES though.
                e.Handled = false;
                close.Text = "Cancel";
            }
        }

        private void reset_Click(object sender, EventArgs e)
        {
            _sync_data = new SyncSettingsData();

            video_delay.Text = _sync_data.video_delay.ToString();
            video_click_delay.Text = _sync_data.video_click_delay.ToString();
            constraints.Text = _sync_data.constraints.ToString();
            close.Text = "Cancel";
        }

        private void apply_Click(object sender, EventArgs e)
        {
            try 
            { 
                _sync_data.video_delay = int.Parse(video_delay.Text);
                _sync_data.video_click_delay = int.Parse(video_click_delay.Text);
                _sync_data.constraints = int.Parse(constraints.Text);

                SyncSettingsData.WriteSyncSettings(_sync_data, mbApiInterface.Setting_GetPersistentStoragePath());
                _video_panel.SetSyncSettings(_sync_data);
                close.Text = "Close";
            }
            catch
            {
                MessageBox.Show("Input valid numbers", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Since it's a ShowDialog() it doesn't dispose right away, but when you open another it disposes of the last.
        private void close_Click(object sender, EventArgs e)
        {
            Close();
        }     
    }
}
