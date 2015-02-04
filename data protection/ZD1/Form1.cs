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
    public partial class Form1 : Form
    {
        
        
        public Form1()
        { string currentDirectory = AppDomain.CurrentDomain.BaseDirectory + "accountList.dat";
        if (!File.Exists(currentDirectory))
        {

            try
            {

                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);
                // Открыли папку, true означает - хотим ли мы записывать в этот раздел 
                // реестра ? 
                RegistryKey wKey = key.CreateSubKey("RegistryTesting");
                // Создали новую папку в реестре 
                wKey.SetValue("PassMAX", 7);
                wKey.SetValue("PassMIN", 1);
                wKey.SetValue("PassLength", 10);
                // Здесь мы создали 3 ключа в которых сохранили параметры
                // MessageBox.Show("Параметры сохранены ."); 

            }
            catch (System.Exception err)
            {
                MessageBox.Show("Произошла ошибка при сохранении параметров : " + err.Message);

            }

        }
        
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
        
           Class1.account=new List<Account>();
           Class1.Audit = new List<string>();

            Class1.admin();
            Class1.NewAudit();
          //Class1.writeFromFileToList();
            InitializeComponent();
            label3.Text = "";
            Class1.trying = 3;
        }

        private void войтиВСистемуToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {

        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
             FormInfo Info=new FormInfo();
             Info.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit(); ;
        }

        private void FormVxod_Load(object sender, EventArgs e)
        {

        }

        public void label3_Click(object sender, EventArgs e)
        {

        }

        public void button1_Click(object sender, EventArgs e)
        {
          
             

            Class1.writeFromFileToList();
            Class1.writeFromAuditToList();
            DateTime now = DateTime.Now;
            for (int i = 0; i < Class1.account.Count; i++)
                if (Class1.account[i].login == textBox1.Text)
                {
                    if (Class1.account[i].blocking)
                    {
                        
                        MessageBox.Show("Учетная запись заблокирована");
                        Class1.Audit.Add(Convert.ToString(now) + " Пользователь:" + Class1.account[i].login + " не смог войти в систему, так как его учетная запись заблокирована !");
                        Class1.writeFromListToAudit();
                        return;
                    }

                    if (Class1.account[i].passwd != textBox2.Text)
                    {
                        Class1.trying--;
                        textBox2.Clear();
                        MessageBox.Show("Неверный пароль");
                        textBox2.Focus();
                        Class1.Audit.Add(Convert.ToString(now) + " Пользователь: " + Class1.account[i].login + " не смог войти в систему, так как ввел неверный пароль !");
                        Class1.writeFromListToAudit();
                        if (Class1.trying == 0)
                            Application.Exit();
                        return;
                    }
                    if ((DateTime.Now - Convert.ToDateTime(Class1.account[i].date)).TotalDays > Program.PassMAX && Class1.account[i].login != "ADMIN")
                    {

                        
                            MessageBox.Show("Максимальный срок действия вашего пароля истек!");
                            Form5 f5 = new Form5(this.textBox1.Text);
                            f5.ShowDialog();
                            this.Hide();
                            return;
                    }
                    if ((DateTime.Now - Convert.ToDateTime(Class1.account[i].date)).TotalDays > Program.PassMAX && Class1.account[i].login == "ADMIN")
                    {
                        MessageBox.Show("Максимальный срок действия вашего пароля истек!");
                        Form5 f5 = new Form5(this.textBox1.Text);
                        f5.ShowDialog();
                        this.Hide();
                        return;
                        
                    }
                   // if ((DateTime.Now - Convert.ToDateTime(Class1.account[i].date)).TotalDays < Program.PassMAX && (DateTime.Now - Convert.ToDateTime(Class1.account[i].date)).TotalDays > Program.PassMIN && Class1.account[i].login != "ADMIN")
                     //   MessageBox.Show("Смените пароль в ближайшее время, иначе ваша учетная запись может быть заблокирована!За дополнительной информацией обратитесь к администратору", "ВНИМАНИЕ!!!", MessageBoxButtons.OK);
                    

                    // flag = true;
                    Hide();

                    if (Class1.account[i].login == "ADMIN")
                    {
                       // Form3 form3 = new Form3();
                       // form3.Show();
                        if (!Class1.ComplexPasswd(Class1.account[i].passwd))
                        {
                            Form5 form5 = new Form5(Class1.account[i].login);
                            form5.ShowDialog();
                            
                        }
                        else
                        {
                           
                                Class1.Audit.Add(Convert.ToString(now) + " ADMIN вошёл в систему!");
                            
                            Class1.writeFromListToAudit();
                            
                            Form3 form3 = new Form3();
                            form3.Show();

                        }
                        
                    }
                    else
                    {
                        Form5 form5 = new Form5(Class1.account[i].login);
                        form5.Show();
                        if (!Class1.EmptyPasswd(Class1.account[i].passwd))
                        {
                            form5.Close();
                            Form5 form55 = new Form5(Class1.account[i].login);
                            form55.Show();
                            
                        }
                    }
                    
                    
                }
           // MessageBox.Show("Нет  учетной записи с таким именем");
            
            textBox1.Focus();
           // Class1.account.Clear();
            //Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }   
    }
}
