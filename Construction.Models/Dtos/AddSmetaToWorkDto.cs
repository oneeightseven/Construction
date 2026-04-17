namespace Construction.Models.Dtos
{
    public class AddSmetaToWorkDto
    {
        public int? Id { get; set; }
        public int Count { get; set; }
        public int MaterialId { get; set; }
        public decimal Price { get; set; }
        public int WorkId { get; set; }
    }
}
