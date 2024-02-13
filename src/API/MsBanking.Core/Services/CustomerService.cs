using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MsBanking.Common.Entity;
using MsBanking.Core.Domain;

namespace MsBanking.Core.Services
{
    public class CustomerService : ICustomerService
    {

        //*Bununla tabloyu temsil ediyoruz.
        public readonly IMongoCollection<Customer> customerCollection;

        public CustomerService(IOptions<DatabaseOption> options)
        {
            var dbOption = options.Value;
            var client = new MongoClient(dbOption.ConnectionString);
            var database = client.GetDatabase(dbOption.DatabaseName);
            customerCollection = database.GetCollection<Customer>(dbOption.CustomerCollectionName);
        }

        //*Customer ekleme
        public async Task<Customer> GetCustomer(int id)
        {
            var customerEntity = await customerCollection.FindAsync(c => c.Id == id);
            return customerEntity.FirstOrDefault();

        }

        //*Tüm customerları getirme
        public async Task<List<Customer>> GetCustomers()
        {
            var customerEntities = await customerCollection.FindAsync(c => true);
            return customerEntities.ToList();
        }

        public async Task<Customer> CreateCustomer(Customer customer)
        {
            await customerCollection.InsertOneAsync(customer);
            return customer;
        }


    }
}
