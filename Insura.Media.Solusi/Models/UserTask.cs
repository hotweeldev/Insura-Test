using Insura.Media.Solusi.Common.Enums;

namespace Insura.Media.Solusi.Models
{
    public class UserTask
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string? TaskDescription { get; set; }
        public DateTime TaskStart { get; set; }
        public DateTime? TaskEnd { get; set; }
        public TaskProgress TaskProgress { get; set; }
        public string TaskModule { get; set; }
        public Users User { get; set; }
    }
}
