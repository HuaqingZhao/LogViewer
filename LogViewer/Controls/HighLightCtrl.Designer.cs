namespace LogViewer.Controls
{
    partial class HighLightCtrl
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
            this.dgvHighLights = new System.Windows.Forms.DataGridView();
            this.highLightNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.highLightValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.highLightBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHighLights)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.highLightBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvHighLights
            // 
            this.dgvHighLights.AllowUserToAddRows = false;
            this.dgvHighLights.AllowUserToDeleteRows = false;
            this.dgvHighLights.AllowUserToResizeColumns = false;
            this.dgvHighLights.AllowUserToResizeRows = false;
            this.dgvHighLights.AutoGenerateColumns = false;
            this.dgvHighLights.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHighLights.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.highLightNameDataGridViewTextBoxColumn,
            this.highLightValueDataGridViewTextBoxColumn});
            this.dgvHighLights.DataSource = this.highLightBindingSource;
            this.dgvHighLights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHighLights.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dgvHighLights.Location = new System.Drawing.Point(0, 0);
            this.dgvHighLights.MultiSelect = false;
            this.dgvHighLights.Name = "dgvHighLights";
            this.dgvHighLights.ReadOnly = true;
            this.dgvHighLights.RowHeadersVisible = false;
            this.dgvHighLights.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHighLights.Size = new System.Drawing.Size(698, 493);
            this.dgvHighLights.TabIndex = 1;
            this.dgvHighLights.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHighLights_CellContentClick);
            this.dgvHighLights.SelectionChanged += new System.EventHandler(this.dgvHighLights_SelectionChanged);
            this.dgvHighLights.Resize += new System.EventHandler(this.dgvHighLights_Resize);
            // 
            // highLightNameDataGridViewTextBoxColumn
            // 
            this.highLightNameDataGridViewTextBoxColumn.DataPropertyName = "HighLightName";
            this.highLightNameDataGridViewTextBoxColumn.HeaderText = "HighLightName";
            this.highLightNameDataGridViewTextBoxColumn.Name = "highLightNameDataGridViewTextBoxColumn";
            this.highLightNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // highLightValueDataGridViewTextBoxColumn
            // 
            this.highLightValueDataGridViewTextBoxColumn.DataPropertyName = "HighLightValue";
            this.highLightValueDataGridViewTextBoxColumn.HeaderText = "HighLightValue";
            this.highLightValueDataGridViewTextBoxColumn.Name = "highLightValueDataGridViewTextBoxColumn";
            this.highLightValueDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // highLightBindingSource
            // 
            //this.highLightBindingSource.DataSource = typeof(LogViewer.Entities.Pattern);
            // 
            // HighLightCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvHighLights);
            this.Name = "HighLightCtrl";
            this.Size = new System.Drawing.Size(698, 493);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHighLights)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.highLightBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvHighLights;
        private System.Windows.Forms.DataGridViewTextBoxColumn highLightNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn highLightValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource highLightBindingSource;
    }
}
