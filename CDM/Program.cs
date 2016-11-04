using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace CDM
{
    class Program
    {
        static void Main(string[] args)
        {
            string tasks = ConfigurationManager.ConnectionStrings["CDMTask"].ConnectionString;
            // string connection = "Data Source=DESKTOP-E57JEEP;Initial Catalog=userdb;Integrated Security=True";

            List<TaskData> listTaskData = new List<TaskData> { };
            TaskData td1 = new TaskData { TaskId = 8, TaskText = "E" };
            TaskData td2 = new TaskData { TaskId = 9, TaskText = "F" };
            TaskData td3 = new TaskData { TaskId = 10, TaskText = "G" };
            TaskData td4 = new TaskData { TaskId = 11, TaskText = "H" };


            listTaskData.AddRange(new List<TaskData> { td1, td2, td3, td4 });
            AdoNetTasksRepository obj = new AdoNetTasksRepository();
            Console.WriteLine(obj.DeleteTasks(listTaskData).ToString());

        }
    }
}
