using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Construction.Models.Models
{
    [Table("Works")]
    public class Work
    {
        [Key]
        public int Id { get; set; }
        public DateOnly? DateBid { get; set; } = null;
        public DateOnly? Term { get; set; } = null;
        public DateOnly? CompletionDate { get; set; } = null;

        [Required]
        public int CityId { get; set; }

        [ForeignKey("CityId")]
        public City? City { get; set; }

        [Required]
        public int ShoppingMallId { get; set; }

        [ForeignKey("ShoppingMallId")]
        public ShoppingMall? ShoppingMall { get; set; }

        [Required]
        public int BrandId { get; set; }

        [ForeignKey("BrandId")]
        public Brand? Brand { get; set; }

        [Required]
        public int StatusId { get; set; }

        [ForeignKey("StatusId")]
        public Status? Status { get; set; }

        [Required]
        public int ConstructionObjectId { get; set; }

        [ForeignKey("ConstructionObjectId")]
        public ConstructionObject? ConstructionObject { get; set; }

        [Required]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client? Client { get; set; }

        public DateOnly? DateOfCreation { get; set; } = null;
        public decimal? Summ { get; set; }
    }
}
