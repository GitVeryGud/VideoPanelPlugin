namespace MusicBeePlugin.Forms_and_Controls
{
    partial class SetVideoSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetVideoSettings));
            this.force_artwork_checkbox = new System.Windows.Forms.CheckBox();
            this.default_checkbox = new System.Windows.Forms.CheckBox();
            this.custom_video_checkbox = new System.Windows.Forms.CheckBox();
            this.custom_video_text = new System.Windows.Forms.TextBox();
            this.custom_video_button = new System.Windows.Forms.Button();
            this.custom_combo_box = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.default_tooltip_icon = new System.Windows.Forms.PictureBox();
            this.force_artwork_tooltip_icon = new System.Windows.Forms.PictureBox();
            this.custom_video_tooltip_icon = new System.Windows.Forms.PictureBox();
            this.custom_tooltip_icon = new System.Windows.Forms.PictureBox();
            this.close = new System.Windows.Forms.Button();
            this.apply = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.default_tooltip_icon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.force_artwork_tooltip_icon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.custom_video_tooltip_icon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.custom_tooltip_icon)).BeginInit();
            this.SuspendLayout();
            // 
            // force_artwork_checkbox
            // 
            this.force_artwork_checkbox.AutoSize = true;
            this.force_artwork_checkbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.force_artwork_checkbox.Location = new System.Drawing.Point(22, 77);
            this.force_artwork_checkbox.Name = "force_artwork_checkbox";
            this.force_artwork_checkbox.Size = new System.Drawing.Size(147, 28);
            this.force_artwork_checkbox.TabIndex = 0;
            this.force_artwork_checkbox.Text = "Force Artwork";
            this.force_artwork_checkbox.UseVisualStyleBackColor = true;
            this.force_artwork_checkbox.CheckedChanged += new System.EventHandler(this.force_artwork_checkbox_CheckedChanged);
            // 
            // default_checkbox
            // 
            this.default_checkbox.AutoSize = true;
            this.default_checkbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.default_checkbox.Location = new System.Drawing.Point(22, 21);
            this.default_checkbox.Name = "default_checkbox";
            this.default_checkbox.Size = new System.Drawing.Size(86, 28);
            this.default_checkbox.TabIndex = 1;
            this.default_checkbox.Text = "Default";
            this.default_checkbox.UseVisualStyleBackColor = true;
            this.default_checkbox.CheckedChanged += new System.EventHandler(this.default_checkbox_CheckedChanged);
            // 
            // custom_video_checkbox
            // 
            this.custom_video_checkbox.AutoSize = true;
            this.custom_video_checkbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.custom_video_checkbox.Location = new System.Drawing.Point(22, 134);
            this.custom_video_checkbox.Name = "custom_video_checkbox";
            this.custom_video_checkbox.Size = new System.Drawing.Size(148, 28);
            this.custom_video_checkbox.TabIndex = 2;
            this.custom_video_checkbox.Text = "Custom Video";
            this.custom_video_checkbox.UseVisualStyleBackColor = true;
            this.custom_video_checkbox.CheckedChanged += new System.EventHandler(this.custom_video_checkbox_CheckedChanged);
            // 
            // custom_video_text
            // 
            this.custom_video_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.custom_video_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.custom_video_text.Location = new System.Drawing.Point(169, 132);
            this.custom_video_text.Name = "custom_video_text";
            this.custom_video_text.Size = new System.Drawing.Size(269, 29);
            this.custom_video_text.TabIndex = 3;
            // 
            // custom_video_button
            // 
            this.custom_video_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.custom_video_button.Font = new System.Drawing.Font("Microsoft New Tai Lue", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.custom_video_button.Location = new System.Drawing.Point(444, 134);
            this.custom_video_button.Name = "custom_video_button";
            this.custom_video_button.Size = new System.Drawing.Size(28, 25);
            this.custom_video_button.TabIndex = 4;
            this.custom_video_button.Text = "...";
            this.custom_video_button.UseVisualStyleBackColor = true;
            this.custom_video_button.Click += new System.EventHandler(this.custom_video_button_Click);
            // 
            // custom_combo_box
            // 
            this.custom_combo_box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.custom_combo_box.FormattingEnabled = true;
            this.custom_combo_box.Location = new System.Drawing.Point(168, 201);
            this.custom_combo_box.Name = "custom_combo_box";
            this.custom_combo_box.Size = new System.Drawing.Size(46, 21);
            this.custom_combo_box.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 198);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 24);
            this.label1.TabIndex = 6;
            this.label1.Text = "Saves to Custom";
            // 
            // default_tooltip_icon
            // 
            this.default_tooltip_icon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.default_tooltip_icon.Image = ((System.Drawing.Image)(resources.GetObject("default_tooltip_icon.Image")));
            this.default_tooltip_icon.Location = new System.Drawing.Point(513, 21);
            this.default_tooltip_icon.Name = "default_tooltip_icon";
            this.default_tooltip_icon.Size = new System.Drawing.Size(24, 24);
            this.default_tooltip_icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.default_tooltip_icon.TabIndex = 8;
            this.default_tooltip_icon.TabStop = false;
            // 
            // force_artwork_tooltip_icon
            // 
            this.force_artwork_tooltip_icon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.force_artwork_tooltip_icon.Location = new System.Drawing.Point(513, 77);
            this.force_artwork_tooltip_icon.Name = "force_artwork_tooltip_icon";
            this.force_artwork_tooltip_icon.Size = new System.Drawing.Size(24, 24);
            this.force_artwork_tooltip_icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.force_artwork_tooltip_icon.TabIndex = 10;
            this.force_artwork_tooltip_icon.TabStop = false;
            // 
            // custom_video_tooltip_icon
            // 
            this.custom_video_tooltip_icon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.custom_video_tooltip_icon.Location = new System.Drawing.Point(513, 134);
            this.custom_video_tooltip_icon.Name = "custom_video_tooltip_icon";
            this.custom_video_tooltip_icon.Size = new System.Drawing.Size(24, 24);
            this.custom_video_tooltip_icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.custom_video_tooltip_icon.TabIndex = 11;
            this.custom_video_tooltip_icon.TabStop = false;
            // 
            // custom_tooltip_icon
            // 
            this.custom_tooltip_icon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.custom_tooltip_icon.Image = ((System.Drawing.Image)(resources.GetObject("custom_tooltip_icon.Image")));
            this.custom_tooltip_icon.Location = new System.Drawing.Point(513, 198);
            this.custom_tooltip_icon.Name = "custom_tooltip_icon";
            this.custom_tooltip_icon.Size = new System.Drawing.Size(24, 24);
            this.custom_tooltip_icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.custom_tooltip_icon.TabIndex = 12;
            this.custom_tooltip_icon.TabStop = false;
            // 
            // close
            // 
            this.close.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.close.Location = new System.Drawing.Point(451, 271);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(86, 33);
            this.close.TabIndex = 14;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // apply
            // 
            this.apply.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.apply.Location = new System.Drawing.Point(359, 271);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(86, 33);
            this.apply.TabIndex = 13;
            this.apply.Text = "Apply";
            this.apply.UseVisualStyleBackColor = true;
            this.apply.Click += new System.EventHandler(this.apply_Click);
            // 
            // SetVideoSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 320);
            this.Controls.Add(this.close);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.custom_tooltip_icon);
            this.Controls.Add(this.custom_video_tooltip_icon);
            this.Controls.Add(this.force_artwork_tooltip_icon);
            this.Controls.Add(this.default_tooltip_icon);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.custom_combo_box);
            this.Controls.Add(this.custom_video_button);
            this.Controls.Add(this.custom_video_text);
            this.Controls.Add(this.custom_video_checkbox);
            this.Controls.Add(this.default_checkbox);
            this.Controls.Add(this.force_artwork_checkbox);
            this.Name = "SetVideoSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Current Video Settings";
            ((System.ComponentModel.ISupportInitialize)(this.default_tooltip_icon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.force_artwork_tooltip_icon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.custom_video_tooltip_icon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.custom_tooltip_icon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox force_artwork_checkbox;
        private System.Windows.Forms.CheckBox default_checkbox;
        private System.Windows.Forms.CheckBox custom_video_checkbox;
        private System.Windows.Forms.TextBox custom_video_text;
        private System.Windows.Forms.Button custom_video_button;
        private System.Windows.Forms.ComboBox custom_combo_box;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox default_tooltip_icon;
        private System.Windows.Forms.PictureBox force_artwork_tooltip_icon;
        private System.Windows.Forms.PictureBox custom_video_tooltip_icon;
        private System.Windows.Forms.PictureBox custom_tooltip_icon;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Button apply;
    }
}