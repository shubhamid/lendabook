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


namespace lendabook
{
    
    public partial class Lend : Form
    {
        int id;
        String binding;
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
      
        public void connect1()
        {
            String oradb = "datasource=localhost;port=3306;username=root;password=root";
            conn = new MySqlConnection(oradb);
            conn.Open();
        }
        public Lend(int userid)
        {
            InitializeComponent();
            id = userid;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            menu m = new menu(id);
            this.Close();
            m.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int j=0;
            String bookname = textBox1.Text;
            String author = textBox2.Text;
            int point = (int.Parse(textBox3.Text)) / 10;
            int year = int.Parse(comboBox1.SelectedItem.ToString());
            if(radioButton1.Checked)
               binding=radioButton1.Text;
            if(radioButton2.Checked)
               binding=radioButton2.Text;
            if (binding.Equals("Hard Bound"))
                point += 20;
            if (binding.Equals("Paper Bound"))
                point += 10;
            if (year > 2000)
                point += 10;
            String[] arr = new String[6];
            if (checkBox1.Checked)
                arr[j++] = checkBox1.Text;
            if (checkBox2.Checked)
                arr[j++] = checkBox2.Text;
            if (checkBox3.Checked)
                arr[j++] = checkBox3.Text;
            if (checkBox4.Checked)
                arr[j++] = checkBox4.Text;
            if (checkBox5.Checked)
                arr[j++] = checkBox5.Text;
            if (checkBox6.Checked)
                arr[j++] = checkBox6.Text;
            
            connect1();
            comm = new MySqlCommand();
            comm.CommandText = "select * from database.books1";
            ds = new DataSet();
            da = new MySqlDataAdapter(comm.CommandText, conn);
            da.Fill(ds, "database.books1");
            dt = ds.Tables["database.books1"];
            i = dt.Rows.Count;
            int bookid = (i + 1);
            // MessageBox.Show("");
            comm2 = new MySqlCommand();
            comm2.Connection = conn;
            comm2.CommandText = "insert into database.books1(bookid,bookname,points,binding,pubyear) values ('" + bookid + "','" + bookname + "','" + point + "','" + binding + "','" + year + "')";
            comm2.CommandType = CommandType.Text;
            comm2.ExecuteNonQuery();
            //MessageBox.Show("");
            for (int k = 0; k < j;k++ )
            {
                comm3 = new MySqlCommand();
                comm3.Connection = conn;
                comm3.CommandText = "insert into database.books3(bookid,genre) values ('" + bookid + "','" + arr[k] + "')";
                comm3.CommandType = CommandType.Text;
                comm3.ExecuteNonQuery();
            }
            comm1 = new MySqlCommand();
            comm1.Connection = conn;
            comm1.CommandText = "insert into database.books2(bookid,author) values ('" + bookid + "','" + author + "')";
            comm1.CommandType = CommandType.Text;
            comm1.ExecuteNonQuery();

            if(ans==true && textBox4.Text!=null)
            {
                comm1 = new MySqlCommand();
                comm1.Connection = conn;
                comm1.CommandText = "insert into database.books2(bookid,author) values ('" + bookid + "','" + textBox4.Text + "')";
                comm1.CommandType = CommandType.Text;
                comm1.ExecuteNonQuery();
            }
            //comm1 = new MySqlCommand();
            //comm1.Connection = conn;
            //comm1.CommandText = "insert into database.booklender(bookid,userid) values ('" + bookid + "','" + id + "')";
            //comm1.CommandType = CommandType.Text;
            //comm1.ExecuteNonQuery();
            comm1 = new MySqlCommand();
            comm1.Connection = conn;
            comm1.CommandText = "update database.userinfo1 SET points = points + '" + point + "' where userid = '" + id + "'";
            comm1.CommandType = CommandType.Text;
            comm1.ExecuteNonQuery();
            MessageBox.Show("Thanks for Lending a book! \n" + point + " points have been added to your account! ");
            menu m = new menu(id);
            this.Close();
            m.Show();
    

        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Lend_Load(object sender, EventArgs e)
        {
            for (int i = 1900; i <= 2014; i++)
                comboBox1.Items.Add(i);
            textBox4.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Hide();
            textBox4.Show();
            ans = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
