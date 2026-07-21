using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsProject
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            btnDownload = new Button();
            dgvDocuments = new DataGridView();
            DocID = new DataGridViewTextBoxColumn();
            DocName = new DataGridViewTextBoxColumn();
            dgvAttachments = new DataGridView();
            txtLoanId = new TextBox();
            label2 = new Label();
            txtBorrowerName = new TextBox();
            label1 = new Label();
            dgvLoans = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewTextBoxColumn();
            chkSelect = new DataGridViewCheckBoxColumn();
            FileName = new DataGridViewTextBoxColumn();
            FileType = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDocuments).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvAttachments).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvLoans).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(btnDownload);
            splitContainer1.Panel1.Controls.Add(dgvDocuments);
            splitContainer1.Panel1.Controls.Add(dgvAttachments);
            splitContainer1.Panel1.Controls.Add(txtLoanId);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(txtBorrowerName);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Paint += splitContainer1_Panel1_Paint_1;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dgvLoans);
            splitContainer1.Size = new Size(1001, 505);
            splitContainer1.SplitterDistance = 333;
            splitContainer1.TabIndex = 0;
            // 
            // btnDownload
            // 
            btnDownload.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDownload.Location = new Point(747, 271);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(228, 43);
            btnDownload.TabIndex = 6;
            btnDownload.Text = "Download Selected";
            btnDownload.UseVisualStyleBackColor = true;
            btnDownload.Click += btnDownload_Click;
            // 
            // dgvDocuments
            // 
            dgvDocuments.AllowUserToAddRows = false;
            dgvDocuments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDocuments.Columns.AddRange(new DataGridViewColumn[] { DocID, DocName });
            dgvDocuments.Location = new Point(32, 136);
            dgvDocuments.Name = "dgvDocuments";
            dgvDocuments.ReadOnly = true;
            dgvDocuments.Size = new Size(412, 179);
            dgvDocuments.TabIndex = 5;
            dgvDocuments.CellContentClick += dgvDocuments_CellClick;
            // 
            // DocID
            // 
            DocID.DataPropertyName = "DocumentId";
            DocID.HeaderText = "Document ID";
            DocID.Name = "DocID";
            DocID.ReadOnly = true;
            DocID.Width = 200;
            // 
            // DocName
            // 
            DocName.DataPropertyName = "DocumentName";
            DocName.HeaderText = "Document Name";
            DocName.Name = "DocName";
            DocName.ReadOnly = true;
            DocName.Width = 200;
            // 
            // dgvAttachments
            // 
            dgvAttachments.AllowUserToAddRows = false;
            dgvAttachments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAttachments.Columns.AddRange(new DataGridViewColumn[] { chkSelect, FileName, FileType });
            dgvAttachments.Location = new Point(482, 12);
            dgvAttachments.Name = "dgvAttachments";
            dgvAttachments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAttachments.Size = new Size(493, 253);
            dgvAttachments.TabIndex = 4;
            dgvAttachments.VirtualMode = true;
            dgvAttachments.CellContentClick += dgvAttachments_CellContentClick;
            // 
            // txtLoanId
            // 
            txtLoanId.BorderStyle = BorderStyle.FixedSingle;
            txtLoanId.Location = new Point(137, 14);
            txtLoanId.Multiline = true;
            txtLoanId.Name = "txtLoanId";
            txtLoanId.ReadOnly = true;
            txtLoanId.Size = new Size(307, 41);
            txtLoanId.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(32, 21);
            label2.Name = "label2";
            label2.Size = new Size(81, 30);
            label2.TabIndex = 2;
            label2.Text = "LoanId";
            label2.Click += label2_Click;
            // 
            // txtBorrowerName
            // 
            txtBorrowerName.BorderStyle = BorderStyle.FixedSingle;
            txtBorrowerName.Location = new Point(137, 73);
            txtBorrowerName.Multiline = true;
            txtBorrowerName.Name = "txtBorrowerName";
            txtBorrowerName.ReadOnly = true;
            txtBorrowerName.Size = new Size(307, 41);
            txtBorrowerName.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(32, 79);
            label1.Name = "label1";
            label1.Size = new Size(71, 30);
            label1.TabIndex = 0;
            label1.Text = "Name";
            label1.Click += label1_Click;
            // 
            // dgvLoans
            // 
            dgvLoans.AllowUserToAddRows = false;
            dgvLoans.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLoans.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3 });
            dgvLoans.Location = new Point(32, 3);
            dgvLoans.Name = "dgvLoans";
            dgvLoans.ReadOnly = true;
            dgvLoans.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLoans.Size = new Size(943, 165);
            dgvLoans.TabIndex = 0;
            dgvLoans.CellContentClick += dataGridView1_CellContentClick;
            // 
            // Column1
            // 
            Column1.DataPropertyName = "LoanId";
            Column1.HeaderText = "Loan ID";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.Width = 300;
            // 
            // Column2
            // 
            Column2.DataPropertyName = "BorrowerName";
            Column2.HeaderText = "Borrower Name";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.Width = 300;
            // 
            // Column3
            // 
            Column3.DataPropertyName = "LoanAmount";
            Column3.HeaderText = "Loan Amount";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            Column3.Width = 300;
            // 
            // chkSelect
            // 
            chkSelect.DataPropertyName = "IsSelected";
            chkSelect.HeaderText = "✅";
            chkSelect.Name = "chkSelect";
            chkSelect.Resizable = DataGridViewTriState.True;
            chkSelect.Width = 50;
            // 
            // FileName
            // 
            FileName.HeaderText = "Attachment Name";
            FileName.Name = "FileName";
            FileName.ReadOnly = true;
            FileName.Width = 300;
            // 
            // FileType
            // 
            FileType.HeaderText = "File Type";
            FileType.Name = "FileType";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1001, 505);
            Controls.Add(splitContainer1);
            Name = "Form1";
            Text = "Form1";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDocuments).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvAttachments).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvLoans).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private DataGridView dgvLoans;
        private Label label1;
        private TextBox txtLoanId;
        private Label label2;
        private TextBox txtBorrowerName;
        private DataGridView dgvAttachments;
        private DataGridView dgvDocuments;
        private Button btnDownload;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewTextBoxColumn Column3;
        private DataGridViewTextBoxColumn DocID;
        private DataGridViewTextBoxColumn DocName;
        private DataGridViewCheckBoxColumn chkSelect;
        private DataGridViewTextBoxColumn FileName;
        private DataGridViewTextBoxColumn FileType;
    }
}
