using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.Security.Cryptography;

namespace lendabook
{
    public partial class EditProfile : Form
    {
        private static string Key = "qcfsdofdskjdbhnjrfgtvbghioshbnhm";
        private static string IV = "pkjmgbhnuhjvagfc";

        private static string Encrypt(string text)
        {
            byte[] plaintextbytes = System.Text.ASCIIEncoding.ASCII.GetBytes(text);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(Key);
            aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            ICryptoTransform crypto = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] encrypted = crypto.TransformFinalBlock(plaintextbytes, 0, plaintextbytes.Length);
            crypto.Dispose();
            return Convert.ToBase64String(encrypted);
        }

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
        MySqlCommand comm1;
        MySqlCommand comm2;
        MySqlCommand comm3;
        MySqlDataAdapter da;
        DataSet ds;
        DataTable dt;
        DataRow dr;
        int i;
        Boolean ans = false;
        Boolean ans2 = false;
        int id;
        String password;

        //String contactno1;

        public void connect1()
        {
            String oradb = "datasource=localhost;port=3306;username=root;password=root";
            conn = new MySqlConnection(oradb);
            conn.Open();
        }
        public EditProfile(int userid)
        {
            InitializeComponent();
            id = userid;
        }

        private void EditProfile_Load(object sender, EventArgs e)
        {
            textBox1.Hide();
            textBox2.Hide();
            textBox3.Hide();
            label1.Hide();
            label2.Hide();
            label3.Hide();
            connect1();

            comm = new MySqlCommand();
            comm.Connection = conn;
            comm.CommandText = "select * from database.userinfo1 where userid='" + id + "'";
            comm.CommandType = CommandType.Text;
            MySqlDataReader read = comm.ExecuteReader();
            String password;
            while (read.Read())
            {
                //password = read["password"].ToString();
                textBox4.Text = read["fname"].ToString();
                textBox5.Text = read["lname"].ToString();
                textBox6.Text = read["email"].ToString();
                textBox8.Text = read["houseno"].ToString();
                textBox9.Text = read["streetno"].ToString();
                textBox10.Text = read["city"].ToString();
                textBox11.Text = read["state"].ToString();
                
            }
            read.Close();
            connect1();
            comm = new MySqlCommand();
            comm.CommandText = "select * from database.userinfo2 where userid='" + id + "'";
            ds=new DataSet();
            da = new MySqlDataAdapter(comm.CommandText, conn);
            da.Fill(ds, "database.userinfo2");
            dt = ds.Tables["database.userinfo2"];
            dr = dt.Rows[0];
            textBox7.Text=dr["contactno"].ToString();
            if (dt.Rows.Count > 1)
            {
                dr = dt.Rows[1];
                textBox12.Text = dr["contactno"].ToString();
            }
            else
                textBox12.Text = "Enter another number";
                
            /*comm1 = new OracleCommand();
            comm1.Connection = conn;
            comm1.CommandText = "select * from userinfo2 where userid='" + id + "'";
            comm1.CommandType = CommandType.Text;
            OracleDataReader read1 = comm.ExecuteReader();
            while (read1.Read())
            {
                textBox7.Text = read1["contactno"].ToString();
                textBox12.Text = read1["contactno"].ToString();
            }
            read1.Close();*/
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Hide();
            textBox1.Show();
            textBox2.Show();
            textBox3.Show();
            label1.Show();
            label2.Show();
            label3.Show();
            connect1();
            comm = new MySqlCommand();
            comm.Connection = conn;
            comm.CommandText = "select * from database.login where userid='" + id + "'";
            comm.CommandType = CommandType.Text;
            MySqlDataReader read1 = comm.ExecuteReader();
           // String password;
            while (read1.Read())
            {
                password = read1["password"].ToString();
            }
            //MessageBox.Show(Decrypt(password));
            read1.Close();
            conn.Close();
            ans = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            menu m = new menu(id);
            this.Close();
            m.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ans == true && password.Equals(Encrypt(textBox1.Text)))
            {
                String password1 = textBox2.Text;
                if (password1 != null && password1.Equals(textBox3.Text))
                {
                    connect1();
                    comm1 = new MySqlCommand();
                    comm1.Connection = conn;
                    password1 = Encrypt(password1);
                    comm1.CommandText = "update database.login SET password = '" + password1 + "' where userid= '" + id + "'";
                    comm1.CommandType = CommandType.Text;
                    comm1.ExecuteNonQuery();
                    conn.Close();
                    ans2 = true;
                }
                else
                {

                    MessageBox.Show("New passwords do not match! Fill the form again.");
                    EditProfile ep1 = new EditProfile(id);
                    this.Hide();
                    ep1.Show();
                    textBox2.Clear();
                    textBox3.Clear();
                }
                
            }
            else if (!password.Equals(Encrypt(textBox1.Text)))
            {
                MessageBox.Show("Current password entered is wrong. Please login again.");
                this.Hide();
                Form1 f11 = new Form1();
                f11.Show();
                ans2 = false;
            }

            if (ans2 == true)
            {
                String fname = textBox4.Text;
                String lname = textBox5.Text;
                String email = textBox6.Text;
                String contactno = textBox7.Text;
                String contactno2 = textBox12.Text;
                String houseno = textBox8.Text;
                String streetno = textBox9.Text;
                String city = textBox10.Text;
                String State = textBox11.Text;
                // MessageBox.Show("");
                connect1();
                comm2 = new MySqlCommand();
                comm2.Connection = conn;
                comm2.CommandText = "update database.userinfo1 SET email='" + email + "', fname= '" + fname + "',lname = '" + lname + "',houseno = '" + houseno + "',streetno = '" + streetno + "',city = '" + city + "',state = '" + State + "' where userid = '" + id + "'";
                comm2.CommandType = CommandType.Text;
                comm2.ExecuteNonQuery();
                //MessageBox.Show("");
                comm3 = new MySqlCommand();
                comm3.Connection = conn;
                comm3.CommandText = "delete from database.userinfo2 where userid = '" + id + "'";
                comm3.CommandType = CommandType.Text;
                comm3.ExecuteNonQuery();
                if (textBox7.Text != null)
                {
                    comm3 = new MySqlCommand();
                    comm3.Connection = conn;
                    comm3.CommandText = "insert into database.userinfo2(userid,contactno) values ('" + id + "','" + contactno + "')";
                    comm3.CommandType = CommandType.Text;
                    comm3.ExecuteNonQuery();
                }

                // MessageBox.Show("");
                if (textBox12.Text != null && textBox12.Text != "Enter another number")
                {
                    comm3 = new MySqlCommand();
                    comm3.Connection = conn;
                    comm3.CommandText = "insert into database.userinfo2(userid,contactno) values ('" + id + "','" + contactno2 + "')";
                    comm3.CommandType = CommandType.Text;
                    comm3.ExecuteNonQuery();
                }
                conn.Close();
                MessageBox.Show("Your Information is updated. Please Sign in Again!");
                this.Hide();
                Form1 f12 = new Form1();
                f12.Show();
            }

         }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '\b')
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void textBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '\b')
                e.Handled = false;
            else
                e.Handled = true;
        }
                
            
    }
}

