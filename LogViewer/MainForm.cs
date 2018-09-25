using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LogViewer.Controls;
using LogViewer.Utilities;
namespace LogViewer
{
    public partial class MainForm : Form
    {
        private IList<Log> logs;
        private IDictionary<string, string> patterns;
        private int tabPagesCount = 0;
        private TabControl tbcMain;
        private PanelMask panelMask;
        private PatternManagerForm patternManagerForm;

        private IList<string> paths;

        public MainForm()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            LogManager.ShowMask += ShowMaskHandler;
            LogManager.HideMask += HideMaskHandler;
            paths = new List<string>();

            LogManager.StyleChanged += OnStyleChanged;

            panelMask = new PanelMask();

            this.Controls.Add(panelMask);
        }

        public void LoadLog()
        {
            GC.Collect();

            LogManager.ShowMask("Loading...");

            Task.Factory.StartNew(() =>
            {
                this.BeginInvokeOnGui(() =>
                      {
                          this.SuspendLayout();
                          pnlMain.Controls.Remove(tbcMain);

                          LogManager.CaseSensitive = chkCaseSensitive.Checked;

                          tbcMain = new TabControl();

                          this.tbcMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
                          tbcMain.Dock = DockStyle.Fill;
                          tbcMain.Multiline = true;

                          tbcMain.DrawItem += tbcMain_DrawItem;

                          this.Text = string.Empty;
                          foreach (var path in paths)
                          {
                              this.Text += Path.GetFileName(path) + " ";
                          }

                          this.Text += "- Log Analycis Tool";

                          LogManager.Load(paths);

                          logs = LogManager.LogList;

                          if (logs == null || logs.Count < 1)
                          {
                              MessageBox.Show("Log is empty.");
                              btnOpen.Enabled = true;
                              this.tableLayoutPanel3.Visible = false;
                              LogManager.HideMask();
                              return;
                          }

                          patterns = LogManager.Patterns;

                          dtpTo.Value = logs[0].LogTime;
                          dtpFrom.Value = logs[logs.Count - 1].LogTime;

                          var tabCount = tbcMain.TabPages.Count;
                          if (tabCount > 0)
                          {
                              for (int i = 0; i < tabCount; i++)
                              {
                                  tbcMain.TabPages.RemoveAt(0);
                              }
                          }

                          AddTabPage("High Light", new FullLogCtrl());

                          foreach (KeyValuePair<string, string> item in patterns)
                          {
                              AddTabPage(item.Key, item.Value, new LogCtrl());
                          }

                          AddTabPage("Compare", new CompareCtrl());

                          if (LogManager.LoadControl != null)
                              LogManager.LoadControl();

                          pnlMain.Controls.Add(tbcMain);

                          this.tableLayoutPanel3.Visible = true;
                          pnlTitleBar.Visible = true;
                          chkCaseSensitive.Visible = true;
                          ckbStyle.Visible = true;
                          lblFrom.Visible = true;
                          lblTo.Visible = true;
                          dtpFrom.Visible = true;
                          dtpTo.Visible = true;
                          btnGo.Visible = true;
                          btnPattern.Visible = true;

                          for (int i = 0; i < tbcMain.TabCount; i++)
                          {
                              tbcMain.SelectTab(i);
                          }

                          tbcMain.SelectTab(0);
                          GUIHelper.ChangeStyle(this);

                          LogManager.HideMask();
                          this.ResumeLayout();
                      });
            });
        }

        private void OnStyleChanged()
        {
            GUIHelper.ChangeStyle(this);
        }

        private void AddTabPage(string tabName, string pattern, LogCtrl logsCtrl)
        {
            logsCtrl.SuspendLayout();
            logsCtrl.TabName = tabName;
            logsCtrl.SetPatterns(pattern);

            var tabPage = new TabPage();
            tabPage.Location = new System.Drawing.Point(4, 22);
            tabPage.Padding = new System.Windows.Forms.Padding(3);
            tabPage.Text = tabName;

            logsCtrl.ResumeLayout();
            tabPage.Controls.Add(logsCtrl);

            this.tbcMain.Controls.Add(tabPage);
        }

        private void AddTabPage(string tabName, FullLogCtrl logsCtrl)
        {
            logsCtrl.SuspendLayout();

            var tabPage = new TabPage();
            tabPage.Location = new System.Drawing.Point(4, 22);
            tabPage.Padding = new System.Windows.Forms.Padding(3);
            tabPage.Text = tabName;

            logsCtrl.ResumeLayout();
            tabPage.Controls.Add(logsCtrl);

            this.tbcMain.Controls.Add(tabPage);
        }

        private void AddTabPage(string tabName, CompareCtrl cc)
        {
            cc.SuspendLayout();

            var tabPage = new TabPage();
            tabPage.Location = new System.Drawing.Point(4, 22);
            tabPage.Padding = new System.Windows.Forms.Padding(3);
            tabPage.Text = tabName;

            tabPage.Controls.Add(cc);

            this.tbcMain.Controls.Add(tabPage);
            cc.ResumeLayout();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LogManager.ShowMask("Working...");

            Task.Factory.StartNew(() =>
            {
                if (dtpFrom.Value > dtpTo.Value)
                {
                    MessageBox.Show("Start time greater than end.");
                    LogManager.HideMask();
                    return;
                }

                var temp = LogManager.EventTimeChanged;
                if (temp != null)
                {
                    temp(dtpFrom.Value, dtpTo.Value);
                }

                LogManager.HideMask();
            });
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if(PatternHelper.IsUnZip())
            {
                ZipHelper.UnZip();
            }

            openFileDialog1.Multiselect = true;
            openFileDialog1.Title = "Open log files...";
            openFileDialog1.Filter = "Log Files (*.log)|*.log";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                paths = openFileDialog1.FileNames;
                LoadLog();
            }
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files != null && files.Length == 1)
                {
                    paths = new List<string>() { files[0] };
                    LoadLog();
                }
            }
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files != null && files.Length == 1)
                {
                    paths = new List<string>() { files[0] };
                    LoadLog();
                }
            }
        }

        private void tbcMain_DrawItem(object sender, DrawItemEventArgs e)
        {
            ChangeTabColor(e);
        }

        private void ChangeTabColor(DrawItemEventArgs e)
        {
            Rectangle lasttabrect = tbcMain.GetTabRect(0);
            RectangleF emptyspacerect = new RectangleF(
                    0,
                    0,
                    tbcMain.Width,
                    lasttabrect.Height * tbcMain.RowCount + 2);

            Brush b = LogManager.BackGroundBrush; // the color you want
            e.Graphics.FillRectangle(b, emptyspacerect);
            tabPagesCount = tbcMain.TabPages.Count;

            Brush ForeBrush = new SolidBrush(LogManager.ForceColor);//Set foreground color

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            for (int i = 0; i < tbcMain.TabPages.Count; i++)
            {
                string TabName = this.tbcMain.TabPages[i].Text;
                e.Graphics.DrawString(TabName, e.Font, ForeBrush, tbcMain.GetTabRect(i), sf);
            }

            //Dispose objects
            sf.Dispose();
            if (e.Index != this.tbcMain.SelectedIndex)
            {
                //BackBrush.Dispose();
                ForeBrush.Dispose();
            }
        }

        private void ckbStyle_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbStyle.Checked)
            {
                LogManager.ForceBrush = Brushes.White;
                LogManager.ForceColor = Color.White;
                LogManager.BackGroundBrush = Brushes.Black;
                LogManager.BackGroundColor = Color.Black;
            }
            else
            {
                LogManager.ForceBrush = Brushes.Black;
                LogManager.ForceColor = Color.Black;
                LogManager.BackGroundBrush = Brushes.White;
                LogManager.BackGroundColor = Color.White;
            }

            if (LogManager.StyleChanged != null)
                LogManager.StyleChanged();
        }

        private void chkCaseSensitive_CheckedChanged(object sender, EventArgs e)
        {
            LogManager.ShowMask("Changing case sensitive");

            Task.Factory.StartNew(() =>
            {
                LogManager.CaseSensitive = chkCaseSensitive.Checked;
                LogManager.CaseSensitiveChanged();
                LogManager.HideMask();
            });
        }

        private void ShowMaskHandler(string message)
        {
            panelMask.Message = message;
            panelMask.BringToFront();
        }

        private void HideMaskHandler()
        {
            this.BeginInvokeOnGui(() =>
                   {
                       panelMask.Message = string.Empty;
                       panelMask.SendToBack();
                   });
        }


        private void txtPattern_Click(object sender, EventArgs e)
        {
            if (patternManagerForm == null || patternManagerForm.IsDisposed)
                patternManagerForm = new PatternManagerForm();

            patternManagerForm.BringToFront();
            patternManagerForm.Show();
        }
    }
}
