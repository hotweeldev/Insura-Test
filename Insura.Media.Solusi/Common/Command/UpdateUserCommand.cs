using Insura.Media.Solusi.Common.Enums;

namespace Insura.Media.Solusi.Common.Command
{
    public class UpdateUserCommand
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public UserStatus? Status { get; set; }
    }
}
