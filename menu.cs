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
    public partial class menu : Form
    {
        int id;
        MySqlConnection conn;
        MySqlCommand comm;
        MySqlDataAdapter da;
        DataSet ds;
        DataTable dt;
        DataRow dr;
        String points;

        public void connect1()
        {
            String oradb = "datasource=localhost;port=3306;username=root;password=root";
            conn = new MySqlConnection(oradb);
            conn.Open();
        }
        public menu(int userid)
        {
            InitializeComponent();
            id = userid;
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void menu_Load(object sender, EventArgs e)
        {
            connect1();
            comm = new MySqlCommand();
            comm.Connection = conn;
            comm.CommandText = "select fname from database.userinfo1 where userid='" + id + "'";
            comm.CommandType = CommandType.Text;
            MySqlDataReader read = comm.ExecuteReader();
            while (read.Read())
            {
                label1.Text = read["fname"].ToString();
            }
            read.Close();
            conn.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Browse b = new Browse(id);
            b.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Borrow b = new Borrow(id);
            b.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Lend l = new Lend(id);
            l.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 f = new Form1();
            f.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            EditProfile ep = new EditProfile(id);
            ep.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            //connect1();
            //comm = new MySqlCommand();
            //comm.Connection = conn;
            //comm.CommandText = "select points from database.userinfo1 where userid='" + id + "'";
            //comm.CommandType = CommandType.Text;
            //MySqlDataReader read = comm.ExecuteReader();
            //while (read.Read())
            //{
            //    points = read["points"].ToString();
            //}
            //read.Close();
            //conn.Close();
            //MessageBox.Show("Points : "+ points);

            //SqlConnection sqlConnection1 = new SqlConnection("Your Connection String");
            connect1();
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader reader;
            try
            {
                //int pointss = 
                cmd.CommandText = "database.get_points";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                //cmd.Parameters.Add(new MySqlParameter("userid1",MySqlDbType.Int32)).Value=id;
                cmd.Parameters.AddWithValue("userid1",id);
                cmd.Parameters["userid1"].Direction = ParameterDirection.Input;
                //sqlConnection1.Open();

                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    points = reader["points"].ToString();
                    MessageBox.Show("Points : " + points);
                }
                // Data is accessible through the DataReader object here.
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            conn.Close();
        }
        
    }
}
