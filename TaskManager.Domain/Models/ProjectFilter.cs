namespace TaskManager.Domain.Models
{
    public class ProjectFilter
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string OrderBy { get; set; } = "Name";
        public string SortBy { get; set; } = "asc";
    }
}
