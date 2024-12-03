namespace BasicSaasTenent.Models
{
    public interface ITenantGetter
    {
        public string Id { get; }
        public string Name { get; }
        public bool IsActive { get; } 
    }
}
