
using System.Diagnostics.CodeAnalysis;

namespace TaskManager.Application.UseCases.Project.v1
{
    [ExcludeFromCodeCoverage]
    public class DeleteProjectResult
    {
        public Guid ModifiedBy { get; set; }
        public DateTime Date { get; set; }
        public Domain.Entities.Projects EntityOld { get; set; }
        public Domain.Entities.Projects EntityNew { get; set; }
    }
}
