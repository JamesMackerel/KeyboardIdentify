using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

using System.Configuration;

using System.Collections.ObjectModel;

namespace KeyboardIdentify
{
    public class DatabaseManager
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["Data"].ToString();
        
        private static DatabaseManager instance = new DatabaseManager();

        public static DatabaseManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new DatabaseManager();
                }
                return instance;
            }

            private set
            {
                instance = value;
            }
        }

        public ICollection<ExperimentModel> GetExperimentModels()
        {
            ObservableCollection<ExperimentModel> ModelList = new ObservableCollection<ExperimentModel>();

            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT id, password FROM [experiment]";

            try
            {
                conn.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

                foreach(DataRow dr in dt.Rows)
                {
                    ModelList.Add(new ExperimentModel((int)dr["id"], (string)dr["password"]));
                }
            }
            finally
            {
                conn.Close();
            }

            return ModelList;
        }

        public ICollection<ExperimentDataModel> GetExperimentDataModels(int ExperimentID)
        {
            ObservableCollection<ExperimentDataModel> ModelList = new ObservableCollection<ExperimentDataModel>();

            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT (id, data) FROM [data]";

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ModelList.Add(new ExperimentDataModel((int)reader[0], (string)reader[1], (int)reader[2]));
                }
            }
            finally
            {
                conn.Close();
            }

            return ModelList;
        }
    }
}
