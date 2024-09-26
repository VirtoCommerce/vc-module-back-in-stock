using AutoMapper;
using VirtoCommerce.BackInStockModule.Core.Models;
using VirtoCommerce.BackInStockModule.ExperienceApi.Commands;

namespace VirtoCommerce.BackInStockModule.ExperienceApi.Mapping;

public class BackInStockProfile : Profile
{
    public BackInStockProfile()
    {
            CreateMap<ActivateBackInStockSubscriptionCommand, BackInStockSubscription>();
        }
}