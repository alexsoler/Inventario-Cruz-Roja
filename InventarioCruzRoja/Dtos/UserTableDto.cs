namespace InventarioCruzRoja.Dtos
{
    public class UserTableDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; }
    }
}
