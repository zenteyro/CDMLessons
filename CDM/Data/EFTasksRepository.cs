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
        


        public bool DeleteTaskById(int id)
        {
            TaskData taskData = this.TaskDatas.FirstOrDefault(p => p.TaskId == id);
            if (taskData != null)
            {
                TaskData result = this.TaskDatas.Remove(taskData);
                this.SaveChanges();
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
                    //    TaskData task = this.TaskDatas.Remove(task);
                    //    if (this.TaskDatas.Remove(task) == null) throw new Exception();
                    }
                    
                    transaction.Commit();
                    this.SaveChanges();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }            
        }

        public List<TaskData> GetAllTasks()
        {
            throw new NotImplementedException();
        }

        public TaskData GetTaskById(int id)
        {
            throw new NotImplementedException();
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
