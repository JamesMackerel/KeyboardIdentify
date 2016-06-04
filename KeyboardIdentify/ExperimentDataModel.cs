using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

using System.ComponentModel;

namespace KeyboardIdentify
{
    public class ExperimentDataModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int id;
        private int exp_id;
        private string data;

        public ExperimentDataModel()
        {

        }

        public ExperimentDataModel(int? id, string data, int exp_id)
        {
            this.id = id==null? -1:(int)id;
            this.exp_id = exp_id;
            this.data = data;
        }

        public int ExpId
        {
            get
            {
                return exp_id;
            }

            set
            {
                exp_id = value;
            }
        }

        public string Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Data"));
            }
        }

        public int Id
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
            cmd.CommandText = "INSERT INTO [data] (data, experiment_id) VALUES(@data, @exp_id)";
            cmd.Parameters.AddWithValue("@data", data);
            cmd.Parameters.AddWithValue("@exp_id", exp_id);

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
            cmd.CommandText = "DELETE FROM [data] WHERE id=@id";
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
