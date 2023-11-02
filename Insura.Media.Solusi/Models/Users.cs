using Insura.Media.Solusi.Common.Enums;

namespace Insura.Media.Solusi.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NIK { get; set; }
        public string Address { get; set; }
        public UserStatus Status { get; set; }
    }
}
