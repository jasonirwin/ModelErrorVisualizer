using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestVisualizer
{
    public partial class CodeViewer : Form
    {
      

        public CodeViewer()
        {
            InitializeComponent();
        }

        public CodeViewer(List<ModelStateError> errors)
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = errors;

            DataGridViewColumn nameColumn = new DataGridViewTextBoxColumn();
            nameColumn.DataPropertyName = "PropertyName";
            nameColumn.Name = "Property Name";
            nameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(nameColumn);


            DataGridViewColumn errorColumn = new DataGridViewTextBoxColumn();
            errorColumn.DataPropertyName = "ErrorMessage";
            errorColumn.Name = "Error Message";
            errorColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns.Add(errorColumn);
           




        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
