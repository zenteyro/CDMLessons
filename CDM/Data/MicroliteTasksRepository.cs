using MicroLite.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroLite;
using MicroLite.Builder;

namespace CDM
{
    class MicroliteTasksRepository : ITasksRepository
    {
        static ISessionFactory sessionfactory = Configure.Fluently().ForMsSql2012Connection("cdmtask").CreateSessionFactory();

        static MicroliteTasksRepository()
        {
            Configure.Extensions().WithAttributeBasedMapping();
            sessionfactory = Configure.Fluently().ForMsSql2012Connection("cdmtask").CreateSessionFactory();
        }
        public bool DeleteTaskById(int id)
        {
            using (var session = sessionfactory.OpenSession())
            {
                return session.Advanced.Delete(typeof(TaskData), id);
            }
        }
        public bool DeleteTasks(List<TaskData> tasks)
        {
            using (var session = sessionfactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    foreach (TaskData taskData in tasks)
                    {
                        if (!session.Advanced.Delete(typeof(TaskData), taskData.TaskId))
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                    transaction.Commit();
                    return true;
                }
            }
        }
        public List<TaskData> GetAllTasks()
        {
            using (var session = sessionfactory.OpenSession())
            {
                return (List<TaskData>) session.Fetch<TaskData>(new SqlQuery("SELECT * FROM Tasks"));
            }
        }
        public TaskData GetTaskById(int id)
        {
            using (var session = sessionfactory.OpenSession())
            {
                return session.Single<TaskData>(id);
            }
        }

        public TaskData GetTasksByUser(TaskUser user)
        {
            throw new NotImplementedException();
        }

        public bool UpsertTask(TaskData task)
        {
            using (var session = sessionfactory.OpenSession())
            {
                TaskData taskData = session.Single<TaskData>(task.TaskId);
                if (taskData != null)
                    session.Update(task);
                else
                {
                    task.TaskId = 0;
                    session.Insert(task);
                }
                return true;
            }
        }
    }
}
