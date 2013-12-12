using AutoMapper;

namespace Tuto.Web.Mappers
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
                {
                    x.AddProfile<DomainToViewModelMappingProfile>();
                    x.AddProfile<ViewModelToDomainMappingProfile>();
                });
        }
    }
}