
using AutoMapper;
using Pleromi.BLS.Models.Sample;

namespace Pleromi.BLS.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //**CreateMap<TEntity, VEntity>();
            CreateMap<SampleModel, SampleModel>().ReverseMap();
           
        }

    }
}
