using Npgsql;
using System;

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

        public void saveMetrics(GeneralMetric generalMetric, int project_id)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                saveGeneralMetric(connection, generalMetric, project_id);
                connection.Close();
            }
        }

        private bool saveGeneralMetric(NpgsqlConnection connection, GeneralMetric generalMetric, int project_id)
        {
            bool result = true;
            DateTime localDate = DateTime.Now;
            string cultureName = "de-DE";

            using (connection)
            {
                var sqlStatement = "INSERT INTO general_metrics(general_metrics_id, project_id, update_date" +
                    "lines_of_code, lines_of_comments, number_of_namespaces, number_of_classes) values(" +
                    "DEFAULT, @project_id, @update_date, @lines_of_code, @lines_of_comments, @number_of_namespaces, @number_of_classes)";
                var sqlCommand = new NpgsqlCommand(sqlStatement, connection);
                sqlCommand.Parameters.AddWithValue("project_id", project_id.ToString());
                sqlCommand.Parameters.AddWithValue("update_date", localDate.ToString(cultureName));
                sqlCommand.Parameters.AddWithValue("lines_of_code", generalMetric.NumberOfLines.ToString());
                sqlCommand.Parameters.AddWithValue("lines_of_comments", generalMetric.NumberOfComments.ToString());
                sqlCommand.Parameters.AddWithValue("number_of_namespaces,", generalMetric.NumberOfNamespaces.ToString());
                sqlCommand.Parameters.AddWithValue("number_of_classes", generalMetric.NumberOfClasses.ToString());
                try
                {
                    sqlCommand.ExecuteScalar();
                    // todo get general metrics id
                    int generalMetricsId = 0;
                    //
                    foreach (ClassMetric classMetric in generalMetric.ClassMetricList)
                    {
                        result = (saveClassMetric(connection, classMetric, generalMetricsId) && result);
                    }
                } catch
                {
                    result = false;
                }
            }
            return result;
        }

        private bool saveClassMetric(NpgsqlConnection connection, ClassMetric classMetric, int generalMetricsId)
        {
            bool result = true;
            using (connection)
            {
                var sqlStatement = "INSERT INTO class_metrics(class_id, general_metrics_id, class_name, lines_of_code, lines_of_comments" +
                    "number_of_childrens, depth_of_inheritance, weighted_methods) values(DEFAULT, @general_metrics_id, @class_name, " +
                    "@lines_of_code, @lines_of_comments, @number_of_childrens, @depth_of_inheritance)";
                var sqlCommand = new NpgsqlCommand(sqlStatement, connection);
                sqlCommand.Parameters.AddWithValue("general_metrics_id", generalMetricsId.ToString());
                sqlCommand.Parameters.AddWithValue("class_name", classMetric.name);
                sqlCommand.Parameters.AddWithValue("lines_of_code", classMetric.NumberOfLines.ToString());
                sqlCommand.Parameters.AddWithValue("lines_of_comments", classMetric.NumberOfComments.ToString());
                sqlCommand.Parameters.AddWithValue("number_of_childrens", classMetric.NumberOfChildrens.ToString());
                sqlCommand.Parameters.AddWithValue("depth_of_inheritance", classMetric.DepthOfInheritance.ToString());
                try
                {
                    sqlCommand.ExecuteScalar();
                    // todo get class metrics id
                    int classMetricsId = 0;
                    //
                    foreach(FunctionMetric functionMetric in classMetric.FunctionMetricList)
                    {
                        result = (saveFunctionMetric(connection, functionMetric, classMetricsId) && result);
                    }
                }
                catch
                {
                    result = false;
                }

            }
            return result;
        }

        private bool saveFunctionMetric(NpgsqlConnection connection, FunctionMetric functionMetric, int classMetricsId)
        {
            bool result = true;
            using (connection)
            {
                var sqlStatement = "INSERT INTO method_metrics(class_id, method_name, lines_of_code, lines_of_comments, nested_block_depths)" +
                    "values(@class_id, @method_name, @lines_of_code, @lines_of_comments, @nested_block_depths)";
                var sqlCommand = new NpgsqlCommand(sqlStatement, connection);
                sqlCommand.Parameters.AddWithValue("class_id", classMetricsId);
                sqlCommand.Parameters.AddWithValue("method_name", functionMetric.name);
                sqlCommand.Parameters.AddWithValue("lines_of_code", functionMetric.NumberOfLines.ToString());
                sqlCommand.Parameters.AddWithValue("lines_of_comments", functionMetric.NumberOfComments.ToString());
                sqlCommand.Parameters.AddWithValue("nested_block_depths", functionMetric.NestedBlockDepth.ToString());
                try
                {
                    sqlCommand.ExecuteScalar();
                } catch
                {
                    result = false;
                }
            }
            return result;
        }

        public void saveProject()
        {

        }

        public void getProjectData()
        {

        }
    }
}