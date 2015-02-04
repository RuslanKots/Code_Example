using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Runtime.InteropServices;




namespace ZD1
{
    public partial  class Form3 : Form
    {
        
      
        public Form3()
        {

            
            InitializeComponent();
            try
            {

                RegistryKey key = Registry.CurrentUser;
                key = key.OpenSubKey("Software\\RegistryTesting");
                System.Object PassMAX = Convert.ToString(key.GetValue("PassMAX"));
                System.Object PassMIN = Convert.ToString(key.GetValue("PassMIN"));
                System.Object PassLength = Convert.ToString(key.GetValue("PassLength"));
                // Получили значения ключей и теперь применяем их к форме 
                textBox2.Text = (string)PassMAX;
                textBox3.Text = (string)PassMIN;
                textBox4.Text = (string)PassLength;
                // MessageBox.Show("Форма восстановлена ."); 

            }
            catch (System.Exception err)
            {
                MessageBox.Show("Произошла ошибка при загрузке параметров " + err.Message);
            } 
          
            Class1.account = new List<Account>();
          
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "accountList.dat"))
            {
                Class1.writeFromFileToList();
                reloadInf();
            }
        }
        int numberUser = 0;
        public void reloadInf()
        {
         
            if (numberUser == Class1.account.Count - 1)
                button3.Enabled = false;
            if (numberUser == 0)
                button2.Enabled = false;
            textBox1.Text = Class1.account[numberUser].login;
            checkBox1.Checked = Class1.account[numberUser].blocking;
            checkBox2.Checked = Class1.account[numberUser].restrction;
            label1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button3.Enabled = true;
            numberUser--;
            reloadInf();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            if (Class1.account[numberUser].login == "ADMIN")
            {
                label1.Text = "Запрещено менять настройки этой записи";
                checkBox1.Checked = false;
                checkBox2.Checked = true;
                return;
            }
           // Class1.account[numberUser].restrction = checkBox2.Checked;
            if (checkBox2.Checked == true && Class1.account[numberUser].restrction == false)
            {
                Class1.Audit.Add(Convert.ToString(now) + " Для пользователя: " + Class1.account[numberUser].login + " были введены ограничения на выбираемый пароль");
                Class1.writeFromListToAudit();
            }
            if (checkBox2.Checked == false && Class1.account[numberUser].restrction == true)
            {
                Class1.Audit.Add(Convert.ToString(now) + " Для пользователя: " + Class1.account[numberUser].login + " были сняты ограничения на  выбираемый пароль");
                Class1.writeFromListToAudit();
            }
           // Class1.account[numberUser].blocking = checkBox1.Checked;
            if (checkBox1.Checked == true && Class1.account[numberUser].blocking == false)
            {
                Class1.Audit.Add(Convert.ToString(now) + " Пользователь: " + Class1.account[numberUser].login + " был заблокирован администратором");
                Class1.writeFromListToAudit();
            }
            if (checkBox1.Checked == false && Class1.account[numberUser].blocking == true)
            {
                Class1.Audit.Add(Convert.ToString(now) + " Пользователь: " + Class1.account[numberUser].login + " был разблокирован администратором");
                Class1.writeFromListToAudit();
            }
           
            Class1.account[numberUser].restrction = checkBox2.Checked;
            Class1.account[numberUser].blocking = checkBox1.Checked;
            Class1.writeFromListToFile();
            MessageBox.Show("Изменения сохранены");

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Form5 f5 = new Form5(Class1.account[0].login);
            f5.Show();
            this.Close();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            Class1.Audit.Add(Convert.ToString(now) + " ADMIN вышел из системы !");
            Class1.writeFromListToAudit();
            Form1 f = new Form1();
            f.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            numberUser++;
            reloadInf();
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Введите имя учетной записи");
               
            }
            string login = textBox1.Text;
            string passwd = "";
            bool restriction = checkBox2.Checked;
            bool blocking = checkBox1.Checked;
            string[] AllPass = new string[Program.PassLength];
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "accountList.dat";
            string date = "";
            // Проверка на наличие в системе учетной записи с таким именем
            BinaryReader binar = new BinaryReader(File.Open(filePath, FileMode.Open));
            while (binar.PeekChar() >= 0)
            {
                login = binar.ReadString();
                /*passwd = binar.ReadString();        
                restriction = binar.ReadBoolean();
                blocking = binar.ReadBoolean();
                date= binar.ReadString();
                for (int j = 0; j < Program.PassLength; j++ )
                    AllPass[j] = binar.ReadString();*/
                if (login == textBox1.Text)
                {
                    binar.Close();
                    MessageBox.Show("Учетная запись с таким именем уже существует");
                    
                    return;
                }
            }
            binar.Close();
            DateTime now = DateTime.Now;
            // Добавление учетной записи
            login = textBox1.Text;
            passwd = "";
            restriction = checkBox2.Checked;
            blocking = checkBox1.Checked;
            date = Convert.ToString(now);

            BinaryWriter binaw = new BinaryWriter(File.Open(filePath, FileMode.Append));
            binaw.Write(login);
            binaw.Write(passwd);
            binaw.Write(restriction);
            binaw.Write(blocking);
            binaw.Write(date);
            for (int j = 0; j < Program.PassLength; j++)
                binaw.Write("");
            binaw.Close();
            Class1.Audit.Add(Convert.ToString(now) + " Был добавлен новый пользователь с ограничениями на используемый пароль: "+login);
            Class1.writeFromListToAudit();
            Form3 f3=new Form3();
            f3.Show();
            this.Close(); 
          // reloadInf();
            
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            
        }

       

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
           string nameUser=textBox1.Text;
            for (int i = 0; i < Class1.account.Count; i++)
                if (Class1.account[i].login == nameUser)
                    for (int j = 0; j < Program.PassLength; j++)
                        if (Class1.account[i].AllPass[j]!="")
                           comboBox1.Items.Add(Class1.account[i].AllPass[j]);                                                  
         
        }

        public  void textBox4_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            Hide();
            Form2 f2 = new Form2();
            f2.ShowDialog();
            //Hide();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
         
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
           
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(this.textBox2.Text) < Convert.ToInt32(this.textBox3.Text))
            {
                MessageBox.Show("Максимальный срок действия пароля, не может быть меньше минимального", "Ошибка", MessageBoxButtons.OK,
          MessageBoxIcon.Error);
                return;
            }
            if (Convert.ToInt32(this.textBox2.Text) == 0 && Convert.ToInt32(this.textBox3.Text) == 0)
            {
                MessageBox.Show("Максимальный и минимальный срок действия пароля не могу равняться нулю", "Ошибка", MessageBoxButtons.OK,
          MessageBoxIcon.Error);
                return;
            }
            if (Convert.ToInt32(this.textBox2.Text) < 0)
            {
                MessageBox.Show("Максимальный срок действия пароля, не может быть меньше нулю", "Ошибка", MessageBoxButtons.OK,
          MessageBoxIcon.Error);
                return;
            }
            if (Convert.ToInt32(this.textBox3.Text)<0 )
            {
                MessageBox.Show("Минимальный срок действия пароля не может быть меньше нулю", "Ошибка", MessageBoxButtons.OK,
          MessageBoxIcon.Error);
                return;
            }
            if (Convert.ToInt32(this.textBox4.Text) <= 0)
            {
                MessageBox.Show("Длина списка хранимых паролей не может быть меньше или равняться нулю", "Ошибка", MessageBoxButtons.OK,
          MessageBoxIcon.Error);
                return;
            }
            try
            {

                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);
                // Открыли папку, true означает - хотим ли мы записывать в этот раздел 
                // реестра ? 
                RegistryKey wKey = key.CreateSubKey("RegistryTesting");
                // Создали новую папку в реестре 
                wKey.SetValue("PassMAX", Convert.ToInt32(this.textBox2.Text));
                wKey.SetValue("PassMIN", Convert.ToInt32(this.textBox3.Text));
                wKey.SetValue("PassLength", Convert.ToInt32(this.textBox4.Text));
                // Здесь мы создали 3 ключа в которых сохранили параметры
                // MessageBox.Show("Параметры сохранены ."); 

            }
            catch (System.Exception err)
            {
                MessageBox.Show("Произошла ошибка при сохранении параметров : " + err.Message);
               
            }
            //MessageBox.Show("Все параметры сохранены и вступят в силу после повторного входа!");
            if (Convert.ToInt32(this.textBox2.Text) < Convert.ToInt32(this.textBox3.Text))
            {
                MessageBox.Show("Максимальный срок действия пароля, не может быть меньше минимального", "Ошибка", MessageBoxButtons.OK,
          MessageBoxIcon.Error);
                return;
            }
            if (Convert.ToInt32(this.textBox2.Text)==0 && Convert.ToInt32(this.textBox3.Text)==0)
            {
                MessageBox.Show("Максимальный и минимальный срок действия пароля не могу равняться нулю", "Ошибка", MessageBoxButtons.OK,
          MessageBoxIcon.Error);
                return;
            }
            if(Program.PassLength<Convert.ToInt32(this.textBox4.Text))
            {
              //  Class1.writeFromFileToList();

                string mainFile = AppDomain.CurrentDomain.BaseDirectory + "accountList.dat";
                BinaryWriter binaw = new BinaryWriter(File.Open(mainFile, FileMode.Create));

                Account newAccount = new Account();
                for (int i = 0; i < Class1.account.Count; i++)
                {
                    newAccount = Class1.account[i];
                    binaw.Write(newAccount.login);
                    binaw.Write(newAccount.passwd);
                    binaw.Write(newAccount.restrction);
                    binaw.Write(newAccount.blocking);
                    binaw.Write(newAccount.date);
                    for (int j = 0; j < Program.PassLength; j++)     
                       binaw.Write(newAccount.AllPass[j]);
                    for (int j = Program.PassLength; j < Convert.ToInt32(this.textBox4.Text); j++)    
                            binaw.Write("");

                }
                binaw.Close();
                DateTime now = DateTime.Now;
                Class1.Audit.Add(Convert.ToString(now) + " Администратор увеличил длину списка хранимых паролей до: " + this.textBox4.Text + " записей");
                
                Class1.writeFromListToAudit();
            }

            else 
            {
                //  Class1.writeFromFileToList();

                DateTime now = DateTime.Now;
                Class1.Audit.Add(Convert.ToString(now) + " Администратор уменьшил длину списка хранимых паролей до: " + this.textBox4.Text + " записей");
                Class1.writeFromListToAudit();

                string mainFile = AppDomain.CurrentDomain.BaseDirectory + "accountList.dat";
                BinaryWriter binaw = new BinaryWriter(File.Open(mainFile, FileMode.Create));

                Account newAccount = new Account();
                for (int i = 0; i < Class1.account.Count; i++)
                {
                    newAccount = Class1.account[i];
                    binaw.Write(newAccount.login);
                    binaw.Write(newAccount.passwd);
                    binaw.Write(newAccount.restrction);
                    binaw.Write(newAccount.blocking);
                    binaw.Write(newAccount.date);
                    for (int j = 0; j < Convert.ToInt32(this.textBox4.Text); j++)
                        if (newAccount.AllPass[j]!="")
                        binaw.Write(newAccount.AllPass[j]);
                    
                        else binaw.Write("");

                }
                binaw.Close();
            }

            DateTime now1 = DateTime.Now;
            if (Program.PassMAX < Convert.ToInt32(this.textBox2.Text))
                Class1.Audit.Add(Convert.ToString(now1) + " Администратор увеличил максимальный срок действия паролей до:  " + this.textBox2.Text + " дней");
            if (Program.PassMIN < Convert.ToInt32(this.textBox3.Text))
                Class1.Audit.Add(Convert.ToString(now1) + " Администратор увеличил минимальный срок действия паролей до:  " + this.textBox3.Text + " дней");
            if (Program.PassMAX > Convert.ToInt32(this.textBox2.Text))
                Class1.Audit.Add(Convert.ToString(now1) + " Администратор уменьшил максимальный срок действия паролей до:  " + this.textBox2.Text + " дней");
            if (Program.PassMIN > Convert.ToInt32(this.textBox3.Text))
                Class1.Audit.Add(Convert.ToString(now1) + " Администратор уменьшил минимальный срок действия паролей до:  " + this.textBox3.Text + " дней");
            Class1.writeFromListToAudit();
            try
            {

                RegistryKey key = Registry.CurrentUser;
                key = key.OpenSubKey("Software\\RegistryTesting");
                System.Object PassMAX = key.GetValue("PassMAX");
                System.Object PassMIN = key.GetValue("PassMIN");
                System.Object PassLength = key.GetValue("PassLength");
                // Получили значения ключей и теперь применяем их к форме 
                Program.PassMAX = (int)PassMAX;
                Program.PassMIN = (int)PassMIN;
                Program.PassLength = (int)PassLength;
                // MessageBox.Show("Форма восстановлена ."); 

            }
            catch (System.Exception err)
            {
                MessageBox.Show("Произошла ошибка при загрузке параметров " + err.Message);
            }
            MessageBox.Show("Ограничения для всех пользователей вступили в силу" );
            /*Form3 f3 = new Form3();
            f3.Show();
            this.Close();*/
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
