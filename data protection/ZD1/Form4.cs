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
using ZD1;

namespace ZD1
{
    public partial class Form4 : Form
    {
        
        public Form4(bool Chek1, bool Chek2)
        {
            InitializeComponent();
           
            Class1.writeFromAuditToList();
        DataGridViewTextBoxColumn firstColumn =
        new DataGridViewTextBoxColumn();
            firstColumn.HeaderText = "АУДИТ";
            firstColumn.Name = "АУДИТ";
            firstColumn.Width = 850;
            DataGridView1.Columns.Add(firstColumn);

            if (Chek1 && Chek2)
            {
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

            else if (Chek1)
            {
                for (int i = 0; i < Class1.Audit.Count; i++)
                {
                    if (Class1.Audit[i].Contains("!"))
                    {
                        DataGridViewCell firstCell =
                        new DataGridViewTextBoxCell();
                        DataGridViewRow row = new DataGridViewRow();
                        firstCell.Value = Class1.Audit[i];
                        row.Cells.AddRange(firstCell);
                        DataGridView1.Rows.Add(row);
                    }

                }
            }

            else
            {
                for (int i = 0; i < Class1.Audit.Count; i++)
                {
                    if (!Class1.Audit[i].Contains("!"))
                    {
                        DataGridViewCell firstCell =
                        new DataGridViewTextBoxCell();
                        DataGridViewRow row = new DataGridViewRow();
                        firstCell.Value = Class1.Audit[i];
                        row.Cells.AddRange(firstCell);
                        DataGridView1.Rows.Add(row);
                    }

                }
            }
            
            
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory + "CopyAudit.dat";
            BinaryWriter outBin = new BinaryWriter(File.Open(currentDirectory, FileMode.Create));
            for (int i = 0; i < DataGridView1.RowCount-1; i++)
            {
                
                    //Class1.Audit.Add(DataGridView1.Rows[i].Cells[0].Value.ToString() + " ");
                    outBin.Write(DataGridView1.Rows[i].Cells[0].Value.ToString() + " ");
                    
            }
            MessageBox.Show("Копия файла аудита успешно создана");

            outBin.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string mainFile = AppDomain.CurrentDomain.BaseDirectory + "CopyAudit.dat";
            
            BinaryWriter outBin = new BinaryWriter(File.Open(mainFile, FileMode.Create));
            outBin.Write("");
            outBin.Close();

        MessageBox.Show("Копия файла аудита очищена");
            return;
        }
    }
}
