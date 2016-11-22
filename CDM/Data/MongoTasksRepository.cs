using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MongoDB.Driver;
using MongoDB.Bson;

namespace CDM
{
    class MongoTasksRepository : ITasksRepository
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["CDMTaskMongoDB"].ConnectionString;
        static MongoClient connection = new MongoClient(connectionString);
        static IMongoDatabase database = connection.GetDatabase("Task");
        static IMongoCollection<TaskData> collection = database.GetCollection<TaskData>("tasks");

        public bool DeleteTaskById(int id)
        {
            var result = collection.DeleteOne(t => t.TaskId == id);
            if (result.DeletedCount == 0)
                return false;
            else
                return true;

        }
        public bool DeleteTasks(List<TaskData> tasks)
        {
            try
            {
                foreach (var task in tasks)
                    collection.Find(t=>t.TaskId == task.TaskId).Single();
            }
            catch
            {
                return false;
            }
            foreach (var task in tasks)
                collection.DeleteOne(t => t.TaskId == task.TaskId);
            return true;
            
        }
        public List<TaskData> GetAllTasks()
        {
            return collection.Find(new BsonDocument()).ToList();
        }
        public TaskData GetTaskById(int id)
        {
            return collection.Find<TaskData>(t => t.TaskId == id).Single();
        }

        public TaskData GetTasksByUser(TaskUser user)
        {
            throw new NotImplementedException();
        }
        public bool UpsertTask(TaskData task)
        {
            collection.ReplaceOne(t => t.TaskId == task.TaskId, task, new UpdateOptions { IsUpsert = true });
            return true;
        }
    }
}
