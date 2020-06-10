using Npgsql;
using System;

namespace CodeMeasurement.Measurements.StorageObjects
{
    class DatabaseManagement
    {
        string connectionString, cultureName;
        DateTime localDate;

        public DatabaseManagement(string connectionString)
        {
            this.connectionString = connectionString;
            cultureName = "de-DE";
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

        public string saveProject(string name, string email, string sourceName)
        {
            string result = "";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sourceId = getSourceIdFromName(connection, sourceName);
                    var sqlStatement = "INSERT INTO project(project_id, name, creation_date, last_update_date, email, source_id)" +
                        "values(DEFAULT, @name, @creation_date, @last_update_date, @email, @source_id)" +
                        "RETURNING project_id";
                    var sqlCommand = new NpgsqlCommand(sqlStatement, connection);
                    sqlCommand.Parameters.AddWithValue("name", name);
                    sqlCommand.Parameters.AddWithValue("creation_date", localDate.ToString(cultureName));
                    sqlCommand.Parameters.AddWithValue("last_update_date", localDate.ToString(cultureName));
                    sqlCommand.Parameters.AddWithValue("email", email);
                    sqlCommand.Parameters.AddWithValue("source_id", sourceId);
                    var execution = sqlCommand.ExecuteScalar();
                    result = execution.ToString();
                    connection.Close();
                } catch {}
            }
            return result;
        }

        private string getSourceIdFromName(NpgsqlConnection connection, string sourceName)
        {
            string sourceId;
            using (connection)
            {
                var sqlStatement = "SELECT source_id FROM project_source WHERE source_name = @source_name";
                var sqlCommand = new NpgsqlCommand(sqlStatement, connection);
                sqlCommand.Parameters.AddWithValue("source_name", sourceName);
                try
                {
                    var execution = sqlCommand.ExecuteScalar();
                    sourceId = execution.ToString();
                }
                catch
                {
                    sourceId = "";
                }
            }
            return sourceId;
        }

        public bool saveMetrics(GeneralMetric generalMetric, int project_id)
        {
            bool result = true;
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    var sqlStatement = "UPDATE project SET last_update_date = @last_update_date WHERE project_id = @project_id";
                    var sqlCommand = new NpgsqlCommand(sqlStatement, connection);
                    sqlCommand.Parameters.AddWithValue("last_update_date", localDate.ToString(cultureName));
                    sqlCommand.Parameters.AddWithValue("project_id", project_id.ToString());
                    sqlCommand.ExecuteScalar();
                    result = saveGeneralMetric(connection, generalMetric, project_id);
                }
                catch
                {
                    result = false;
                }
                connection.Close();
            }
            return result;
        }

        private bool saveGeneralMetric(NpgsqlConnection connection, GeneralMetric generalMetric, int project_id)
        {
            bool result = true;

            using (connection)
            {
                var sqlStatement = "INSERT INTO general_metrics(general_metrics_id, project_id, update_date" +
                    "lines_of_code, lines_of_comments, number_of_namespaces, number_of_classes) values(" +
                    "DEFAULT, @project_id, @update_date, @lines_of_code, @lines_of_comments, @number_of_namespaces, @number_of_classes) " +
                    "RETURNING general_metrics_id";
                var sqlCommand = new NpgsqlCommand(sqlStatement, connection);
                sqlCommand.Parameters.AddWithValue("project_id", project_id.ToString());
                sqlCommand.Parameters.AddWithValue("update_date", localDate.ToString(cultureName));
                sqlCommand.Parameters.AddWithValue("lines_of_code", generalMetric.NumberOfLines.ToString());
                sqlCommand.Parameters.AddWithValue("lines_of_comments", generalMetric.NumberOfComments.ToString());
                sqlCommand.Parameters.AddWithValue("number_of_namespaces,", generalMetric.NumberOfNamespaces.ToString());
                sqlCommand.Parameters.AddWithValue("number_of_classes", generalMetric.NumberOfClasses.ToString());
                try
                {
                    var execution = sqlCommand.ExecuteScalar();
                    string generalMetricsId = execution.ToString();
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

        private bool saveClassMetric(NpgsqlConnection connection, ClassMetric classMetric, string generalMetricsId)
        {
            bool result = true;
            using (connection)
            {
                var sqlStatement = "INSERT INTO class_metrics(class_id, general_metrics_id, class_name, lines_of_code, lines_of_comments" +
                    "number_of_childrens, depth_of_inheritance, weighted_methods) values(DEFAULT, @general_metrics_id, @class_name, " +
                    "@lines_of_code, @lines_of_comments, @number_of_childrens, @depth_of_inheritance)" +
                    "RETURNING class_id";
                var sqlCommand = new NpgsqlCommand(sqlStatement, connection);
                sqlCommand.Parameters.AddWithValue("general_metrics_id", generalMetricsId);
                sqlCommand.Parameters.AddWithValue("class_name", classMetric.Name);
                sqlCommand.Parameters.AddWithValue("lines_of_code", classMetric.NumberOfLines.ToString());
                sqlCommand.Parameters.AddWithValue("lines_of_comments", classMetric.NumberOfComments.ToString());
                sqlCommand.Parameters.AddWithValue("number_of_childrens", classMetric.NumberOfChildrens.ToString());
                sqlCommand.Parameters.AddWithValue("depth_of_inheritance", classMetric.DepthOfInheritance.ToString());
                try
                {
                    var execution = sqlCommand.ExecuteScalar();
                    string classMetricsId = execution.ToString();
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

        private bool saveFunctionMetric(NpgsqlConnection connection, FunctionMetric functionMetric, string classMetricsId)
        {
            bool result = true;
            using (connection)
            {
                var sqlStatement = "INSERT INTO method_metrics(class_id, method_name, lines_of_code, lines_of_comments, nested_block_depths)" +
                    "values(@class_id, @method_name, @lines_of_code, @lines_of_comments, @nested_block_depths)";
                var sqlCommand = new NpgsqlCommand(sqlStatement, connection);
                sqlCommand.Parameters.AddWithValue("class_id", classMetricsId);
                sqlCommand.Parameters.AddWithValue("method_name", functionMetric.Name);
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


        public void getProjectData()
        {

        }
    }
}