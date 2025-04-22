namespace TaskManager.Application.UseCases.Project.v1.GetProjectsByUser
{
    public class ProjectDTO
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }

        public string OwnerUser { get; set; }
    }
}
