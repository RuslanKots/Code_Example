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
    public partial class Form5 : Form
    {
        string userName;
        public Form5(string UserName)
        {
            InitializeComponent();
            this.Text = "Смена пароля ("+UserName+")";
            userName = UserName;
            label1.Text = "";
         //   contin = false;
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            // Проверка соответствия нового пароля и подтверждения

            if (textBox2.Text != textBox3.Text)
            {
                textBox3.Text = "";
                textBox2.Text = "";
                textBox1.Focus();
                MessageBox.Show("Новый пароль не совпадает с подтверждением");
                
                return;
            }


            Account ac = new Account();
                for (int i = 0; i < Class1.account.Count; i++)
                    for (int j = 0; j < ac.AllPass.Length; j++)
                        if (Class1.account[i].login==userName && this.textBox2.Text == Class1.account[i].AllPass[j])
                        {
                            MessageBox.Show("Вы уже пользовались этим паролем");
                            return;
                        }
            

            Class1.writeFromFileToList();
            for (int i = 0; i < Class1.account.Count; i++)
                if (Class1.account[i].login == userName)
                {
                    if (Class1.account[i].passwd != textBox1.Text)
                    {
                        MessageBox.Show("Неверно введен старый пароль");
                   
                        return;
                    }
                    if (Class1.account[i].restrction)
                    {
                        if (!Class1.ComplexPasswd(textBox2.Text))
                        {
                            textBox3.Text = "";
                            textBox3.Focus();
                            textBox2.Text = "";
                            textBox1.Text = "";
                            MessageBox.Show("Новым паролем  может быть любая  комбинация из символов и цифр");
                            return;
                        }
                    }
                    if ((DateTime.Now - Convert.ToDateTime(Class1.account[i].date)).TotalDays < Program.PassMIN && Class1.account[i].passwd!="")
                    {
                        MessageBox.Show("Минимальный срок действия текущего пароля пользователя "+userName+" ещё не истек");
                        textBox3.Text = "";
                        textBox3.Focus();
                        textBox2.Text = "";
                        textBox1.Text = "";
                            return;
                    }
                   /* else
                    {
                        if (!Class1.EmptyPasswd(textBox1.Text))
                        {
                            label1.Text = "Очень простой пароль";
                            textBox3.Text = "";
                            textBox3.Focus();
                            textBox2.Text = "";
                            textBox1.Text = "";
                            return;
                        }
                    }*/
                    if (Class1.account[i].passwd == textBox1.Text)
                    {
                        MessageBox.Show("Пароль успешно сменен!");
                        Class1.account[i].date = Convert.ToString(now);
                        
                       
                    }
                    Class1.Audit.Add(Convert.ToString(now) + " Пользователь: " + userName  + " Удачно сменил пароль и вышел из системы!");
                    Class1.writeFromListToAudit();
                    Class1.account[i].passwd = textBox3.Text;
                    for (int j = 0; j < Program.PassLength; j++)
                        if (Class1.account[i].AllPass[j] == "")
                        { Class1.account[i].AllPass[j] = Class1.account[i].passwd;
                        break;
                        }
                    Class1.writeFromListToFile();
                    this.Close();
                    Form1 f11 = new Form1();
                    f11.Show();
                }
            return;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            Class1.Audit.Add(Convert.ToString(now) + " Пользователь: " + userName + " Удачно вышел из системы!");
            Class1.writeFromListToAudit();
            Form1 f1 = new Form1();
            f1.Show();
            this.Close();
        }
    }
}
