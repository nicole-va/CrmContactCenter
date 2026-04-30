using CrmContactCenter.Server.Data;
using CrmContactCenter.Server.DTOs.Customers;
using CrmContactCenter.Server.Models;
using CrmContactCenter.Server.Models.Enums;
using CrmContactCenter.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrmContactCenter.Server.Services
{
    public class CustomerService: ICustomerService
    {
        private readonly CrmDbContext _context;

        public CustomerService(CrmDbContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerDto>> GetAllAsync(string? search)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(c =>
                    c.FirstName.ToLower().Contains(search) ||
                    c.LastName.ToLower().Contains(search) ||
                    (c.Cedula != null && c.Cedula.Contains(search)) ||
                    (c.Phone != null && c.Phone.Contains(search))
                );
            }

            return await query
                .Select(c => ToDto(c))
                .ToListAsync();
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            return customer == null ? null : ToDto(customer);
        }

        public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
        {
            var customer = new Customer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Cedula = dto.Cedula,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                City = dto.City,
                Notes = dto.Notes,
                Status = CustomerStatus.activo,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return ToDto(customer);
        }

        public async Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return null;

            customer.FirstName = dto.FirstName;
            customer.LastName = dto.LastName;
            customer.Cedula = dto.Cedula;
            customer.Phone = dto.Phone;
            customer.Email = dto.Email;
            customer.Address = dto.Address;
            customer.City = dto.City;
            customer.Notes = dto.Notes;
            customer.Status = Enum.Parse<CustomerStatus>(dto.Status);
            customer.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return ToDto(customer);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        private static CustomerDto ToDto(Customer c) => new CustomerDto
        {
            Id = c.Id,
            FirstName = c.FirstName,
            LastName = c.LastName,
            Cedula = c.Cedula,
            Phone = c.Phone,
            Email = c.Email,
            Address = c.Address,
            City = c.City,
            Status = c.Status.ToString(),
            Notes = c.Notes,
            CreatedAt = c.CreatedAt
        };
    }
}

