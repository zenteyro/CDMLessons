using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace CDM
{
    class EFTasksRepository : DbContext, ITasksRepository
    {
        public EFTasksRepository() : base("CDMTask") { }
        public DbSet<TaskData> TaskDatas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskData>().ToTable("Tasks");
            modelBuilder.Entity<TaskData>().HasKey(p => p.TaskId);
            modelBuilder.Entity<TaskData>().Property(p => p.TaskText).HasColumnName("TaskText");
            base.OnModelCreating(modelBuilder);
        }
        protected override void Dispose(bool disposing)
        {
            Dispose();
            base.Dispose(disposing);
        }


        public bool DeleteTaskById(int id)
        {
            TaskData taskData = TaskDatas.Find(id);
            if (taskData != null)
            {
                TaskData result = TaskDatas.Remove(taskData);
                SaveChanges();
                return true;
            }
            return false;
        }
        public bool DeleteTasks(List<TaskData> tasks)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    foreach (TaskData task in tasks)
                    {
                        TaskData item = TaskDatas.Find(task.TaskId);
                        TaskDatas.Remove(item);
                    }
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }

                transaction.Commit();
                SaveChanges();
                return true;
            }            
        }
        public List<TaskData> GetAllTasks()
        {
            return TaskDatas.ToList();
        }
        public TaskData GetTaskById(int id)
        {
            return TaskDatas.Find(id);
        }

        public TaskData GetTasksByUser(TaskUser user)
        {
            throw new NotImplementedException();
        }

        public bool UpsertTask(TaskData task)
        {
            TaskData taskData = TaskDatas.Find(task.TaskId);
            if (taskData == null)
                TaskDatas.Add(task);
            else
            {
                Entry(taskData).State = EntityState.Modified;
                taskData.TaskText = task.TaskText;
            }
            SaveChanges();
            return true;
        }
    }
}
