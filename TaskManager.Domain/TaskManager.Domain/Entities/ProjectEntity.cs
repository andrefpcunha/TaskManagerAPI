using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain.Entities
{
    public class ProjectEntity
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public IEnumerable<TaskEntity> Tasks { get; set; }
    }
}
