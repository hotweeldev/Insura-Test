namespace Insura.Media.Solusi.Common.Dto
{
    public class DataTableDto<T>
    {
        public int Page { get; set; }
        public int TotalData { get; set; }
        public int TotalPages { get; set; }
        public int Size { get; set; }
        public List<T>? Data { get; set; }
    }
}
