using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace DllStringEditor
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
            btnOpen = new Button();
            btnSave = new Button();
            btnApply = new Button();
            txtSearch = new TextBox();
            listBox1 = new ListBox();
            txtEdit = new TextBox();
            lblHex = new Label();
            comboFilter = new ComboBox();

            SuspendLayout();

            btnOpen.Location = new Point(30, 30);
            btnOpen.Size = new Size(75, 23);
            btnOpen.Text = "Open";
            btnOpen.Click += btnOpen_Click;

            btnSave.Location = new Point(120, 30);
            btnSave.Size = new Size(75, 23);
            btnSave.Text = "Save";
            btnSave.Click += btnSave_Click;

            btnApply.Location = new Point(210, 30);
            btnApply.Size = new Size(75, 23);
            btnApply.Text = "Apply";
            btnApply.Click += btnApply_Click;

            txtSearch.Location = new Point(300, 30);
            txtSearch.Size = new Size(300, 23);

            comboFilter.Location = new Point(610, 30);
            comboFilter.Size = new Size(100, 23);

            listBox1.Location = new Point(30, 70);
            listBox1.Size = new Size(680, 200);
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;

            txtEdit.Location = new Point(30, 280);
            txtEdit.Size = new Size(680, 23);

            lblHex.Location = new Point(30, 320);
            lblHex.Size = new Size(680, 23);

            Controls.Add(btnOpen);
            Controls.Add(btnSave);
            Controls.Add(btnApply);
            Controls.Add(txtSearch);
            Controls.Add(comboFilter);
            Controls.Add(listBox1);
            Controls.Add(txtEdit);
            Controls.Add(lblHex);

            ClientSize = new Size(750, 380);
            Text = "DLL String Editor";

            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOpen;
        private Button btnSave;
        private Button btnApply;
        private TextBox txtSearch;
        private ComboBox comboFilter;
        private ListBox listBox1;
        private TextBox txtEdit;
        private Label lblHex;
    }
}
