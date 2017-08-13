using Invoice.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace InvoiceReader.WindowsForm
{
    public partial class Main : Form, IInvoiceView
    {
        public Main()
        {
            InitializeComponent();
        }

        public event EventHandler OpenFile;

        public event EventHandler ExportData;

        private List<object> _data;

        public List<object> Data
        {
            get
            {
                return _data;
            }

            set
            {
                _data = value;
                dataGridView.DataSource = _data;
            }
        }

        public List<object> DataToExport { get; set; }

        public string OpenFileName { get; set; }

        public string ExportFileName { get; set; }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenFileName = openFileDialog.FileName;
                OpenFile?.Invoke(this, e);
            }
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (cbSelectedRows.Checked)
                {
                    DataToExport = (from sr in dataGridView.SelectedRows.Cast<DataGridViewRow>() select sr.DataBoundItem).ToList();
                }
                else
                {
                    DataToExport = _data;
                }

                ExportFileName = saveFileDialog.FileName;

                ExportData?.Invoke(this, e);
            }
        }

        public void ShowPopupMessage(string s)
        {
            MessageBox.Show(s);
        }
    }
}
