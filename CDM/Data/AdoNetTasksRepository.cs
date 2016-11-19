using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace CDM
{
    class AdoNetTasksRepository : ITasksRepository
    {
        string connectionString { get; set; } = ConfigurationManager.ConnectionStrings["CDMTask"].ConnectionString;

        SqlConnection GetConnection()
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception)
            {
                Console.WriteLine("ConnectingExceptionToDatabase");
                return null;
            }
        }

        public bool DeleteTaskById(int id)
        {
            using (SqlConnection connection = GetConnection())
            {
                SqlParameter param = new SqlParameter("@TaskId", id);
                SqlCommand command = new SqlCommand("DELETE FROM Tasks WHERE TaskId = @TaskId", connection);
                command.Parameters.Add(param);
                

                //SqlCommand command = new SqlCommand("DELETE FROM Tasks WHERE TaskId = @TaskId", connection);

                int number = command.ExecuteNonQuery();

                if (number == 1)
                    return true;
                else
                    return false;
            }
        }//+

        public bool DeleteTasks(List<TaskData> tasks)
        {
            using (SqlConnection connection = GetConnection())
            {
                SqlTransaction transaction = connection.BeginTransaction();

                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                command.CommandText = "DELETE FROM Tasks WHERE TaskId = @TaskId";

                SqlParameter paramTaskId = new SqlParameter();
                paramTaskId.ParameterName = "@TaskId";
                command.Parameters.Add(paramTaskId);

                try
                {
                    foreach (TaskData td in tasks)
                    {
                        paramTaskId.Value = td.TaskId;
                        if(command.ExecuteNonQuery() == 0) throw new Exception();
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
                return true;
            }
        }//+

        public List<TaskData> GetAllTasks()
        {
            using (SqlConnection connection = GetConnection())
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Tasks", connection);
                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows) return null;

                List<TaskData> taskData = new List<TaskData> { };
                while(reader.Read())
                {
                    TaskData td = new TaskData();
                    td.TaskId = reader.GetInt32(0);
                    td.TaskText = reader.GetString(1);
                    taskData.Add(td);
                }

                return taskData;
            }
        }//+

        public TaskData GetTaskById(int id)
        {
            using (SqlConnection connection = GetConnection())
            {
                SqlParameter param = new SqlParameter("@TaskId", id);
                
                //SqlCommand command = new SqlCommand(String.Format("SELECT * FROM Tasks WHERE TaskId = {0}", id), connection);
                SqlCommand command = new SqlCommand();
                command.Parameters.Add(param);
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Tasks WHERE TaskId = @TaskId";

                SqlDataReader reader = command.ExecuteReader();
                

                TaskData td = new TaskData();
                while (reader.Read())
                {
                    td.TaskId = reader.GetInt32(0);
                    td.TaskText = reader.GetString(1);
                    return td;
                }
                return null;


            }
        }//+

        public TaskData GetTasksByUser(TaskUser user)
        {
            throw new NotImplementedException();
        }

        public bool UpsertTask(TaskData task)
        {
            using (SqlConnection connection = GetConnection())
            {
                SqlParameter taskIdParam = new SqlParameter("@TaskId", task.TaskId);
                SqlParameter taskTextParam = new SqlParameter("@TaskText", task.TaskText);
                string SqlExUpd = "UPDATE Tasks SET TaskText = @TaskText WHERE TaskId = @TaskId ";
                string SqlExIns = "INSERT INTO Tasks (TaskText) VALUES (@TaskText)";

                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Tasks WHERE TaskId = @TaskId", connection);
                command.Parameters.Add(taskIdParam);
                command.Parameters.Add(taskTextParam);
                object number = command.ExecuteScalar();

                command.CommandText = (int)number == 1 ? SqlExUpd : SqlExIns;
                try
                {
                    command.ExecuteNonQuery();
                    return true;
                }
                catch (Exception)
                {
                    Console.WriteLine("UpsertTaskException");
                    return false;
                }
            }
        }//+
    }
}
