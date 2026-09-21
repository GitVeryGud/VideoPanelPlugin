namespace MusicBeePlugin
{
    partial class SyncSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SyncSettingsForm));
            this.video_delay = new System.Windows.Forms.TextBox();
            this.video_click_delay = new System.Windows.Forms.TextBox();
            this.constraints = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.apply = new System.Windows.Forms.Button();
            this.video_delay_tooltip_icon = new System.Windows.Forms.PictureBox();
            this.reset = new System.Windows.Forms.Button();
            this.video_click_delay_tooltip_icon = new System.Windows.Forms.PictureBox();
            this.constraints_tooltip_icon = new System.Windows.Forms.PictureBox();
            this.close = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.video_delay_tooltip_icon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.video_click_delay_tooltip_icon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.constraints_tooltip_icon)).BeginInit();
            this.SuspendLayout();
            // 
            // video_delay
            // 
            this.video_delay.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.video_delay.Location = new System.Drawing.Point(196, 34);
            this.video_delay.Name = "video_delay";
            this.video_delay.ShortcutsEnabled = false;
            this.video_delay.Size = new System.Drawing.Size(57, 29);
            this.video_delay.TabIndex = 0;
            this.video_delay.Text = "-5423";
            // 
            // video_click_delay
            // 
            this.video_click_delay.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.video_click_delay.Location = new System.Drawing.Point(196, 84);
            this.video_click_delay.Name = "video_click_delay";
            this.video_click_delay.ShortcutsEnabled = false;
            this.video_click_delay.Size = new System.Drawing.Size(57, 29);
            this.video_click_delay.TabIndex = 1;
            // 
            // constraints
            // 
            this.constraints.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.constraints.Location = new System.Drawing.Point(196, 134);
            this.constraints.Name = "constraints";
            this.constraints.ShortcutsEnabled = false;
            this.constraints.Size = new System.Drawing.Size(57, 29);
            this.constraints.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Video delay";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(169, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "Video click delay";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 24);
            this.label3.TabIndex = 5;
            this.label3.Text = "Constraints";
            // 
            // apply
            // 
            this.apply.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.apply.Location = new System.Drawing.Point(318, 275);
            this.apply.Name = "apply";
            this.apply.Size = new System.Drawing.Size(86, 33);
            this.apply.TabIndex = 6;
            this.apply.Text = "Apply";
            this.apply.UseVisualStyleBackColor = true;
            this.apply.Click += new System.EventHandler(this.apply_Click);
            // 
            // video_delay_tooltip_icon
            // 
            this.video_delay_tooltip_icon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.video_delay_tooltip_icon.Image = ((System.Drawing.Image)(resources.GetObject("video_delay_tooltip_icon.Image")));
            this.video_delay_tooltip_icon.Location = new System.Drawing.Point(472, 34);
            this.video_delay_tooltip_icon.Name = "video_delay_tooltip_icon";
            this.video_delay_tooltip_icon.Size = new System.Drawing.Size(24, 24);
            this.video_delay_tooltip_icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.video_delay_tooltip_icon.TabIndex = 7;
            this.video_delay_tooltip_icon.TabStop = false;
            // 
            // reset
            // 
            this.reset.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reset.Location = new System.Drawing.Point(12, 275);
            this.reset.Name = "reset";
            this.reset.Size = new System.Drawing.Size(169, 33);
            this.reset.TabIndex = 8;
            this.reset.Text = "Reset to default";
            this.reset.UseVisualStyleBackColor = true;
            this.reset.Click += new System.EventHandler(this.reset_Click);
            // 
            // video_click_delay_tooltip_icon
            // 
            this.video_click_delay_tooltip_icon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.video_click_delay_tooltip_icon.Location = new System.Drawing.Point(472, 84);
            this.video_click_delay_tooltip_icon.Name = "video_click_delay_tooltip_icon";
            this.video_click_delay_tooltip_icon.Size = new System.Drawing.Size(24, 24);
            this.video_click_delay_tooltip_icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.video_click_delay_tooltip_icon.TabIndex = 9;
            this.video_click_delay_tooltip_icon.TabStop = false;
            // 
            // constraints_tooltip_icon
            // 
            this.constraints_tooltip_icon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.constraints_tooltip_icon.Location = new System.Drawing.Point(472, 134);
            this.constraints_tooltip_icon.Name = "constraints_tooltip_icon";
            this.constraints_tooltip_icon.Size = new System.Drawing.Size(24, 24);
            this.constraints_tooltip_icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.constraints_tooltip_icon.TabIndex = 10;
            this.constraints_tooltip_icon.TabStop = false;
            // 
            // close
            // 
            this.close.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.close.Location = new System.Drawing.Point(410, 275);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(86, 33);
            this.close.TabIndex = 11;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // SyncSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 320);
            this.Controls.Add(this.close);
            this.Controls.Add(this.constraints_tooltip_icon);
            this.Controls.Add(this.video_click_delay_tooltip_icon);
            this.Controls.Add(this.reset);
            this.Controls.Add(this.video_delay_tooltip_icon);
            this.Controls.Add(this.apply);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.constraints);
            this.Controls.Add(this.video_click_delay);
            this.Controls.Add(this.video_delay);
            this.Name = "SyncSettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Sync Settings";
            ((System.ComponentModel.ISupportInitialize)(this.video_delay_tooltip_icon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.video_click_delay_tooltip_icon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.constraints_tooltip_icon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox video_delay;
        private System.Windows.Forms.TextBox video_click_delay;
        private System.Windows.Forms.TextBox constraints;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button apply;
        private System.Windows.Forms.PictureBox video_delay_tooltip_icon;
        private System.Windows.Forms.Button reset;
        private System.Windows.Forms.PictureBox video_click_delay_tooltip_icon;
        private System.Windows.Forms.PictureBox constraints_tooltip_icon;
        private System.Windows.Forms.Button close;
    }
}