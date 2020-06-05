using Npgsql;

namespace CodeMeasurement.Measurements.StorageObjects
{
    class DatabaseManagement
    {
        string connectionString;

        public DatabaseManagement(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool loginUser(string email, string password)
        {
            bool result;
            using (var connection = new NpgsqlConnection(connectionString))
            {          
                connection.Open();
                var sqlStatement = "SELECT * FROM account WHERE email=@email AND password=@password";
                var sqlCommand = new NpgsqlCommand(sqlStatement, connection);
                sqlCommand.Parameters.AddWithValue("email", email);
                sqlCommand.Parameters.AddWithValue("password", password);    
                var execution = sqlCommand.ExecuteScalar();      
                if (execution == null)
                {
                    result = false;
                } else
                {
                    result = true;
                }     
                connection.Close();
            }

            return result;
        }

        public bool registerUser(string email, string password, string name)
        {
            bool result;
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sqlStatement = "INSERT INTO account(email, password, name) VALUES(@email, @password, @name)";
                var sqlCommand = new NpgsqlCommand(sqlStatement, connection);
                sqlCommand.Parameters.AddWithValue("email", email);
                sqlCommand.Parameters.AddWithValue("password", password);
                sqlCommand.Parameters.AddWithValue("name", name);
                try
                {
                    sqlCommand.ExecuteScalar();
                    result = true;
                } catch
                {
                    result = false;
                }
                connection.Close();
            }
            return result;
        }

        public void saveProjectData()
        {
          
        }

        public void getProjectData()
        {

        }
    }
}