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
        string connectionString = ConfigurationManager.ConnectionStrings["CDMTask"].ConnectionString;

        public bool DeleteTaskById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(String.Format("DELETE FROM Tasks WHERE TaskId = {0}", id), connection);

                int number = command.ExecuteNonQuery();

                if (number > 0)
                    return true;
                return false;
            }
        }

        public bool DeleteTasks(List<TaskData> tasks)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                try
                {
                    foreach (TaskData td in tasks)
                    {
                        if (!DeleteTaskById(td.TaskId)) throw new Exception();
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    connection.Close();
                }
                return true;
            }
        }

        public List<TaskData> GetAllTasks()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
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
        }

        public TaskData GetTaskById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(String.Format("SELECT * FROM Tasks WHERE TaskId = {0}", id), connection);
                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows) return null;

                TaskData td = new TaskData();
                while (reader.Read())
                {
                    td.TaskId = reader.GetInt32(0);
                    td.TaskText = reader.GetString(1);
                }

                return td;            
            }
        }

        public TaskData GetTasksByUser(TaskUser user)
        {
            throw new NotImplementedException();
        }

        public bool UpsertTask(TaskData task)
        {
            throw new NotImplementedException();
        }
    }
}
