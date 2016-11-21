using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using MicroLite.Configuration;
using MicroLite.Builder;
using MicroLite;
using MongoDB.Driver;
using MongoDB.Bson;

namespace CDM
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoTasksRepository mt = new MongoTasksRepository();
            TaskData task1 = new TaskData { TaskId = 4, TaskText = "a" };
            TaskData task2 = new TaskData { TaskId = 4, TaskText = "b" };
            TaskData task3 = new TaskData { TaskId = 4, TaskText = "c" };
            TaskData task4 = new TaskData { TaskId = 4, TaskText = "d" };


            Console.WriteLine(mt.DeleteTasks(new List<TaskData> { task1, task2, task3, task4}).ToString());
        }
    }
}