namespace LogViewer.Controls
{
    partial class PatternSelectionForm
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
            this.patternCtrl1 = new LogViewer.Controls.PatternCtrl();
            this.SuspendLayout();
            // 
            // patternCtrl1
            // 
            this.patternCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.patternCtrl1.Location = new System.Drawing.Point(0, 0);
            this.patternCtrl1.Name = "patternCtrl1";
            this.patternCtrl1.Size = new System.Drawing.Size(666, 375);
            this.patternCtrl1.TabIndex = 0;
            // 
            // PatternSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 375);
            this.Controls.Add(this.patternCtrl1);
            this.Name = "PatternSelectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PatternSelectionForm";
            this.ResumeLayout(false);

        }

        #endregion

        private PatternCtrl patternCtrl1;
    }
}