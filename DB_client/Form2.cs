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

namespace DB_client
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            CallBackMy.callbackEventHandler = new CallBackMy.callbackEvent(this.Query);
        }

        private MySqlConnection Connection_method()
        {
            string ConnetStr = "SERVER=localhost;" + "DATABASE=univerdb;" + "UID=root;" + "PASSWORD=;";

            MySqlConnection Connection = new MySqlConnection(ConnetStr);// Создаем соединение

            try
            {
                //textBox1.Text = "Успешное соединение!";
                Connection.Open();// Соединяемся
            }
            catch (MySqlException SSDB_Exception)
            {
                // Ошибка - выходим
                //textBox1.Text = "Проверьте настройки соединения, не могу соединиться с базой данных!\nОшибка: " + SSDB_Exception.Message;
                //return;
            }

            return Connection;
        }

        void Select1(string param)
        {
            MySqlConnection Connection = Connection_method();

            string sql = param;
            MySqlCommand sqlCom = new MySqlCommand(sql, Connection);// С помощью этого объекта выполняются запросы к БД
            sqlCom.ExecuteNonQuery();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCom);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            dataGridView1.DataSource = dt;

            dataGridView1.Columns[0].Width = 50;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }

            dataGridView1.Columns[0].Width = 300;

            Connection.Close();
        }

        void Select2(string param)
        {
            MySqlConnection Connection = Connection_method();

            string sql = param;
            MySqlCommand sqlCom = new MySqlCommand(sql, Connection);// С помощью этого объекта выполняются запросы к БД
            sqlCom.ExecuteNonQuery();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sqlCom);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            dataGridView1.DataSource = dt;

            dataGridView1.Columns[0].Width = 50;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }

            dataGridView1.Columns[0].Width = 300;

            Connection.Close();
        }

        void Query(string param)
        {
            if (param == "select name_pred from discipline where nom_pred in (select discipline_nom_pred from ocenki where stud_nomz in (select nomz from stud where faclt_nom_fct in (select nom_fct from faclt where name_fct = 'ВМК')))")
            {
                Select1(param);
            }
            else if (param == "select name_spec from special where nom_special in (select special_nom_special from stud where faclt_nom_fct = '1');")
            {
                Select2(param);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }
    }
}
