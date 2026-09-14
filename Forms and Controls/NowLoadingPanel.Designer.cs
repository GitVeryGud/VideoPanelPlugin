namespace MusicBeePlugin
{
    partial class NowLoadingPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NowLoadingPanel));
            this.Loading = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Loading)).BeginInit();
            this.SuspendLayout();
            // 
            // Loading
            // 
            this.Loading.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Loading.Image = ((System.Drawing.Image)(resources.GetObject("Loading.Image")));
            this.Loading.Location = new System.Drawing.Point(100, 100);
            this.Loading.Name = "Loading";
            this.Loading.Size = new System.Drawing.Size(50, 50);
            this.Loading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Loading.TabIndex = 0;
            this.Loading.TabStop = false;
            // 
            // NowLoadingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Loading);
            this.DoubleBuffered = true;
            this.Name = "NowLoadingPanel";
            this.Size = new System.Drawing.Size(250, 250);
            ((System.ComponentModel.ISupportInitialize)(this.Loading)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Loading;
    }
}
