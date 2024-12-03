namespace BasicSaasTenent.Models
{
    public interface ITenantSetter
    {
        public string Id { set; }
        public string Name { set; }
        public bool IsActive {  set; }  
    }
}
