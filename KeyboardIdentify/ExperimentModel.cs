using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

using System.Configuration;

using System.ComponentModel;

namespace KeyboardIdentify
{
    public class ExperimentModel : INotifyPropertyChanged
    {
        private int id;
        private string password;

        public ExperimentModel()
        {

        }

        public ExperimentModel(int ID, string password)
        {
            this.id = ID;
            this.password = password;
        }

        public int ID
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Password"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        public void Save()
        {
            SqlConnection conn = new SqlConnection(DatabaseManager.ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO [experiment] (password) VALUES(@password)";
            cmd.Parameters.AddWithValue("@password", password);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        public void Delete()
        {
            SqlConnection conn = new SqlConnection(DatabaseManager.ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "DELETE FROM [experiment] WHERE id=@id";
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
