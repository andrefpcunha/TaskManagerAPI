namespace TaskManager.Application.UseCases.Project.v1.GetProjectsByUser
{
    public class GetProjectsByUserResult
    {
        public String UserId { get; set; }
        public IEnumerable<ProjectDTO> Projects { get; set; }
    }
}
