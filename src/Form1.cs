using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Linq;

namespace DllStringEditor
{
    public partial class Form1 : Form
    {
        private byte[] data;
        private List<(int offset, byte[] raw, int originalLength, string type)> strings = new();
        private List<int> filteredIndices = new();
        private int? currentIndex = null;

        public Form1()
        {
            InitializeComponent();

            comboFilter.Items.AddRange(new string[] { "all", "ascii", "utf16" });
            comboFilter.SelectedIndex = 0;

            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            txtSearch.TextChanged += (s, e) => UpdateList();
            comboFilter.SelectedIndexChanged += (s, e) => UpdateList();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                txtSearch.Focus();
                txtSearch.SelectAll();
                e.SuppressKeyPress = true;
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK) return;

            data = File.ReadAllBytes(ofd.FileName);
            ExtractStrings();
            UpdateList();
        }

        private void ExtractStrings()
        {
            strings.Clear();

            foreach (Match m in Regex.Matches(Encoding.ASCII.GetString(data), @"[ -~]{4,}"))
            {
                int offset = m.Index;
                byte[] raw = Encoding.ASCII.GetBytes(m.Value);
                strings.Add((offset, raw, raw.Length, "ascii"));
            }

            for (int i = 0; i < data.Length - 4; i++)
            {
                int start = i;
                List<byte> bytes = new List<byte>();

                int count = 0;

                while (i < data.Length - 1)
                {
                    byte b1 = data[i];
                    byte b2 = data[i + 1];

                    if (b2 == 0x00 && b1 >= 0x20 && b1 <= 0x7E)
                    {
                        bytes.Add(b1);
                        bytes.Add(b2);
                        i += 2;
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (count >= 4)
                {
                    try
                    {
                        string str = Encoding.Unicode.GetString(bytes.ToArray());

                        bool hasLetters = str.Any(c => char.IsLetter(c));
                        bool hasPrintable = str.Count(c => !char.IsControl(c)) >= str.Length * 0.7;

                        if (hasLetters && hasPrintable)
                        {
                            strings.Add((start, bytes.ToArray(), bytes.Count, "utf16"));
                        }
                    }
                    catch
                    {
                    }
                }

                i = start;
            }
        }

        private void UpdateList()
        {
            listBox1.Items.Clear();
            filteredIndices.Clear();

            string search = txtSearch.Text.ToLower();
            string filter = comboFilter.SelectedItem.ToString();

            for (int i = 0; i < strings.Count; i++)
            {
                var (offset, raw, originalLength, type) = strings[i];

                if (filter != "all" && type != filter) continue;

                string text = type == "ascii"
                    ? Encoding.ASCII.GetString(raw)
                    : Encoding.Unicode.GetString(raw);

                if (text.ToLower().Contains(search))
                {
                    filteredIndices.Add(i);
                    listBox1.Items.Add($"{offset:X8} | {type} | {text}");
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1) return;

            currentIndex = filteredIndices[listBox1.SelectedIndex];

            var (offset, raw, originalLength, type) = strings[currentIndex.Value];

            string text = type == "ascii"
                ? Encoding.ASCII.GetString(raw)
                : Encoding.Unicode.GetString(raw);

            txtEdit.Text = text;
            lblHex.Text = BitConverter.ToString(raw).Replace("-", " ");
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (currentIndex == null) return;

            var (offset, oldRaw, originalLength, type) = strings[currentIndex.Value];

            byte[] newBytes;

            if (type == "ascii")
            {
                newBytes = Encoding.ASCII.GetBytes(txtEdit.Text);
            }
            else
            {
                newBytes = Encoding.Unicode.GetBytes(txtEdit.Text);

                int targetLength = newBytes.Length;

                if (targetLength % 2 != 0)
                    targetLength++;

                if (targetLength + 2 <= originalLength)
                    targetLength += 2;

                Array.Resize(ref newBytes, targetLength);

                if (targetLength >= 2)
                {
                    newBytes[targetLength - 2] = 0x00;
                    newBytes[targetLength - 1] = 0x00;
                }
            }

            if (newBytes.Length > originalLength)
            {
                MessageBox.Show("Text too long");
                return;
            }

            byte[] finalBytes = new byte[originalLength];
            Array.Copy(newBytes, finalBytes, newBytes.Length);

            for (int i = newBytes.Length; i < originalLength; i++)
            {
                finalBytes[i] = 0x00;
            }

            if (offset + originalLength > data.Length)
            {
                MessageBox.Show("Out of bounds write prevented");
                return;
            }

            byte[] newData = new byte[data.Length];
            Array.Copy(data, newData, data.Length);

            Array.Copy(finalBytes, 0, newData, offset, originalLength);

            data = newData;

            MessageBox.Show("Applied");

            ExtractStrings();
            UpdateList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (data == null) return;

            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() != DialogResult.OK) return;

            File.WriteAllBytes(sfd.FileName, data);
            MessageBox.Show("Saved");
        }
    }
}