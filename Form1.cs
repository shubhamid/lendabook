using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.Security.Cryptography;

namespace lendabook
{
    public partial class Form1 : Form
    {
        private static string Key = "qcfsdofdskjdbhnjrfgtvbghioshbnhm";
        private static string IV = "pkjmgbhnuhjvagfc";

     
        public static string Decrypt(string encrypted)
        {
            byte[] encryptedbytes = Convert.FromBase64String(encrypted);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(Key);
            aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            ICryptoTransform crypto = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] secret = crypto.TransformFinalBlock(encryptedbytes, 0, encryptedbytes.Length);
            crypto.Dispose();
            return System.Text.ASCIIEncoding.ASCII.GetString(secret);
        }

        MySqlConnection conn;
        MySqlCommand comm;
        MySqlDataAdapter da;
        DataSet ds;
        DataTable dt;
        DataRow dr;
        //String [,] a= new String [25,2];
        int i;
        public void connect1()
        {
            String oradb = "datasource=localhost;port=3306;username=root;password=root";
            conn = new MySqlConnection(oradb);
            conn.Open();
        }
        public Form1()
        {
            InitializeComponent();
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            connect1();
            comm = new MySqlCommand();
            comm.CommandText = "select * from database.login";
            ds=new DataSet();
            da = new MySqlDataAdapter(comm.CommandText, conn);
            da.Fill(ds, "login");
            dt=ds.Tables["login"];
            String username=textBox1.Text;
            String password=textBox2.Text;
            for(i=0;i<dt.Rows.Count;i++)
            {
                
                dr=dt.Rows[i];
                String u = dr["username"].ToString();
                String p = dr["password"].ToString();
                int id= int.Parse(dr["userid"].ToString());
                if (username.Equals(u))
                {
                    if (password.Equals(Decrypt(p)))
                    {
                        //MessageBox.Show(""+id);
                        menu m = new menu(id);
                        m.Show();
                        this.Hide();
                        break;
                        
                    }
                }
            }
            if (i >= dt.Rows.Count)
            {
                Dialog1 d = new Dialog1();
                d.Show();
                conn.Close();
            }
           
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Register r = new Register();
            r.Show();
        }
        //protected override void OnFormClosing(FormClosingEventArgs e)
        //{
        //    base.OnFormClosing(e);

        //   // if (e.CloseReason == CloseReason.WindowsShutDown) return;
        //    //Application.Exit();
        //    // Confirm user wants to close
        //    switch (MessageBox.Show(this, "Are you sure you want to close?", "Closing", MessageBoxButtons.YesNo))
        //    {
        //        case DialogResult.Yes:
        //            e.Cancel = false;
        //            Application.Exit();
        //            break;
        //        case DialogResult.No:
        //            e.Cancel = true;
        //            break;

        //        default:
        //            //Application.Exit();
        //            break;
        //    }
        //}

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Do you really want to exit?","Exit"
                ,MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                Application.ExitThread();
            }
            else if (dialog == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = String.Empty; 
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = String.Empty;
        }
    }
}
