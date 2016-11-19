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

namespace CDM
{
    class Program
    {
        static void Main(string[] args)
        {
            EFTasksRepository ef = new EFTasksRepository();
            

            ef.DeleteTaskById(55);
        }
    }
}