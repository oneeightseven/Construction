using Construction.Models.Dtos;
using Construction.Models.Models;

namespace Construction.Service.Mapping
{
    public class AccountMapping
    {
        public static List<AccountDto> Map(List<Account> models) => models.Select(x => new AccountDto(x)).ToList();

        public static AccountDto Map(Account model) => new (model);
    }
}
