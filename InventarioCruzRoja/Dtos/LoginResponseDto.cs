﻿namespace InventarioCruzRoja.Dtos
{
    public class LoginResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; }
        public string AccessToken { get; set; }
    }
}
