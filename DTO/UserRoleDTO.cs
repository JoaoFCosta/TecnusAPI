using System.ComponentModel.DataAnnotations;

namespace TecnusAPI.DTO
{
    public class UserRoleModel
    {
        [Required]
        public string UserNome { get; set; } // Nome do usuario

        [Required]
        public string RoleName { get; set; } // O nome da role (ex: "Admin")
    }
}