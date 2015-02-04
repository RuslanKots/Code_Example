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
    
    public  class Account  // представление информации об учетной записи пользователя программы:
    {  
        public string login; 
        public string passwd;
        public bool restrction;
        public bool blocking;
        public string date;
       
        public string[] AllPass = new string[Program.PassLength];
        public Account() { }
    }
      public static class Class1 
    {
         
         public   static int trying;
       public static List<Account> account = new List<Account>();  // список объектов, доступных по индексу
       public static List<string> Audit = new List<string>(); // список объектов, доступных по индексу
      
          
     
       static public bool EmptyPasswd(string pass) //пустой пароль
        {
            if (pass == "")
                return false;
            return true;
        }

       static public bool ComparePasswd(string pass)
       {
           int t = 0;
           Account compAc =new Account();
           for (int i = 0; i < account.Count; i++)
               for (int j = 0; j < compAc.AllPass.Length; j++)
                   if (pass == compAc.AllPass[i])
                       t++;
           if (t == 0)
               return false;
           else
               return true;
       }

      static public  bool IsNumber(char c) // проверка на символы кирилицы
        {
            if (c >= '1' && c <= '9')
                return true;
            else return false;
        }
      static public bool IsLatin(char c) //проверка на символы латиницы
        {
            if ((c >= 'a') && (c <= 'z'))
                return true;
            else return false;
        }

      static public bool ComplexPasswd(string pass)  // ограничения на вибираемый пароль
        {
            if (!EmptyPasswd(pass))
                return false;
            bool latinaValue = false,  NumberValue = false;
            for (int i = 0; i < pass.Length; i++)
            {
                if (IsLatin(pass[i]))
                    latinaValue = true;
                if (IsNumber(pass[i]))
                    NumberValue = true;
            
            }
            return latinaValue  && NumberValue;
        }

      static public void writeFromFileToList()  //открывает файл в директории и присваивает членам класса нужные значения
        {
            
            string mainFile = AppDomain.CurrentDomain.BaseDirectory + "accountList.dat";
            BinaryReader binar = new BinaryReader(File.Open(mainFile, FileMode.Open));
            account.Clear();
          
            while (binar.PeekChar() >= 0)
            { 
                Account newAccount = new Account();
            
                newAccount.login = binar.ReadString();
                
                newAccount.passwd = binar.ReadString();
                newAccount.restrction = binar.ReadBoolean();
                newAccount.blocking = binar.ReadBoolean();
                newAccount.date = binar.ReadString();
                for (int i = 0; i < Program.PassLength; i++)      //   newAccount.AllPass.Length
                   newAccount.AllPass[i] = binar.ReadString();

                

                account.Add(newAccount);
                
            }
            binar.Close();
        }

      static public void writeFromListToFile() //создает или меняет существующий файл и из файла записывает данные из файла в структуру(класс) 
        {
          

            string mainFile = AppDomain.CurrentDomain.BaseDirectory + "accountList.dat";
            BinaryWriter binaw = new BinaryWriter(File.Open(mainFile, FileMode.Create));

            Account newAccount = new Account();
            for (int i = 0; i < account.Count; i++)
            {
                newAccount = account[i];
                binaw.Write(newAccount.login);
                binaw.Write(newAccount.passwd);
                binaw.Write(newAccount.restrction);
                binaw.Write(newAccount.blocking);
                binaw.Write(newAccount.date);
                for (int j = 0; j<newAccount.AllPass.Length; j++)   
                   binaw.Write(newAccount.AllPass[j]);
                  
            }
            binaw.Close();
        }
      static public void admin() // Создает  новый файл в директории с учетной записью администратора (если этот файл ещё не создан)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory + "accountList.dat";
            if (!File.Exists(currentDirectory))
            {
                DateTime now=DateTime.Now;
                BinaryWriter outBin = new BinaryWriter(File.Open(currentDirectory, FileMode.Create));
                outBin.Write("ADMIN");          // Имя администратора
                outBin.Write("");               // Пароль администратора по умолчанию
                outBin.Write(true);             // Ограничение на пароль (буквы и знаки ар. оп.)
                outBin.Write(false);           // Блокировки нет
                outBin.Write(Convert.ToString(now));
               
               Account newAccount = new Account();
              for (int j = 0; j < newAccount.AllPass.Length; j++)
                    outBin.Write("");
                outBin.Close();
                
            }

            //trying = 3;
        }

      static public void NewAudit() //Создает  новый файл аудита с записью о первом входе администратора(Если этот файл не создан)
      {
          string currentDirectory = AppDomain.CurrentDomain.BaseDirectory + "Audit.dat";
          DateTime now = DateTime.Now;
          
          
          if (!File.Exists(currentDirectory))
          {
              BinaryWriter outBin = new BinaryWriter(File.Open(currentDirectory, FileMode.Create));
              Audit.Add(Convert.ToString(now) + " " + "Создалась учетная запись администратора: ADMIN");
             outBin.Write(Audit[0]);          
              
              outBin.Close();

          }

          //trying = 3;
      }
      static public void writeFromListToAudit() //создает или меняет существующий файл Аудита и из файла записывает данные из файла в структуру(класс) 
      {
          string mainFile = AppDomain.CurrentDomain.BaseDirectory + "Audit.dat";
          BinaryWriter binaw = new BinaryWriter(File.Open(mainFile, FileMode.Create));

         
          for (int i = 0; i < Audit.Count; i++)
          {
              
                  binaw.Write(Audit[i]);
                  
             
              
          }
          binaw.Close();
      }

      static public void writeFromAuditToList()  //открывает файл Аудита в дириктории и присваивает членам класса нужные значения
      {
          string mainFile = AppDomain.CurrentDomain.BaseDirectory + "Audit.dat";
          BinaryReader binar = new BinaryReader(File.Open(mainFile, FileMode.Open));
          Audit.Clear();
          while (binar.PeekChar() >= 0)
          {
              string s;
           
              s = binar.ReadString();
              Audit.Add(s);

          }
          binar.Close();
      }

      static public void writeFromAuditToList1()  //открывает файл в дириктории и присваивает членам класса нужные значения
      {
          string mainFile = AppDomain.CurrentDomain.BaseDirectory + "CopyAudit.dat";
          BinaryReader binar = new BinaryReader(File.Open(mainFile, FileMode.Open));
          Audit.Clear();
          while (binar.PeekChar() >= 0)
          {
              string s;

              s = binar.ReadString();
              Audit.Add(s);

          }
          binar.Close();
      }

   }

}
