using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Commons.Bases;

namespace TaskManager.Application.UseCases.Task.v1.NewTask
{
    public class NewTaskResult
    {
        public Guid TaskId { get; set; }
    }
}
