using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsBanking.Common.Dto
{
    public class CustomerDto
    {
        public string FullName { get; set; }
        public long CitizenNumber { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class CustomerDtoProfile:Profile
    {
        public CustomerDtoProfile()
        {
            //*Dto'dan entity'e dönüşüm.Tek tek yazmıyoruz çünkü aynı isimde propertyler var.
            CreateMap<CustomerDto, MsBanking.Common.Entity.Customer>().ReverseMap();
            CreateMap<MsBanking.Common.Entity.Customer, CustomerResponseDto>().ReverseMap();
        }
    }

    public class CustomerResponseDto: CustomerDto
    {
        public int Id { get; set; }

    }
}
