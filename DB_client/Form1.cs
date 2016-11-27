using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace DB_client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dataGridView2.AllowUserToResizeRows = false;
            dataGridView2.AllowUserToResizeColumns = false;
            this.dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
        }

        private MySqlConnection Connection_method()
        {
            string ConnetStr = "SERVER=localhost;" + "DATABASE=univerdb;" + "UID=root;" + "PASSWORD=;";

            MySqlConnection Connection = new MySqlConnection(ConnetStr);// Создаем соединение

            try
            {
                textBox1.Text = "Успешное соединение!";
                Connection.Open();// Соединяемся
            }
            catch (MySqlException SSDB_Exception)
            {
                // Ошибка - выходим
                textBox1.Text = "Проверьте настройки соединения, не могу соединиться с базой данных!\nОшибка: " + SSDB_Exception.Message;
                //return;
            }

            return Connection;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection Connection = Connection_method();

            string id = textBox2.Text;
            string semestr = textBox3.Text;
            string ocenka = textBox4.Text;
            string dt_exam = dateTimePicker1.Text;
            string predmet = comboBox1.Text;
            string fam = comboBox2.Text;
            string fio = comboBox3.Text;
           

            string sql1 = "select * from discipline";
            MySqlCommand sqlCom1 = new MySqlCommand(sql1, Connection);
            sqlCom1.ExecuteNonQuery();
            MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(sqlCom1);
            DataTable dt1 = new DataTable();
            dataAdapter1.Fill(dt1);

            
            dt1.DefaultView.RowFilter = "name_pred = '" + predmet + " ' ";
            string nom_pred = dt1.DefaultView[0]["nom_pred"].ToString();

            string sql2 = "select * from sprep";
            MySqlCommand sqlCom2 = new MySqlCommand(sql2, Connection);
            sqlCom2.ExecuteNonQuery();
            MySqlDataAdapter dataAdapter2 = new MySqlDataAdapter(sqlCom2);
            DataTable dt2 = new DataTable();
            dataAdapter2.Fill(dt2);


            dt2.DefaultView.RowFilter = "fam = '" + fam + " ' ";
            string nprep = dt2.DefaultView[0]["nprep"].ToString();

            string sql3 = "select * from stud";
            MySqlCommand sqlCom3 = new MySqlCommand(sql3, Connection);
            sqlCom3.ExecuteNonQuery();
            MySqlDataAdapter dataAdapter3 = new MySqlDataAdapter(sqlCom3);
            DataTable dt3 = new DataTable();
            dataAdapter3.Fill(dt3);


            dt3.DefaultView.RowFilter = "fio = '" + fio + " ' ";
            string nomz = dt3.DefaultView[0]["nomz"].ToString();

            string sql = "insert into ocenki values (" + id + "," + semestr + "," + ocenka + ",'" + dt_exam + "'," + nom_pred + "," + nprep + "," + nomz + ")";
            MySqlCommand sqlCom = new MySqlCommand(sql, Connection);
            sqlCom.ExecuteNonQuery();

            Connection.Close();
            
            UpdateGrid();    
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string s = dataGridView1.CurrentRow.Cells[0].Value.ToString();

            MySqlConnection Connection = Connection_method();

            string sql = "delete from ocenki where id = " + s;
            MySqlCommand sqlCom = new MySqlCommand(sql, Connection);// С помощью этого объекта выполняются запросы к БД
            sqlCom.ExecuteNonQuery();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCom);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            
            dataGridView1.DataSource = dt;
            
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }

            Connection.Close();

            UpdateGrid();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            
        }

        private void UpdateGrid()
        {
            MySqlConnection Connection = Connection_method();

            string sql = "select id as 'Номер экзамена',semestr as 'Семестр',ocenka as 'Отметка',dt_exam as 'Дата сдачи экзамена',name_pred as 'Название предмета',fam as 'ФИО преподавателя',fio as 'ФИО студента' from ocenki, stud, discipline, sprep where discipline_nom_pred = nom_pred and sprep_nprep = nprep and stud_nomz = nomz order by id; ";
            MySqlCommand sqlCom = new MySqlCommand(sql, Connection);// С помощью этого объекта выполняются запросы к БД
            sqlCom.ExecuteNonQuery();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCom);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            dataGridView1.DataSource = dt;

            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 100;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 150;
            dataGridView1.Columns[5].Width = 200;
            dataGridView1.Columns[6].Width = 200;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }


            string sql1 = "select name_pred from discipline";
            MySqlCommand sqlCom1 = new MySqlCommand(sql1, Connection);

            comboBox1.Items.Clear();
           
            MySqlDataReader dataReader1 = sqlCom1.ExecuteReader();

            while (dataReader1.Read())
            {
                comboBox1.Items.Add(dataReader1.GetString(0));
            }

            comboBox1.SelectedIndex = 0;
            dataReader1.Close();

            string sql2 = "select fam from sprep";
            MySqlCommand sqlCom2 = new MySqlCommand(sql2, Connection);

            comboBox2.Items.Clear();

            MySqlDataReader dataReader2 = sqlCom2.ExecuteReader();

            while (dataReader2.Read())
            {
                comboBox2.Items.Add(dataReader2.GetString(0));
            }

            comboBox2.SelectedIndex = 0;
            dataReader2.Close();

            string sql3 = "select fio from stud";
            MySqlCommand sqlCom3 = new MySqlCommand(sql3, Connection);

            comboBox3.Items.Clear();

            MySqlDataReader dataReader3 = sqlCom3.ExecuteReader();

            while (dataReader3.Read())
            {
                comboBox3.Items.Add(dataReader3.GetString(0));
            }

            comboBox3.SelectedIndex = 0;

            Connection.Close();

            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            dateTimePicker1.Enabled = true;
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;

            dataGridView1.Focus();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UpdateGrid();

            button4.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MySqlConnection Connection = Connection_method();

            string sql = "select fio as 'Фамилия Имя Отчество',kurs as 'Курс',nom_gr as 'Номер группы' from ocenki  inner join stud on (ocenki.stud_nomz = stud.nomz) where ocenka > 86 group by fio; ";
            MySqlCommand sqlCom = new MySqlCommand(sql, Connection);// С помощью этого объекта выполняются запросы к БД
            sqlCom.ExecuteNonQuery();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCom);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            dataGridView1.DataSource = dt;

            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 100;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 150;
            dataGridView1.Columns[5].Width = 200;
            dataGridView1.Columns[6].Width = 200;

            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }

            Connection.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MySqlConnection Connection = Connection_method();

            string id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            string semestr = textBox3.Text;
            string ocenka = textBox4.Text;
            string dt_exam = dateTimePicker1.Text;
            string predmet = comboBox1.Text;
            string fam = comboBox2.Text;
            string fio = comboBox3.Text;


            string sql1 = "select * from discipline";
            MySqlCommand sqlCom1 = new MySqlCommand(sql1, Connection);
            sqlCom1.ExecuteNonQuery();
            MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(sqlCom1);
            DataTable dt1 = new DataTable();
            dataAdapter1.Fill(dt1);


            dt1.DefaultView.RowFilter = "name_pred = '" + predmet + " ' ";
            string nom_pred = dt1.DefaultView[0]["nom_pred"].ToString();

            string sql2 = "select * from sprep";
            MySqlCommand sqlCom2 = new MySqlCommand(sql2, Connection);
            sqlCom2.ExecuteNonQuery();
            MySqlDataAdapter dataAdapter2 = new MySqlDataAdapter(sqlCom2);
            DataTable dt2 = new DataTable();
            dataAdapter2.Fill(dt2);


            dt2.DefaultView.RowFilter = "fam = '" + fam + " ' ";
            string nprep = dt2.DefaultView[0]["nprep"].ToString();

            string sql3 = "select * from stud";
            MySqlCommand sqlCom3 = new MySqlCommand(sql3, Connection);
            sqlCom3.ExecuteNonQuery();
            MySqlDataAdapter dataAdapter3 = new MySqlDataAdapter(sqlCom3);
            DataTable dt3 = new DataTable();
            dataAdapter3.Fill(dt3);


            dt3.DefaultView.RowFilter = "fio = '" + fio + " ' ";
            string nomz = dt3.DefaultView[0]["nomz"].ToString();

            string sql = "update ocenki set semestr = " + semestr + ",ocenka = " + ocenka + ",dt_exam = '" + dt_exam + "',discipline_nom_pred = " + nom_pred + ",sprep_nprep = " + nprep + ",stud_nomz = " + nomz + " where id = " + id;
            MySqlCommand sqlCom = new MySqlCommand(sql, Connection);
            sqlCom.ExecuteNonQuery();

            Connection.Close();

            UpdateGrid();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < comboBox1.Items.Count; i++)
            {
                if (comboBox1.Items[i].ToString() == dataGridView1.CurrentRow.Cells[4].Value.ToString())
                {
                    comboBox1.SelectedIndex = i;
                }
            }

            for (int i = 0; i < comboBox2.Items.Count; i++)
            {
                if (comboBox2.Items[i].ToString() == dataGridView1.CurrentRow.Cells[5].Value.ToString())
                {
                    comboBox2.SelectedIndex = i;
                }
            }

            for (int i = 0; i < comboBox3.Items.Count; i++)
            {
                if (comboBox3.Items[i].ToString() == dataGridView1.CurrentRow.Cells[6].Value.ToString())
                {
                    comboBox3.SelectedIndex = i;
                }
            }

            textBox2.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void вывестиНазванияПредметовкоторыеСдавалиСтудентыВМКToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2();
            newForm.Show();

            CallBackMy.callbackEventHandler("select name_pred from discipline where nom_pred in (select discipline_nom_pred from ocenki where stud_nomz in (select nomz from stud where faclt_nom_fct in (select nom_fct from faclt where name_fct = 'ВМК')))");
        }

        private void вывестиСпециальностикоторыеЕстьНаВМКToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2();
            newForm.Show();

            CallBackMy.callbackEventHandler("select name_spec from special where nom_special in (select special_nom_special from stud where faclt_nom_fct = '1');");
        }
    }
}
