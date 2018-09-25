namespace LogViewer.Controls
{
    partial class PatternCtrl
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
            this.components = new System.ComponentModel.Container();
            this.dgvPatterns = new System.Windows.Forms.DataGridView();
            this.patternNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.patternValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.patternBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPatterns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.patternBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPatterns
            // 
            this.dgvPatterns.AllowUserToAddRows = false;
            this.dgvPatterns.AllowUserToDeleteRows = false;
            this.dgvPatterns.AllowUserToResizeColumns = false;
            this.dgvPatterns.AllowUserToResizeRows = false;
            this.dgvPatterns.AutoGenerateColumns = false;
            this.dgvPatterns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPatterns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.patternNameDataGridViewTextBoxColumn,
            this.patternValueDataGridViewTextBoxColumn});
            this.dgvPatterns.DataSource = this.patternBindingSource;
            this.dgvPatterns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPatterns.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dgvPatterns.Location = new System.Drawing.Point(0, 0);
            this.dgvPatterns.MultiSelect = false;
            this.dgvPatterns.Name = "dgvPatterns";
            this.dgvPatterns.ReadOnly = true;
            this.dgvPatterns.RowHeadersVisible = false;
            this.dgvPatterns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPatterns.Size = new System.Drawing.Size(698, 493);
            this.dgvPatterns.TabIndex = 1;
            this.dgvPatterns.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPatterns_CellContentClick);
            this.dgvPatterns.SelectionChanged += new System.EventHandler(this.dgvPatterns_SelectionChanged);
            this.dgvPatterns.Resize += new System.EventHandler(this.dgvPatterns_Resize);
            // 
            // patternNameDataGridViewTextBoxColumn
            // 
            this.patternNameDataGridViewTextBoxColumn.DataPropertyName = "PatternName";
            this.patternNameDataGridViewTextBoxColumn.HeaderText = "PatternName";
            this.patternNameDataGridViewTextBoxColumn.Name = "patternNameDataGridViewTextBoxColumn";
            this.patternNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // patternValueDataGridViewTextBoxColumn
            // 
            this.patternValueDataGridViewTextBoxColumn.DataPropertyName = "PatternValue";
            this.patternValueDataGridViewTextBoxColumn.HeaderText = "PatternValue";
            this.patternValueDataGridViewTextBoxColumn.Name = "patternValueDataGridViewTextBoxColumn";
            this.patternValueDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // patternBindingSource
            // 
            this.patternBindingSource.DataSource = typeof(LogViewer.Entities.Pattern);
            // 
            // PatternCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvPatterns);
            this.Name = "PatternCtrl";
            this.Size = new System.Drawing.Size(698, 493);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPatterns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.patternBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPatterns;
        private System.Windows.Forms.DataGridViewTextBoxColumn patternNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn patternValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource patternBindingSource;
    }
}
