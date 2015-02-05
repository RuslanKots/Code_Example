using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace vmss_2_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        public string str = ""; // входная строка
        public string res = ""; //результат перевода в постфиксную запись


        public Stack<char> operand = new Stack<char>(); // вспомогательный стек 

        //допускамые переменные: логические операции и литералы
        public char[] all = { 'a', 'b', 'c', 'd', 
                                '1', '0', 
                                '~', 'V', '&', '+', '=', '>', '\\', '|',
                                ')', '(','#' };

        public char[] operations = { '~', 'V', '&', '+', '=', '>', '\\', '|', ')', '(', '#' };

        public char[] liter = { 'a', 'b', 'c', 'd', '1', '0' };



        int variable(char c)
        {
            switch (c)
            {
                case 'a': return 0;
                case 'b': return 1;
                case 'c': return 2;
                case 'd': return 3;
            }
            return -1;
        }
        int prior(char c)
        {
            switch (c)
            {
                case ')': return -1;
                case 'V':
                case '|':
                case '+': return 1;
                case '&':
                case '=':
                case '>':
                case '\\': return 2;
                case '~': return 3;
                case '(': return 4;
                case '#': return -2;
                case '[': return 0;
            }
            return 0;
        }
        string process(char c, Stack<bool> v, Stack<char> o)
        {
            bool f = false;
            bool v1, v2;
            string p = "";
            char ch = '#';
            bool br = false;
            if (c == ')') br = true;
            while ((prior(c) < prior(o.Peek())) || (br))
            {
                ch = o.Peek();
                switch (ch)
                {
                    case ')':
                        o.Pop();
                        break;
                    case 'V':
                        v2 = v.Pop();
                        v1 = v.Pop();
                        v.Push(v1 || v2);
                        o.Pop();
                        p += 'V';
                        break;
                    case '|':
                        v2 = v.Pop();
                        v1 = v.Pop();
                        v.Push(!(v1 && v2));
                        o.Pop();
                        p += '|';
                        break;
                    case '+':
                        v2 = v.Pop();
                        v1 = v.Pop();
                        v.Push(v1 != v2);
                        o.Pop();
                        p += '+';
                        break;
                    case '&':
                        v2 = v.Pop();
                        v1 = v.Pop();
                        v.Push(v1 && v2);
                        o.Pop();
                        p += '&';
                        break;
                    case '=':
                        v2 = v.Pop();
                        v1 = v.Pop();
                        v.Push(v1 == v2);
                        o.Pop();
                        p += '=';
                        break;
                    case '>':
                        v2 = v.Pop();
                        v1 = v.Pop();
                        v.Push((!v1) || v2);
                        o.Pop();
                        p += '>';
                        break;
                    case '\\':
                        v2 = v.Pop();
                        v1 = v.Pop();
                        v.Push(!(v1 || v2));
                        o.Pop();
                        p += '\\';
                        break;
                    case '~':
                        v2 = v.Pop();
                        v.Push(!v2);
                        o.Pop();
                        p += '~';
                        break;
                    case '(': f = true;
                        if (br) { o.Pop(); br = false; c = '('; } else o.Push('[');
                        break;
                    case '[': f = true;
                        if (br) { o.Pop(); o.Pop(); br = false; c = '('; }
                        break;
                    case '#': f = true;
                        break;
                }
                //
            }
            return p;
        }
        string analisator(string s)
        {
            bool[] x; x = new bool[4];
            bool[] r; r = new bool[16];
            Stack<bool> v; v = new Stack<bool>();
            Stack<char> o; o = new Stack<char>();
            string p;
            p = "";
            int k = 0;

            {
                v = new Stack<bool>();
                o = new Stack<char>();
                o.Push('#');
                for (int i = 0; i < s.Count(); i++)
                {
                    k = variable(s[i]);
                    if (s[i] == '1') { v.Push(true); }
                    else
                        if (s[i] == '0') { v.Push(false); }
                        else
                            if (k >= 0) { v.Push(x[k]); p += s[i]; }
                            else
                            {
                                p += process(s[i], v, o);
                                if (s[i] != ')') o.Push(s[i]);
                            }

                }
                p += process('#', v, o);

            }
            return p;
        }

        public bool NOT(bool x) //логическое "НЕ"
        {
            return !x;
        }

        public bool OR(bool x, bool y) // логическое "ИЛИ"
        {
            return (x || y);
        }
        public bool AND(bool x, bool y) // логическое "И"
        {
            return (x && y);
        }

        public bool SUMMOD2(bool x, bool y) // функция разноименности
        {
            if (x ^ y) return false;
            else return true;
        }

        public bool IMPLICATION(bool x, bool y) // импликация 
        {
            if (!x || y) return true;
            else return false;
        }

        public bool EQUAL(bool x, bool y) //эквивалентность
        {
            return IMPLICATION(x, y) && IMPLICATION(y, x);
        }


        public bool PIRS(bool x, bool y) //стрелка Пирса или функция Вебба
        {
            return NOT(OR(x, y));
        }

        public bool SHEFFER(bool x, bool y) // штрих Шеффера
        {
            return OR(NOT(x), NOT(y));
        }





        public void Gen()
        {
            // генерация всех наборов для 4х переменных

            bool fl = false;
            dataGridView1.RowCount = (int)System.Math.Pow(2, 4);
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[4].HeaderText = "F(A,B,C,D)";
            dataGridView1.Columns[0].HeaderText = "A";
            dataGridView1.Columns[1].HeaderText = "B";
            dataGridView1.Columns[2].HeaderText = "C";
            dataGridView1.Columns[3].HeaderText = "D";
            for (int j = 0; j < 4; j++)
            {
                dataGridView1.Columns[j].ReadOnly = true;
                dataGridView1.Columns[j].SortMode = 0;

            }

            dataGridView1.Columns[4].SortMode = 0;
            for (int j = 3; j >= 0; j--)
                for (int i = 0; i < Math.Pow(2, 4); i++)
                {
                    if (fl) dataGridView1[j, i].Value = 1; else dataGridView1[j, i].Value = 0;
                    if ((i + 1) % Math.Pow(2, (4 - j - 1)) == 0) { fl = !fl; }

                }



        }

        public void Postfix() //перевод выражения в постфиксную запись
        {
            res = "";
            res = analisator(str);

            MessageBox.Show("Результат перевода в постфиксную запись    " + res);

            operand.Clear();


        }


        public void Deikstra()
        {

            bool bool1 = true, bool2 = true, bool3 = true;
            char c1, c2; // хранение текущих символов выражения

            for (int j = 0; j < 16; j++)
            {
                operand.Clear();

                if (res.Length == 1) // если было введено ППФ из одного символа
                {
                    switch (res[0])
                    {
                        case 'a': dataGridView1[4, j].Value = dataGridView1[0, j].Value; break;
                        case 'b': dataGridView1[4, j].Value = dataGridView1[1, j].Value; break;
                        case 'c': dataGridView1[4, j].Value = dataGridView1[2, j].Value; break;
                        case 'd': dataGridView1[4, j].Value = dataGridView1[3, j].Value; break;
                        case '1': dataGridView1[4, j].Value = 1; break;
                        case '0': dataGridView1[4, j].Value = 0; break;
                        default: break;
                    }
                    continue;
                }// если более одного
                for (int i = 0; i < res.Length; i++)
                {
                    if (Array.IndexOf(liter, res[i]) >= 0)
                    {
                        operand.Push(res[i]);
                    }
                    else if (res[i] == '~') // обработка отрицания отдельно
                    {
                        c1 = operand.Pop();
                        if (c1 == 'a') bool1 = Convert.ToBoolean(Convert.ToInt32(dataGridView1[0, j].Value));
                        if (c1 == 'b') bool1 = Convert.ToBoolean(Convert.ToInt32(dataGridView1[1, j].Value));
                        if (c1 == 'c') bool1 = Convert.ToBoolean(Convert.ToInt32(dataGridView1[2, j].Value));
                        if (c1 == 'd') bool1 = Convert.ToBoolean(Convert.ToInt32(dataGridView1[3, j].Value));
                        if (c1 == '1') bool1 = true;
                        if (c1 == '0') bool1 = false;
                        bool3 = NOT(bool1);
                        if (bool3) operand.Push('1');
                        else operand.Push('0');
                    }
                    else
                    {

                        c1 = operand.Pop(); // разбирает 2 текущих элемента на вычисление
                        c2 = operand.Pop();

                        if (c1 == 'a') bool1 = Convert.ToBoolean(Convert.ToInt32(dataGridView1[0, j].Value));
                        if (c1 == 'b') bool1 = Convert.ToBoolean(Convert.ToInt32(dataGridView1[1, j].Value));
                        if (c1 == 'c') bool1 = Convert.ToBoolean(Convert.ToInt32(dataGridView1[2, j].Value));
                        if (c1 == 'd') bool1 = Convert.ToBoolean(Convert.ToInt32(dataGridView1[3, j].Value));
                        if (c1 == '1') bool1 = true;
                        if (c1 == '0') bool1 = false;
                        if (c2 == 'a') bool2 = Convert.ToBoolean(Convert.ToInt32(dataGridView1[0, j].Value));
                        if (c2 == 'b') bool2 = Convert.ToBoolean(Convert.ToInt32(dataGridView1[1, j].Value));
                        if (c2 == 'c') bool2 = Convert.ToBoolean(Convert.ToInt32(dataGridView1[2, j].Value));
                        if (c2 == 'd') bool2 = Convert.ToBoolean(Convert.ToInt32(dataGridView1[3, j].Value));
                        if (c2 == '1') bool2 = true;
                        if (c2 == '0') bool2 = false;
                        switch (res[i])
                        {
                            case 'V': bool3 = OR(bool1, bool2); break; // обработка остальных операций
                            case '&': bool3 = AND(bool1, bool2); break;
                            case '>': bool3 = IMPLICATION(bool1, bool2); break;
                            case '+': bool3 = SUMMOD2(bool1, bool2); break;
                            case '=': bool3 = EQUAL(bool1, bool2); break;
                            case '\\': bool3 = SHEFFER(bool1, bool2); break;
                            case '|': bool3 = PIRS(bool1, bool2); break;
                            default: break;
                        }
                        if (bool3) operand.Push('1');
                        else operand.Push('0');

                    }
                }
                dataGridView1[4, j].Value = operand.Pop().ToString();
            }
        }



        private void postfix_Click(object sender, EventArgs e)
        {

            Gen();
            str = TextBox1.Text; // ввод входной строки

            //проверка на пустоту
            if (str.Length == 0) { MessageBox.Show("Введите ППФ!"); return; }


            //перевод в постфиксную запись
            Postfix();

            // вычисление выражения по алгоритму Дейкстры:
            Deikstra();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }




    }
}



