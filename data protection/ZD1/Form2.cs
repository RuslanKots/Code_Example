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
    public partial class Form2 : Form
    {
     
        public Form2()
        {
            InitializeComponent();
           // this.Text = "Настройки параметров аудита";
       
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           Form3 f3 = new Form3();
            f3.Show();
           
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // создание объекта класса для диалога открытия файла
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            // определение свойств диалогового окна
            // маска имени для отображаемых в окне файлов
            openFileDialog1.Filter = "Все файлы (*.*)|*.*";
            /* автоматическая проверка существования файла с введенным пользователем именем до закрытия диалога */
            openFileDialog1.CheckFileExists = true;
            // выбор в качестве начальной папки текущую
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            // восстановление текущей папки после закрытия диалога
            openFileDialog1.RestoreDirectory = true;
            // если пользователь выбрал файл
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    // сохранение и отображение имени выбранного файла
                    textBox1.Text = openFileDialog1.FileName;
        

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text=="")
                MessageBox.Show("Введите путь к файлу аудита", "Ошибка", MessageBoxButtons.OK,
          MessageBoxIcon.Error);
            else if (textBox1.Text != AppDomain.CurrentDomain.BaseDirectory + "Audit.dat")
                MessageBox.Show("Вы выбрали неверный файл аудита, выберете другой", "Ошибка", MessageBoxButtons.OK,
      MessageBoxIcon.Error);
            else if (checkBox1.Checked==false && checkBox2.Checked==false)
                MessageBox.Show("Выберете  параметр/параметры администрирования", "Ошибка", MessageBoxButtons.OK,
      MessageBoxIcon.Error);
            else
            {
                bool Chek1 = checkBox1.Checked;
                bool Chek2 = checkBox2.Checked;
                Form4 f4 = new Form4(Chek1,Chek2);
                f4.Show();
            }

           


        }

        private void button4_Click(object sender, EventArgs e)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory + "CopyAudit.dat";
            if (!File.Exists(currentDirectory))
            {
                MessageBox.Show("Копия  файла аудита ещё не создана!", "Ошибка", MessageBoxButtons.OK,
    MessageBoxIcon.Error);
                return;
            }

            Form6 f6 = new Form6();
            f6.ShowDialog();
        }

    }
}
