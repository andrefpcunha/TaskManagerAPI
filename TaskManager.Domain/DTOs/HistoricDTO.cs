namespace TaskManager.Domain.DTOs
{
    public class HistoricDTO<T>
    {
        public Guid ModifiedBy { get; set; }
        public DateTime Date { get; set; }
        public T EntityOld { get; set; }
        public T EntityNew { get; set; }
    }
}
