using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PDFCounter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<fileItem> fileItems = new List<fileItem>();

        private void button2_Click(object sender, EventArgs e)
        {
            Counter counter = new Counter();
            listView1.Items.Clear();
            string[] files = Directory.GetFiles(textBox1.Text, "*.pdf", SearchOption.AllDirectories);
            progressBar1.Value = 0;
            progressBar1.Maximum = files.Length;            
            foreach (string f in files)
            {
                fileItem item = counter.ReadFile(f);
                fileItems.Add(item);
                progressBar1.Value = progressBar1.Value + 1;
                ListViewItem Item = new ListViewItem(new string[] { item.FileName, item.Count.ToString(), item.ImageCount.ToString(), item.Path });
                listView1.Items.Add(Item);
            }              

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = false;

            if (fbd.ShowDialog(this) == DialogResult.OK)
            {
                textBox1.Text = fbd.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV file|*.csv";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.CheckFileExists = false;
            saveFileDialog.CheckPathExists = true;

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string outfile = saveFileDialog.FileName.ToString();
                StreamWriter sw = new StreamWriter(outfile, false, Encoding.UTF8);
                foreach (fileItem item in fileItems)
                {
                    sw.WriteLine(string.Format(@"""{0}"",""{1}"",""{2}"",""{3}""",item.FileName,item.Count, item.ImageCount,item.Path));
                }
                sw.Close();
                MessageBox.Show(this, "Exported!");
            }
        }
    }
}
