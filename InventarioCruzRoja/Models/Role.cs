namespace InventarioCruzRoja.Models
{
    public class Role : EntidadBase<int>
    {
        public string Name { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}
