using Chinook.Domain.ApiModels;
using Chinook.Domain.Entities;
using Chinook.Domain.Extensions;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor;

public partial class ChinookSupervisor
{
    public List<CustomerApiModel> GetAllCustomer()
    {
        List<Customer> customers = _customerRepository.GetAll();
        var customerApiModels = customers.ConvertAll();

        foreach (var customer in customerApiModels)
        {
            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            ;
            _cache.Set(string.Concat("Customer-", customer.Id), customer, (TimeSpan)cacheEntryOptions);
        }

        return customerApiModels;
    }

    public CustomerApiModel GetCustomerById(int id)
    {
        var customerApiModelCached = _cache.Get<CustomerApiModel>(string.Concat("Customer-", id));

        if (customerApiModelCached != null)
        {
            return customerApiModelCached;
        }
        else
        {
            var customer = _customerRepository.GetById(id);
            var customerApiModel = customer.Convert();
            customerApiModel.Invoices = (GetInvoiceByCustomerId(customerApiModel.Id)).ToList();
            customerApiModel.SupportRep =
                GetEmployeeById(customerApiModel.SupportRepId);
            if (customerApiModel.SupportRep != null)
                customerApiModel.SupportRepName =
                    $"{customerApiModel.SupportRep.LastName}, {customerApiModel.SupportRep.FirstName}";

            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            ;
            _cache.Set(string.Concat("Customer-", customerApiModel.Id), customerApiModel,
                (TimeSpan)cacheEntryOptions);

            return customerApiModel;
        }
    }

    public List<CustomerApiModel> GetCustomerBySupportRepId(int id)
    {
        var customers = _customerRepository.GetBySupportRepId(id);

        foreach (var customer in customers)
        {
            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            ;
            _cache.Set(string.Concat("Customer-", customer.Id), customer, (TimeSpan)cacheEntryOptions);
        }

        return customers.ConvertAll();
    }

    public CustomerApiModel AddCustomer(CustomerApiModel newCustomerApiModel)
    {
        _customerValidator.ValidateAndThrowAsync(newCustomerApiModel);

        var customer = newCustomerApiModel.Convert();

        customer = _customerRepository.Add(customer);
        newCustomerApiModel.Id = customer.Id;
        return newCustomerApiModel;
    }

    public bool UpdateCustomer(CustomerApiModel customerApiModel)
    {
        _customerValidator.ValidateAndThrowAsync(customerApiModel);

        var customer = _customerRepository.GetById(customerApiModel.Id);
        
        customer.FirstName = customerApiModel.FirstName;
        customer.LastName = customerApiModel.LastName;
        customer.Company = customerApiModel.Company ?? string.Empty;
        customer.Address = customerApiModel.Address ?? string.Empty;
        customer.City = customerApiModel.City ?? string.Empty;
        customer.State = customerApiModel.State ?? string.Empty;
        customer.Country = customerApiModel.Country ?? string.Empty;
        customer.PostalCode = customerApiModel.PostalCode ?? string.Empty;
        customer.Phone = customerApiModel.Phone ?? string.Empty;
        customer.Fax = customerApiModel.Fax ?? string.Empty;
        customer.Email = customerApiModel.Email ?? string.Empty;
        customer.SupportRepId = customerApiModel.SupportRepId;

        return _customerRepository.Update(customer);
    }

    public bool DeleteCustomer(int id)
        => _customerRepository.Delete(id);
}