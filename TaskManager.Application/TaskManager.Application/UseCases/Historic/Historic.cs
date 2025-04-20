using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Application.UseCases.Historic
{
    public class UpdateHistory
    {
        public DateTime DateModified { get; set; }
        public string ModifiedBy { get; set; }
        public string Changes { get; set; }
    }
}
