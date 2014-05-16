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

namespace lendabook
{
    public partial class Payment : Form
    {
        String booki;
        int bookid;
        int userid;
        String bookname;
        MySqlConnection conn;
        MySqlCommand comm;
        MySqlCommand comm1;
        MySqlCommand comm2;
        MySqlDataAdapter da;
        DataSet ds;
        DataTable dt;
        DataRow dr;
        int i = 0;
        int points;
        int lenderid;

        public void connect1()
        {
            String oradb = "datasource=localhost;port=3306;username=root;password=root";
            conn = new MySqlConnection(oradb);
            conn.Open();
        }

        public Payment(int id, int id2)
        {
            InitializeComponent();
            //MessageBox.Show("" + id);
            bookid = id;
            userid = id2;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "DHL";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "BlueDart";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "DTDC";
        }

        private void label3_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connect1();
            comm2 = new MySqlCommand();
            comm2.Connection = conn;
            comm2.CommandText = "update database.userinfo1 SET points = points - '" + points + "' where userid = '" + userid + "'";
            comm2.CommandType = CommandType.Text;
            comm2.ExecuteNonQuery();
            conn.Close();

            connect1();
            comm2 = new MySqlCommand();
            comm2.Connection = conn;
            comm2.CommandText = "update database.books1 SET available = 'N' where bookid = '" + bookid + "'";
            comm2.CommandType = CommandType.Text;
            comm2.ExecuteNonQuery();
            conn.Close();

            connect1();
            comm = new MySqlCommand();
            comm.CommandText = "select * from database.courier";
            ds = new DataSet();
            da = new MySqlDataAdapter(comm.CommandText, conn);
            da.Fill(ds, "database.courier");
            dt = ds.Tables["database.courier"];
            i = dt.Rows.Count;
            int courierid = (i + 1);
            // MessageBox.Show("");
            comm2 = new MySqlCommand();
            comm2.Connection = conn;
            comm2.CommandText = "insert into database.courier(courierid,couriername,lender_id,borrower_id) values ('" + courierid + "','" + textBox1.Text + "','" + lenderid + "','" + userid + "')";
            comm2.CommandType = CommandType.Text;
            comm2.ExecuteNonQuery();
            comm2 = new MySqlCommand();
            comm2.Connection = conn;
            comm2.CommandText = "insert into database.bookborrow(bookid,userid) values ('" + bookid + "','" + userid + "')";
            comm2.CommandType = CommandType.Text;
            comm2.ExecuteNonQuery();
            comm2 = new MySqlCommand();
            comm2.Connection = conn;
            comm2.CommandText = "insert into database.dispatch(bookid,courierid,userid) values ('" + bookid + "','" + courierid + "','" + userid + "')";
            comm2.CommandType = CommandType.Text;
            comm2.ExecuteNonQuery();

            conn.Close();
            connect1();
            comm1 = new MySqlCommand();
            comm1.Connection = conn;
            comm1.CommandText = "select * from database.books1 where bookid =" + bookid;
            comm1.CommandType = CommandType.Text;
            MySqlDataReader read1 = comm1.ExecuteReader();
            while (read1.Read())
            {
                bookname = ((read1["bookname"]).ToString());
            }
            //label3.Text = "Points to be used : " + points;
            read1.Close();
            conn.Close();

            MessageBox.Show("Thanks for using the Lend A Book System to borrow " + bookname + "! \nIt will take 3-4 days for the courier to deliver the product to your address.\n\nWe look forward to helping you again!");
            this.Hide();
            menu m = new menu(userid);
            m.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            menu m = new menu(userid);
            m.Show();
        }

        private void Payment_Load(object sender, EventArgs e)
        {
            connect1();
            comm = new MySqlCommand();
            comm.Connection = conn;
            comm.CommandText = "select * from database.books1 where bookid = " + bookid;
            comm.CommandType = CommandType.Text;
            MySqlDataReader read = comm.ExecuteReader();
            while (read.Read())
            {
                points = int.Parse((read["points"]).ToString());
            }
            label3.Text = "Points to be used : " + points;
            read.Close();
            conn.Close();

            connect1();
            comm1 = new MySqlCommand();
            comm1.Connection = conn;
            comm1.CommandText = "select * from database.booklender where bookid =" + bookid;
            comm1.CommandType = CommandType.Text;
            MySqlDataReader read1 = comm1.ExecuteReader();
            while (read1.Read())
            {
                lenderid = int.Parse((read1["userid"]).ToString());
            }
            //label3.Text = "Points to be used : " + points;
            read1.Close();
            conn.Close();
        }
    }
}
