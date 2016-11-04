using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDM
{
    public interface ITasksRepository
    {
        List<TaskData> GetAllTasks();

        TaskData GetTaskById(int id);

        TaskData GetTasksByUser(TaskUser user);

        bool DeleteTasks(List<TaskData> tasks);

        bool UpsertTask(TaskData task);

        bool DeleteTaskById(int id);
    }
}
