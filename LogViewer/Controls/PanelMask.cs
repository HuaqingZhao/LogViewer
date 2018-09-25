using System;
using System.Drawing;
using System.Windows.Forms;

namespace LogViewer
{
    public partial class PanelMask : UserControl
    {
        public PanelMask()
        {
            InitializeComponent();

            BackColor = Color.Gray;
            Dock = DockStyle.Fill;

            this.label1.Resize += new System.EventHandler(this.label1_Resize);
        }

        public string Message
        {
            set
            {
                label1.Text = value;
                RenderMessage();
            }
        }

        private void RenderMessage()
        {
            BackColor = Color.Gray;
            label1.BackColor = Color.Gray;
            label1.Location = new Point(this.Parent.Width / 2 - label1.Width / 2, this.Parent.Height / 2);
        }

        private void label1_Resize(object sender, EventArgs e)
        {
            RenderMessage();
        }
    }
}
