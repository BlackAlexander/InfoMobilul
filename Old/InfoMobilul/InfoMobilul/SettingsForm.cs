using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace InfoMobilul
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            MainForm formPrinc = new MainForm();
            formPrinc.Show();
            this.Close();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            IncarcaEmail();
        }

        private void IncarcaEmail()
        {
            string conStr = Application.ExecutablePath;
            conStr = conStr.Remove(conStr.Length - 25) + "InfoMobilul.mdf";
            conStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + conStr + @";Integrated Security=True";

            SqlConnection con = new SqlConnection(conStr);
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = String.Format(
                "SELECT IdUtilizator FROM Logari WHERE DataLogare = (SELECT MAX(DataLogare) FROM Logari)"
                );

            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            int IdCurent = Convert.ToInt32(reader[0]);
            reader.Close();

            cmd.CommandText = String.Format(
                "SELECT * FROM Utilizatori WHERE Id = {0}",
                IdCurent
                );

            reader = cmd.ExecuteReader();
            reader.Read();
            label1.Text = Convert.ToString(reader[1]);
            textBox1.Text = Convert.ToString(reader[3]);
            textBox6.Text = Convert.ToString(reader[4]);
            textBox2.Text = Convert.ToString(reader[5]);
            reader.Close();

            con.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "")
            {
                MessageBox.Show("Campurile nu pot fi goale");
                return;
            }

            if(textBox3.Text != textBox4.Text)
            {
                MessageBox.Show("Parolele nu coincid");
                return;
            }

            if(textBox2.Text != "Nespecificat" && textBox2.Text != "M" && textBox2.Text != "F")
            {
                MessageBox.Show("Sexul poate fi doar M, F sau Nespecificat");
                return;
            }

            if(ParolaGresita())
            {
                MessageBox.Show("Parola gresita");
                return;
            }

            ModificaDate();
        }

        private bool ParolaGresita()
        {
            string conStr = Application.ExecutablePath;
            conStr = conStr.Remove(conStr.Length - 25) + "InfoMobilul.mdf";
            conStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + conStr + @";Integrated Security=True";

            SqlConnection con = new SqlConnection(conStr);
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = String.Format(
                "SELECT Parola FROM Utilizatori WHERE Email = '{0}'",
                label1.Text
                );

            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();

            return textBox5.Text != Convert.ToString(reader[0]);
        }

        private void ModificaDate()
        {
            string conStr = Application.ExecutablePath;
            conStr = conStr.Remove(conStr.Length - 25) + "InfoMobilul.mdf";
            conStr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + conStr + @";Integrated Security=True";

            SqlConnection con = new SqlConnection(conStr);
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = String.Format(
                "UPDATE Utilizatori " +
                "SET Parola = '{0}', Nume = '{1}', Prenume = '{2}', Sex = '{3}' " +
                "WHERE Email = '{4}'",
                textBox3.Text, textBox1.Text, textBox6.Text, textBox2.Text, label1.Text
                );
            cmd.ExecuteNonQuery();

            con.Close();

            MessageBox.Show("Date modificate cu succes!");
        }

        private void Button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackgroundImage = InfoMobilul.Properties.Resources.red_yellow;
        }

        private void Button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackgroundImage = InfoMobilul.Properties.Resources.blue_yellow;

        }

        private void Button2_MouseEnter(object sender, EventArgs e)
        {
            button2.BackgroundImage = InfoMobilul.Properties.Resources.red_yellow;

        }

        private void Button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackgroundImage = InfoMobilul.Properties.Resources.blue_yellow;

        }
    }
}
