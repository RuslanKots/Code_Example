using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZD1
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();

            Class1.writeFromAuditToList1();
            DataGridViewTextBoxColumn firstColumn =
            new DataGridViewTextBoxColumn();
            firstColumn.HeaderText = "АУДИТ";
            firstColumn.Name = "АУДИТ";
            firstColumn.Width = 850;
            DataGridView1.Columns.Add(firstColumn);

            
                for (int i = 0; i < Class1.Audit.Count; i++)
                {
                    DataGridViewCell firstCell =
                    new DataGridViewTextBoxCell();
                    DataGridViewRow row = new DataGridViewRow();
                    firstCell.Value = Class1.Audit[i];
                    row.Cells.AddRange(firstCell);
                    DataGridView1.Rows.Add(row);

                }

            
        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
