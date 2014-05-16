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
using System.Net.Mail;

namespace lendabook
{
    public partial class Register : Form
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
            byte[] encrypted = crypto.TransformFinalBlock(plaintextbytes,0,plaintextbytes.Length);
            crypto.Dispose();
            return Convert.ToBase64String(encrypted);
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
        //Boolean ans = false;
        String contactno1;
        public void connect1()
        {
            String oradb = "datasource=localhost;port=3306;username=root;password=root";
            conn = new MySqlConnection(oradb);
            conn.Open();
        }
        public Register()
        {
            InitializeComponent();
        }

        private void Register_Load(object sender, EventArgs e)
        {
            
        }

        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            String username = textBox1.Text;
            String password = Encrypt(textBox2.Text);
            String fname = textBox4.Text;
            String lname = textBox5.Text;
            String email = textBox6.Text;
            String contactno1 = textBox7.Text;
            String houseno = textBox8.Text;
            String streetno = textBox9.Text;
            String city = textBox10.Text;
            String State = textBox11.Text;
            String contactno2 = textBox12.Text;

            try
            {

                if (!email.Contains('.') || !email.Contains('@') || username.Equals("Username *") || password.Equals("Password")
                    || fname.Equals("First Name *") || lname.Equals("Last Name *") || email.Equals("E-Mail *")
                    || houseno.Equals("House No. *") || streetno.Equals(" Street No. *") || city.Equals("City *")
                    || State.Equals("State *") || username.Equals("") || password.Equals("") || fname.Equals("") || lname.Equals("")
                    || email.Equals("") || houseno.Equals("") || streetno.Equals("") || city.Equals("") || State.Equals("")
                    || IsValid(email) == false || !(contactno1.Length == 10))
                {

                    MessageBox.Show("Enter Details Correctly!");

                }


                else if (password.Equals(Encrypt(textBox3.Text)))
                {
                    connect1();
                    comm = new MySqlCommand();
                    comm.CommandText = "select * from database.login";
                    ds = new DataSet();
                    da = new MySqlDataAdapter(comm.CommandText, conn);
                    da.Fill(ds, "database.login");
                    dt = ds.Tables["database.login"];
                    i = dt.Rows.Count;
                    int userid = (i + 1);

                    comm1 = new MySqlCommand();
                    comm1.Connection = conn;
                    comm1.CommandText = "insert into database.login(userid,username,password) values ('" + userid + "','" + username + "','" + password + "')";
                    comm1.CommandType = CommandType.Text;
                    comm1.ExecuteNonQuery();
                    // MessageBox.Show("");
                    comm2 = new MySqlCommand();
                    comm2.Connection = conn;
                    comm2.CommandText = "insert into database.userinfo1(userid,email,fname,lname,houseno,streetno,city,state) values ('" + userid + "','" + email + "','" + fname + "','" + lname + "','" + houseno + "','" + streetno + "','" + city + "','" + State + "')";
                    comm2.CommandType = CommandType.Text;
                    comm2.ExecuteNonQuery();
                    //MessageBox.Show("");
                    comm3 = new MySqlCommand();
                    comm3.Connection = conn;
                    comm3.CommandText = "insert into database.userinfo2(userid,contactno) values ('" + userid + "','" + contactno1 + "')";
                    comm3.CommandType = CommandType.Text;
                    comm3.ExecuteNonQuery();
                    // MessageBox.Show("");
                    if (contactno1 != null && !textBox12.Text.Equals("Contact No. 2") && !textBox12.Text.Equals(""))
                    {
                        MySqlCommand comm4 = new MySqlCommand();
                        comm4.Connection = conn;
                        comm4.CommandText = "insert into database.userinfo2(userid,contactno) values ('" + userid + "','" + contactno2 + "')";
                        comm4.CommandType = CommandType.Text;
                        comm4.ExecuteNonQuery();
                        //MessageBox.Show("");
                    }
                    conn.Close();
                    MessageBox.Show("Thanks for Signing up! Please Login with the info. just entered!");
                    this.Close();
                    Form1 f = new Form1();
                    f.Show();
                }
                else
                {
                    MessageBox.Show("Passwords do not match...Please enter the password again!");
                    textBox2.Clear();
                    textBox3.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    ans = true;
        //    contactno1 = textBox7.Text;
        //    textBox7.Clear();
        //    button2.Hide();
        //}

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            this.Close();
            f.Show();

        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //textBox2.Clear();
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            textBox3.Clear();
        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            textBox4.Clear();
        }

        private void textBox5_Click(object sender, EventArgs e)
        {
            textBox5.Clear();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_Click(object sender, EventArgs e)
        {
            textBox6.Clear();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_Click(object sender, EventArgs e)
        {
            textBox7.Clear();
        }

        private void textBox8_Click(object sender, EventArgs e)
        {
            textBox8.Clear();
        }

        private void textBox9_Click(object sender, EventArgs e)
        {
            textBox9.Clear();
        }

        private void textBox10_Click(object sender, EventArgs e)
        {
            textBox10.Clear();
        }

        private void textBox11_Click(object sender, EventArgs e)
        {
            textBox11.Clear();
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
        }

        private void textBox12_Click(object sender, EventArgs e)
        {
            textBox12.Clear();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar>='0' && e.KeyChar<='9'||e.KeyChar=='\b')
                e.Handled=false;
            else
                e.Handled=true;
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
