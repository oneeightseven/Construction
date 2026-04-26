using Construction.Models.Models;

namespace Construction.Models.Dtos
{
    public class AccountDto
    {
        public AccountDto(){}
        public AccountDto(Account model)
        {
            Id = model.Id;
            TypeOfAppointment = model.TypeOfAppointment;
            Date = model.Date;
            Payer = model.Payer;
            Details = model.Details;
            Sum = model.Sum;
        }
        public int Id { get; set; }

        public TypeOfAppointment? TypeOfAppointment { get; set; }

        public DateTime Date { get; set; }

        public Client? Payer { get; set; }

        public string Details { get; set; }

        public decimal Sum { get; set; }
    }
}
