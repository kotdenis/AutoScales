namespace AutoScales.Data.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
    }
}
