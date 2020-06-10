using Npgsql;
using System;
using System.Collections.Generic;

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

        public bool loginUser(string email, string password) // working
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

        public bool registerUser(string email, string password, string name) // working
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

        public string saveProject(string name, string email, string sourceName) // working
        {
            string result = "";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                    connection.Open();
                    long sourceId = getSourceIdFromName(sourceName);
                    var sqlStatement = "INSERT INTO project(project_id, name, creation_date, last_update_date, email, source_id)" +
                    "values(DEFAULT, @name, @creation_date, @last_update_date, @email, @source_id)" +
                    "RETURNING project_id";
                    var sqlCommand = new NpgsqlCommand(sqlStatement, connection);
                    sqlCommand.Parameters.AddWithValue("name", name);
                    sqlCommand.Parameters.AddWithValue("creation_date", DateTime.Now);
                    sqlCommand.Parameters.AddWithValue("last_update_date", DateTime.Now);
                    sqlCommand.Parameters.AddWithValue("email", email);
                    sqlCommand.Parameters.AddWithValue("source_id", sourceId);

                    var execution = sqlCommand.ExecuteScalar();
                    result = execution.ToString();
                    connection.Close();
            }
            return result;
        }

        private long getSourceIdFromName(string sourceName) // working
        {
            long sourceId = 0;
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sqlStatement = "SELECT source_id FROM project_source WHERE source_name = @source_name";
                var sqlCommand = new NpgsqlCommand(sqlStatement, connection);
                sqlCommand.Parameters.AddWithValue("source_name", sourceName);

                NpgsqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    sourceId = reader.GetInt64(0);
                }
                connection.Close();
            }
            return sourceId;
        }

        public bool saveMetrics(GeneralMetric generalMetric, int project_id) // working
        {
            bool result = true;
            DateTime dateTime = DateTime.Now;
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    var sqlStatement = "UPDATE project SET last_update_date = @last_update_date WHERE project_id = @project_id";
                    var sqlCommand = new NpgsqlCommand(sqlStatement, connection);
                    sqlCommand.Parameters.AddWithValue("last_update_date", dateTime);
                    sqlCommand.Parameters.AddWithValue("project_id", (long) project_id);
                    sqlCommand.ExecuteScalar();
                }
                catch
                {
                    result = false;
                }
                connection.Close();
                result = saveGeneralMetric(generalMetric, project_id, dateTime);
            }
            return result;
        }

        private bool saveGeneralMetric(GeneralMetric generalMetric, int project_id, DateTime dateTime) // working 
        {
            bool result = true;
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sqlStatement = "INSERT INTO general_metrics(general_metrics_id, project_id, update_date," +
                    "lines_of_code, lines_of_comments, number_of_classes) values(" +
                    "DEFAULT, @project_id, @update_date, @lines_of_code, @lines_of_comments, @number_of_classes) " +
                    "RETURNING general_metrics_id";
                var sqlCommand = new NpgsqlCommand(sqlStatement, connection);

                sqlCommand.Parameters.AddWithValue("project_id", (long) project_id);
                sqlCommand.Parameters.AddWithValue("update_date", dateTime);
                sqlCommand.Parameters.AddWithValue("lines_of_code", generalMetric.NumberOfLines);
                sqlCommand.Parameters.AddWithValue("lines_of_comments", generalMetric.NumberOfComments);
                //sqlCommand.Parameters.AddWithValue("namespaces,", generalMetric.NumberOfNamespaces);
                sqlCommand.Parameters.AddWithValue("number_of_classes", generalMetric.NumberOfClasses);
                try
                {
                    string generalMetricsId = sqlCommand.ExecuteScalar().ToString();
                    connection.Close();

                    foreach (ClassMetric classMetric in generalMetric.ClassMetricList)
                    {
                        result = (saveClassMetric(classMetric, generalMetricsId) && result);
                    }
                } catch
                {
                    result = false;
               }
            }
            return result;
        }

        private bool saveClassMetric(ClassMetric classMetric, string generalMetricsId) // working
        {
            bool result = true;
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                var sqlStatement = "INSERT INTO class_metrics(class_id, general_metrics_id, class_name, lines_of_code, lines_of_comments," +
                    "number_of_childrens, depth_of_inheritance, weighted_methods) values(DEFAULT, @general_metrics_id, @class_name, " +
                    "@lines_of_code, @lines_of_comments, @number_of_childrens, @depth_of_inheritance, @weighted_methods)" +
                    "RETURNING class_id";
                var sqlCommand = new NpgsqlCommand(sqlStatement, connection);

                sqlCommand.Parameters.AddWithValue("general_metrics_id", long.Parse(generalMetricsId));
                sqlCommand.Parameters.AddWithValue("class_name", classMetric.Name);
                sqlCommand.Parameters.AddWithValue("lines_of_code", classMetric.NumberOfLines);
                sqlCommand.Parameters.AddWithValue("lines_of_comments", classMetric.NumberOfComments);
                sqlCommand.Parameters.AddWithValue("number_of_childrens", classMetric.NumberOfChildrens);
                sqlCommand.Parameters.AddWithValue("depth_of_inheritance", classMetric.DepthOfInheritance);
                sqlCommand.Parameters.AddWithValue("weighted_methods", classMetric.WeightedMethods);
                try
                {

                var execution = sqlCommand.ExecuteScalar();

                string classMetricsId = execution.ToString();

                connection.Close();
                    foreach (FunctionMetric functionMetric in classMetric.FunctionMetricList)
                    {
                        result = (saveFunctionMetric(functionMetric, classMetricsId) && result);
                    }
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }

        private bool saveFunctionMetric(FunctionMetric functionMetric, string classMetricsId) // working
        {
            bool result = true;
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sqlStatement = "INSERT INTO method_metrics(class_id, method_name, lines_of_code, lines_of_comments, nested_block_depths)" +
                    "values(@class_id, @method_name, @lines_of_code, @lines_of_comments, @nested_block_depths)";
                var sqlCommand = new NpgsqlCommand(sqlStatement, connection);
                sqlCommand.Parameters.AddWithValue("class_id", long.Parse(classMetricsId));
                sqlCommand.Parameters.AddWithValue("method_name", functionMetric.Name);
                sqlCommand.Parameters.AddWithValue("lines_of_code", functionMetric.NumberOfLines);
                sqlCommand.Parameters.AddWithValue("lines_of_comments", functionMetric.NumberOfComments);
                sqlCommand.Parameters.AddWithValue("nested_block_depths", functionMetric.NestedBlockDepth);
                try
                {
                    sqlCommand.ExecuteScalar();
                } catch
                {
                    result = false;
                }
                connection.Close();
            }
            return result;
        }


        public List<ProjectInfo> getEveryProjects(string email) // working
        {
            List<ProjectInfo> projectList = new List<ProjectInfo>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sqlStatement = "SELECT project_id, name, creation_date, last_update_date, source_id " +
                    "FROM project WHERE email = @email";
                var sqlCommand = new NpgsqlCommand(sqlStatement, connection);
                sqlCommand.Parameters.AddWithValue("email", email);
                NpgsqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    projectList.Add(new ProjectInfo((int)reader.GetInt64(0), (int)reader.GetInt64(4), reader.GetDateTime(2),
                          reader.GetDateTime(3), email, reader.GetString(1)));
                }

                foreach (ProjectInfo projectInfo in projectList)
                {
                    projectInfo.source = getSourceName(projectInfo.sourceId);
                }

                connection.Close();
            }

            projectList = getSpecificProjectInfo(projectList);

            return projectList;
        }

        private string getSourceName(int sourceId) // working
        {
            string result = "";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var sqlStatement = "SELECT source_name FROM project_source WHERE source_id = @source_id";
                var sqlCommand = new NpgsqlCommand(sqlStatement, connection);
                sqlCommand.Parameters.AddWithValue("source_id", sourceId);

                var execution = sqlCommand.ExecuteScalar();
                result = execution.ToString();
                connection.Close();
            }

            return result;
        }

        private List<ProjectInfo> getSpecificProjectInfo(List<ProjectInfo> projectInfos)
        {
            foreach (ProjectInfo project in projectInfos) {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    var sqlStatement = "SELECT general_metrics_id, update_date, lines_of_code, lines_of_comments, number_of_classes " +
                        "FROM general_metrics WHERE project_id = @project_id";
                    var sqlCommand = new NpgsqlCommand(sqlStatement, connection);
                    sqlCommand.Parameters.AddWithValue("project_id", (long) project.projectId);
                    NpgsqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        GeneralMetric generalMetric = new GeneralMetric();
                        generalMetric.generalMetricId = (int)reader.GetInt64(0);
                        generalMetric.updateDate = reader.GetDateTime(1).ToString();
                        generalMetric.NumberOfLines = reader.GetInt32(2);
                        generalMetric.NumberOfComments = reader.GetInt32(3);
                        generalMetric.NumberOfClasses = reader.GetInt32(4);
                        generalMetric.NumberOfNamespaces = 3;


                        project.generalMetricList.Add(generalMetric);
                    }       
                    connection.Close();
                }
            }

                return projectInfos;
        }
    }
}