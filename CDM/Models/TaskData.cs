using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroLite.Mapping;
using MicroLite.Mapping.Attributes;

namespace CDM
{
    [Table("Tasks")]
    public class TaskData
    {
        [Column("TaskId")]
        [Identifier(IdentifierStrategy.DbGenerated)]
        public int TaskId { get; set; }
        [Column("TaskText")]
        public string TaskText { get; set; }
    }
}
