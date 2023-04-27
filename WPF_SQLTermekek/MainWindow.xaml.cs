using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppSQLTermekek
{
    public partial class MainWindow : Window
    {
        private const string kapcsolatLeiro = "datasource=127.0.0.1;port=3306;username=root;password=;database=hardware;charset=utf8;";
        List<Termek> termekek = new List<Termek>();
        MySqlConnection SQLkapcsolat;
        public MainWindow()
        {
            InitializeComponent();

            OpenDatabase();
            LoadKategoriak();
            LoadGyartok();
            LoadTermekToList();

        }

        private void LoadGyartok()
        {
            string SQLGyartokRendezve = "SELECT DISTINCT gyártó FROM termékek ORDER BY gyártó;";
            MySqlCommand SQLparancs = new MySqlCommand(SQLGyartokRendezve, SQLkapcsolat);
            MySqlDataReader reader = SQLparancs.ExecuteReader();

            cbGyarto.Items.Add(" - Nincs megadva - ");
            while (reader.Read())
            {
                cbGyarto.Items.Add(reader.GetString("Gyártó"));
            }
            reader.Close();
            cbGyarto.SelectedIndex = 0;
        }

        private void LoadKategoriak()
        {
            string SQLKategoriakRendezve = "SELECT DISTINCT kategória FROM termékek ORDER BY kategória;";
            MySqlCommand SQLparancs = new MySqlCommand(SQLKategoriakRendezve, SQLkapcsolat);
            MySqlDataReader reader = SQLparancs.ExecuteReader();

            cbKategoria.Items.Add(" - Nincs megadva - ");
            while (reader.Read())
            {
                cbKategoria.Items.Add(reader.GetString("kategória"));
            }
            reader.Close();
            cbKategoria.SelectedIndex = 0;
        }

        private void OpenDatabase()
        {
            try
            {
                SQLkapcsolat = new MySqlConnection(kapcsolatLeiro);
                SQLkapcsolat.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("Nem tud kapcsolódni az adatbázishoz!");
                this.Close();
            }
        }

        private void CloseDatabase()
        {
            SQLkapcsolat.Close();
            SQLkapcsolat.Dispose();
        }

        private void LoadTermekToList()
        {
            string SQLOsszesTermek = "SELECT * FROM termékek";
            MySqlCommand SQLparancs = new MySqlCommand(SQLOsszesTermek, SQLkapcsolat);
            MySqlDataReader reader = SQLparancs.ExecuteReader();

            while (reader.Read())
            {
                Termek uj = new Termek(reader.GetString("Kategória"),
                                        reader.GetString("Gyártó"),
                                        reader.GetString("Név"),
                                        reader.GetInt32("Ár"),
                                        reader.GetInt32("Garidő"));
                termekek.Add(uj);
            }
            reader.Close();
            dgTermekek.ItemsSource = termekek;
        }

        private void btnSzukit_Click(object sender, RoutedEventArgs e)
        {
            termekek.Clear();
            string SQLSzukitettLista = SzukitettLekerdezesEloallitasa();
            MySqlCommand SQLparancs = new MySqlCommand(SQLSzukitettLista, SQLkapcsolat);
            MySqlDataReader reader = SQLparancs.ExecuteReader();
            while (reader.Read())
            {
                Termek uj = new Termek(reader.GetString("Kategória"),
                                        reader.GetString("Gyártó"),
                                        reader.GetString("Név"),
                                        reader.GetInt32("Ár"),
                                        reader.GetInt32("Garidő"));
                termekek.Add(uj);
            }
            reader.Close();
            dgTermekek.Items.Refresh();
        }

        private string SzukitettLekerdezesEloallitasa()
        {
            string query = "SELECT * FROM termékek";

            bool isFirstFilter = true;
            if (cbKategoria.SelectedItem.ToString() != " - Nincs megadva - ")
            {
                query += $" WHERE Kategória = '{cbKategoria.SelectedItem}'";
                isFirstFilter = false;
            }
            if (cbGyarto.SelectedItem.ToString() != " - Nincs megadva - ")
            {
                if (isFirstFilter)
                {
                    query += $" WHERE Gyártó = '{cbGyarto.SelectedItem}'";
                }
                else
                {
                    query += $" AND Gyártó = '{cbGyarto.SelectedItem}'";
                }
            }

            // Create a SQL command based on the constructed query
            string command = query;

            return command;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            CloseDatabase();
        }
    }
}
