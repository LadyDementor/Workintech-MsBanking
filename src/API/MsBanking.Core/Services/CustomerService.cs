using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MsBanking.Common.Dto;
using MsBanking.Common.Entity;
using MsBanking.Core.Domain;

namespace MsBanking.Core.Services
{
    public class CustomerService : ICustomerService
    {

        //*Bununla tabloyu temsil ediyoruz.
        public readonly IMongoCollection<Customer> customerCollection;
        public readonly IMapper mapper;//*AutoMapper'ı kullanabilmek için

        public CustomerService(IOptions<DatabaseOption> options,IMapper _mapper)//*DatabaseOption'ı kullanabilmek için
        {
            mapper = _mapper;//*AutoMapper'ı kullanabilmek için
            var dbOption = options.Value;
            var client = new MongoClient(dbOption.ConnectionString);
            var database = client.GetDatabase(dbOption.DatabaseName);
            customerCollection = database.GetCollection<Customer>(dbOption.CustomerCollectionName);
        }

        //*Customer ekleme
        public async Task<CustomerResponseDto> GetCustomer(string id)
        {
            var customerEntity = await customerCollection.FindAsync(c => c.Id == id);
             var entity =customerEntity.FirstOrDefault();
            var mapped = mapper.Map<CustomerResponseDto>(entity);
            return mapped;
        }

        //*Tüm customerları getirme
        public async Task<List<CustomerResponseDto>> GetCustomers()
        {
            var customerEntities = await customerCollection.FindAsync(c => true);
           var customerList =customerEntities.ToList();
            var mapped = mapper.Map<List<CustomerResponseDto>>(customerList);
            return mapped;
        }

        public async Task<CustomerResponseDto> CreateCustomer(CustomerDto customer)
        {
            var customerEntity = mapper.Map<Customer>(customer);//*Dto'dan entity'e dönüşüm
            customerEntity.CreatedDate = DateTime.Now;
            customerEntity.UpdatedDate = DateTime.Now;
            customerEntity.IsActive = true;
            await customerCollection.InsertOneAsync(customerEntity);

            var CustomerResponse = mapper.Map<CustomerResponseDto>(customerEntity);//*Entity'den Dto'ya dönüşüm
            return CustomerResponse;
        }

        public async Task<CustomerResponseDto> UpdateCustomer(string id,CustomerDto customer)
        {
            var customerEntity = mapper.Map<Customer>(customer);
            customerEntity.UpdatedDate = DateTime.Now;
            await customerCollection.ReplaceOneAsync(c => c.Id == id, customerEntity);

            var customerResponseDto = mapper.Map<CustomerResponseDto>(customerEntity);
            return customerResponseDto;
        }

        public async Task<bool>  DeleteCustomer(string id)
        {
            var customerEntity = await customerCollection.FindAsync(c => c.Id == id);
            var entity = customerEntity.FirstOrDefault();
            entity.IsActive = false;
            var result=await customerCollection.ReplaceOneAsync(c => c.Id == id, entity);
            return result.ModifiedCount > 0;

          
        }

    }
}
