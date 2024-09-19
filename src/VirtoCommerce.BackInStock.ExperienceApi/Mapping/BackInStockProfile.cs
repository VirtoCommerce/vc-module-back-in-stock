using AutoMapper;
using VirtoCommerce.BackInStock.Core.Models;
using VirtoCommerce.BackInStock.ExperienceApi.Commands;

namespace VirtoCommerce.BackInStock.ExperienceApi.Mapping
{
    public class BackInStockProfile : Profile
    {
        public BackInStockProfile()
        {
            CreateMap<ActivateBackInStockSubscriptionCommand, BackInStockSubscription>();
        }
    }
}
