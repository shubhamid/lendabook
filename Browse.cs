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
    public partial class Browse : Form
    {
        int k = 0;
        MySqlConnection conn;
        MySqlCommand comm;
        MySqlCommand comm1;
        MySqlCommand comm2 = new MySqlCommand();
        MySqlCommand comm3;
        MySqlDataAdapter da;
        DataSet ds;
        DataTable dt;
        DataRow dr;
        int i;
        Boolean ans = false;
        int id;
        String password;

        //String contactno1;

        public void connect1()
        {
            String oradb = "datasource=localhost;port=3306;username=root;password=root";
            conn = new MySqlConnection(oradb);
            conn.Open();
        }
        public Browse(int userid)
        {
            InitializeComponent();
            id = userid;            
        }

        private void Browse_Load(object sender, EventArgs e)
        {
            label2.Hide();
            textBox2.Hide();
            textBox1.Hide();
            button3.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //textBox1.Text = "Enter " + comboBox1.SelectedItem.ToString();
            
            connect1();
            if(comboBox1.SelectedIndex==0)
            {
                comboBox2.Items.Clear();
                comm = new MySqlCommand();
                comm.Connection = conn;
                comm.CommandText = "select * from database.books1";
                comm.CommandType = CommandType.Text;
                MySqlDataReader read = comm.ExecuteReader();
                while (read.Read())
                {
                    comboBox2.Items.Add(read["bookname"].ToString());
                }
            }
            if (comboBox1.SelectedIndex == 1)
            {
                comboBox2.Items.Clear();
                comm = new MySqlCommand();
                comm.Connection = conn;
                comm.CommandText = "select distinct author from database.books2";
                comm.CommandType = CommandType.Text;
                MySqlDataReader read = comm.ExecuteReader();
                while (read.Read())
                {
                    comboBox2.Items.Add(read["author"].ToString());
                }
            }
            if (comboBox1.SelectedIndex == 4)
            {
                comboBox2.Items.Clear();
                comm = new MySqlCommand();
                comm.Connection = conn;
                comm.CommandText = "select distinct binding from database.books1";
                comm.CommandType = CommandType.Text;
                MySqlDataReader read = comm.ExecuteReader();
                while (read.Read())
                {
                    comboBox2.Items.Add(read["binding"].ToString());
                }
            }
            if (comboBox1.SelectedIndex == 5)
            {
                comboBox2.Items.Clear();
                comm = new MySqlCommand();
                comm.Connection = conn;
                comm.CommandText = "select distinct genre from database.books3";
                comm.CommandType = CommandType.Text;
                MySqlDataReader read = comm.ExecuteReader();
                while (read.Read())
                {
                    comboBox2.Items.Add(read["genre"].ToString());
                }
            }
            if (comboBox1.SelectedIndex == 2) //Price
            {
               comboBox2.Items.Clear();
                comboBox2.Items.Add("between");
                comboBox2.Items.Add(">");
                comboBox2.Items.Add("<");
                comboBox2.Items.Add("=");
            }
            if (comboBox1.SelectedIndex == 3) //PubYear
            {
                comboBox2.Items.Clear();
                comboBox2.Items.Add("between");
                comboBox2.Items.Add(">");
                comboBox2.Items.Add("<");
                comboBox2.Items.Add("=");

               

                /*comboBox2.Items.Clear();
                comm = new OracleCommand();
                comm.Connection = conn;
                comm.CommandText = "select distinct genre from books3";
                comm.CommandType = CommandType.Text;
                OracleDataReader read = comm.ExecuteReader();
                while (read.Read())
                {
                    comboBox2.Items.Add(read["genre"].ToString());
                }*/
            }
            conn.Close();
            button2.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            menu m = new menu(id);
            this.Close();
            m.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            connect1();
            comm1 = new MySqlCommand();
            comm1.Connection = conn;
            if (comboBox2.SelectedItem.ToString().Equals("between"))
            {
                int num = (int.Parse(textBox1.Text));
                if (comboBox1.SelectedItem.ToString().Equals("Price"))
                    comm1.CommandText = "select * from database.books1 natural join database.books2 natural join database.books3 where Points between " + (num / 10).ToString() + " and " + ((int.Parse(textBox2.Text)) / 10).ToString() + "";
                else
                    comm1.CommandText = "select * from database.books1 natural join database.books2 natural join database.books3 where " + comboBox1.SelectedItem.ToString() + " between " + num.ToString() + " and " + (int.Parse(textBox2.Text)).ToString() + "";
            }
            else if (comboBox1.SelectedItem.ToString().Equals("Price"))
            {
                int num = (int.Parse(textBox1.Text));
                //MessageBox.Show("select * from books1 natural join books2 natural join books3 where Points " + comboBox2.SelectedItem.ToString() + " ");
                //MessageBox.Show("" + num.ToString()+ "");
                //MessageBox.Show("select * from books1 natural join books2 natural join books3 where Points " + comboBox2.SelectedItem.ToString() + " " + ((int.Parse(textBox2.Text)) / 10).ToString() + " ");
                num /= 10;
                comm1.CommandText = "select * from database.books1 natural join database.books2 natural join database.books3 where Points " + comboBox2.SelectedItem.ToString() + " " + num.ToString() + " ";
            }
            else if (comboBox1.SelectedItem.ToString().Equals("Pubyear"))
            {
                int num = (int.Parse(textBox1.Text));
                comm1.CommandText = "select * from database.books1 natural join database.books2 natural join database.books3 where Pubyear " + comboBox2.SelectedItem.ToString() + " " + num.ToString() + "";
            }

            else
            {
                comm1.CommandText = "select * from database.books1 natural join database.books2 natural join database.books3 where " + comboBox1.SelectedItem.ToString() + " = '" + comboBox2.SelectedItem.ToString() + "'";
            }

            //MessageBox.Show(comm1.CommandText);
            comm1.CommandType = CommandType.Text;
            MySqlDataReader read1 = comm1.ExecuteReader();
                while (read1.Read())
                {
                    textBox3.Text=(read1["bookid"].ToString());
                    textBox4.Text = (read1["bookname"].ToString());
                    textBox5.Text = (read1["points"].ToString());
                    textBox6.Text = (read1["pubyear"].ToString());
                    textBox7.Text = (read1["author"].ToString());
                }
            
            read1.Close();
            conn.Close();
            if (!(textBox3.Text.Equals(null)))
            {
                //button2.Hide();
                button3.Show();
            }
            else
            {
                MessageBox.Show("No books found.");
            }
            if (textBox3.Text.Equals(""))
            {
                MessageBox.Show("No books found.");
            }
            /*dataGridView1.AutoGenerateColumns = true;
            ds = new DataSet();
            da = new OracleDataAdapter();
            da.SelectCommand = comm1;
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            conn.Dispose();*/

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString().Equals("between"))
            {
                label2.Show();
                textBox2.Show();
            }
            else
            {
                label2.Hide();
                textBox2.Hide();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString().Equals("Price") || comboBox1.SelectedItem.ToString().Equals("Pubyear"))
            {
                textBox1.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            connect1();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
           // comm2 = new OracleCommand();
            comm2.Connection = conn;
            if (comboBox2.SelectedItem.ToString().Equals("between"))
            {
                int num = (int.Parse(textBox1.Text));
                if (comboBox1.SelectedItem.ToString().Equals("Price"))
                    comm2.CommandText = "select * from database.books1 natural join database.books2 natural join database.books3 where Points between " + (num / 10).ToString() + " and " + ((int.Parse(textBox2.Text)) / 10).ToString() + "";
                else
                    comm2.CommandText = "select * from database.books1 natural join database.books2 natural join database.books3 where " + comboBox1.SelectedItem.ToString() + " between " + num.ToString() + " and " + (int.Parse(textBox2.Text)).ToString() + "";
            }
            else if (comboBox1.SelectedItem.ToString().Equals("Price"))
            {
                int num = (int.Parse(textBox1.Text));
                //MessageBox.Show("select * from books1 natural join books2 natural join books3 where Points " + comboBox2.SelectedItem.ToString() + " ");
                //MessageBox.Show("" + num.ToString()+ "");
                //MessageBox.Show("select * from books1 natural join books2 natural join books3 where Points " + comboBox2.SelectedItem.ToString() + " " + ((int.Parse(textBox2.Text)) / 10).ToString() + " ");
                num /= 10;
                comm2.CommandText = "select * from database.books1 natural join database.books2 natural join database.books3 where Points " + comboBox2.SelectedItem.ToString() + " " + num.ToString() + " ";
            }
            else if (comboBox1.SelectedItem.ToString().Equals("Pubyear"))
            {
                int num = (int.Parse(textBox1.Text));
                comm2.CommandText = "select * from database.books1 natural join database.books2 natural join database.books3 where Pubyear " + comboBox2.SelectedItem.ToString() + " " + num.ToString() + "";
            }

            else
            {
                comm2.CommandText = "select * from database.books1 natural join database.books2 natural join database.books3 where " + comboBox1.SelectedItem.ToString() + " = '" + comboBox2.SelectedItem.ToString() + "'";
            }

            //MessageBox.Show(comm2.CommandText);
            comm2.CommandType = CommandType.Text;
            MySqlDataReader read2 = comm2.ExecuteReader();
            //ds = new DataSet();
            //da = new OracleDataAdapter(comm2.CommandText,conn);
           // da.Fill(ds, "");
            //dt = ds.Tables[""];


            //for (int m = 0; m < dt.Rows.Count; m++)
            while(read2.Read())
            {
                textBox3.Clear();
                textBox4.Clear();
                textBox5.Clear();
                textBox6.Clear();
                textBox7.Clear();
                textBox3.Text = read2["bookid"].ToString();
                textBox4.Text = read2["bookname"].ToString();
                textBox5.Text = read2["points"].ToString();
                textBox6.Text = read2["pubyear"].ToString();
                textBox7.Text = read2["author"].ToString();
                MessageBox.Show("Next Book?");
            }
            read2.Close();

            conn.Close();
        }
    }
}
