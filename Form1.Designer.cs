
namespace PartType_OEM
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gui_parts_table = new System.Windows.Forms.DataGridView();
            this.partsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.partdataDataSet = new PartType_OEM.partdataDataSet();
            this.partsTableAdapter = new PartType_OEM.partdataDataSetTableAdapters.partsTableAdapter();
            this.PT_select = new System.Windows.Forms.Button();
            this.partnumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gui_parts_table)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.partsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.partdataDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // gui_parts_table
            // 
            this.gui_parts_table.AllowUserToAddRows = false;
            this.gui_parts_table.AllowUserToDeleteRows = false;
            this.gui_parts_table.AutoGenerateColumns = false;
            this.gui_parts_table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gui_parts_table.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.partnumberDataGridViewTextBoxColumn,
            this.partnameDataGridViewTextBoxColumn});
            this.gui_parts_table.DataSource = this.partsBindingSource;
            this.gui_parts_table.Location = new System.Drawing.Point(12, 12);
            this.gui_parts_table.Name = "gui_parts_table";
            this.gui_parts_table.ReadOnly = true;
            this.gui_parts_table.Size = new System.Drawing.Size(310, 426);
            this.gui_parts_table.TabIndex = 0;
            this.gui_parts_table.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // partsBindingSource
            // 
            this.partsBindingSource.DataMember = "parts";
            this.partsBindingSource.DataSource = this.partdataDataSet;
            // 
            // partdataDataSet
            // 
            this.partdataDataSet.DataSetName = "partdataDataSet";
            this.partdataDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // partsTableAdapter
            // 
            this.partsTableAdapter.ClearBeforeFill = true;
            // 
            // PT_select
            // 
            this.PT_select.Location = new System.Drawing.Point(503, 415);
            this.PT_select.Name = "PT_select";
            this.PT_select.Size = new System.Drawing.Size(134, 23);
            this.PT_select.TabIndex = 1;
            this.PT_select.Text = "Select Type";
            this.PT_select.UseVisualStyleBackColor = true;
            this.PT_select.Click += new System.EventHandler(this.button1_Click);
            // 
            // partnumberDataGridViewTextBoxColumn
            // 
            this.partnumberDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.partnumberDataGridViewTextBoxColumn.DataPropertyName = "partnumber";
            this.partnumberDataGridViewTextBoxColumn.HeaderText = "partnumber";
            this.partnumberDataGridViewTextBoxColumn.Name = "partnumberDataGridViewTextBoxColumn";
            this.partnumberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // partnameDataGridViewTextBoxColumn
            // 
            this.partnameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.partnameDataGridViewTextBoxColumn.DataPropertyName = "partname";
            this.partnameDataGridViewTextBoxColumn.HeaderText = "partname";
            this.partnameDataGridViewTextBoxColumn.Name = "partnameDataGridViewTextBoxColumn";
            this.partnameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 458);
            this.Controls.Add(this.PT_select);
            this.Controls.Add(this.gui_parts_table);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gui_parts_table)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.partsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.partdataDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gui_parts_table;
        private partdataDataSet partdataDataSet;
        private System.Windows.Forms.BindingSource partsBindingSource;
        private partdataDataSetTableAdapters.partsTableAdapter partsTableAdapter;
        private System.Windows.Forms.Button PT_select;
        private System.Windows.Forms.DataGridViewTextBoxColumn partnumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn partnameDataGridViewTextBoxColumn;
    }
}

