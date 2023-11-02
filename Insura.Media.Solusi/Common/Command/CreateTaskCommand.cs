using Insura.Media.Solusi.Common.Enums;
using Insura.Media.Solusi.Models;

namespace Insura.Media.Solusi.Common.Command
{
    public class CreateTaskCommand
    {
        public string TaskName { get; set; }
        public string? TaskDescription { get; set; }
        public string TaskModule { get; set; }
        public int UserId { get; set; }
    }
}
