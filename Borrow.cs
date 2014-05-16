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
    public partial class Borrow : Form
    {
        int id;
        MySqlConnection conn;
        MySqlCommand comm;
        MySqlCommand comm1;

        public void connect1()
        {
            String oradb = "datasource=localhost;port=3306;username=root;password=root";
            conn = new MySqlConnection(oradb);
            conn.Open();
        }
        public Borrow(int userid)
        {
            InitializeComponent();
            id = userid;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String ans = "N";
            connect1();
            comm = new MySqlCommand();
            comm.Connection = conn;
            comm.CommandText = "select * from database.books1 where bookid = " + textBox1.Text + " ";
            comm.CommandType = CommandType.Text;
            MySqlDataReader read = comm.ExecuteReader();
            try
            {
                while (read.Read())
                    ans = read["available"].ToString();
                if (ans.Equals("Y") || ans.Equals("y"))
                {
                    int num=int.Parse(textBox1.Text);
                    Payment p = new Payment(num, id);
                    this.Hide();
                    p.Show();
                }
                else
                    MessageBox.Show("The Book with BookId = " + textBox1.Text + " is not available \n Please go to the browse section!");

            }
            catch (MySqlException exc)
            {
                MessageBox.Show("Enter BookId");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            menu m = new menu(id);
            this.Close();
            m.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String ans="N";
            connect1();
            comm = new MySqlCommand();
            comm.Connection=conn;
            comm.CommandText = "select * from database.books1 where bookid = " + textBox1.Text + " ";
            comm.CommandType = CommandType.Text;
            MySqlDataReader read = comm.ExecuteReader();
            try
            {
                while (read.Read())
                    ans = read["available"].ToString();
                if (ans.Equals("Y") || ans.Equals("y"))
                    MessageBox.Show("Yes! The Book is Available...");
                else
                    MessageBox.Show("The Book with BookId = " + textBox1.Text + " is not available \n Please go to the browse section!");

            }
            catch (MySqlException exc)
            {
               MessageBox.Show("Enter BookId");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
