using AutoMapper;

namespace MsBanking.Common.Dto
{
    public class BranchDto
    {

        public int Code { get; set; }
        public string Name { get; set; }
        public int CityCode { get; set; }
      
        public int CountryId { get; set; }
      
    }

    public class BranchResponseDto : BranchDto
    {

        public int Id { get; set; }//KUllanıcıya ıd göstermek istemeyebiliriz data alırken diğerlerini kullancağız ama veritabanında ıd tutmak isteyebiliriz

        public string CityName { get; set; }//Bu alan veritabanında tutmadığımız yer ve karşılığını enum olarak tutuyoruz
        public string CountryName { get; set; }//Bu alan veritabanında tutmadığımız yer ve karşılığını enum olarak tutuyoruz
    }


    public class BranchProfile:Profile
    {
        public BranchProfile()
        {
            CreateMap< MsBanking.Common.Entity.Branch,BranchDto > ().ReverseMap();
            CreateMap<MsBanking.Common.Entity.Branch, BranchResponseDto>()
                .ForMember(x => x.CityName, opt => opt.MapFrom(src => Enum.GetName(typeof(CityEnum), src.CityId)))//ForMember ile enumdan alacağımız city ve county işlemlerini yapabiliriz.
            .ForMember(x => x.CountryName, opt => opt.MapFrom(src => Enum.GetName(typeof(CountryEnum), src.CountryId))).ReverseMap();
        }
    }
}
