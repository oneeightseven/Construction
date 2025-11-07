using Construction.Models.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Construction.Models.Dtos
{
    public class WorkDto
    {
        public WorkDto()
        {
            
        }
        public WorkDto(Work work)
        {
            Brand = work.Brand;
            City = work.City;
            Client = work.Client;
            CompletionDate = work.CompletionDate;
            ConstructionObject = work.ConstructionObject;
            DateBid = work.DateBid;
            DateOfCreation = work.DateOfCreation;
            Id = work.Id;
            ShoppingMall = work.ShoppingMall;
            Status = work.Status;
            Summ = work.Summ;
            Term = work.Term;
        }
        public int Id { get; set; }
        public DateOnly? DateBid { get; set; } = null;
        public DateOnly? Term { get; set; } = null;
        public DateOnly? CompletionDate { get; set; } = null;
        public City? City { get; set; }
        public ShoppingMall? ShoppingMall { get; set; }
        public Brand? Brand { get; set; }
        public Status? Status { get; set; }
        public ConstructionObject? ConstructionObject { get; set; }
        public Client? Client { get; set; }
        public DateOnly? DateOfCreation { get; set; } = null;
        public decimal? Summ { get; set; }
    }
}
