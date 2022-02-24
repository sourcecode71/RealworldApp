using Application.DTOs;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMG.Data.Repository.Projects
{
    public interface ICompanyRepository 
    {
        Task<bool> SaveCleint(ClientDTO dTO);
        Task<bool> SaveCompany(CompanyDTO dTO);
        IQueryable<Client> GetAllClient();
        Task <List<CompanyDTO>> GetAllCompany();
    }
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DataContext _context;
        public CompanyRepository(DataContext dataContext)
        {
            _context = dataContext;
        }

        public IQueryable<Client> GetAllClient()
        {
            return _context.Clients.Where(p => p.IsActive);
        }

        public async Task<List<CompanyDTO>> GetAllCompany()
        {
            try
            {
                var company = await (from cm in _context.Company
                                     join cl in _context.Clients on cm.ClientId equals cl.Id
                                     where (cm.IsActive && cl.IsActive)
                                     select new CompanyDTO
                                     {
                                         Id = cm.Id,
                                         Address = cm.Address,
                                         Name = cm.Name,
                                         ClientName = cl.Name
                                     }).ToListAsync();


                return company;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> SaveCleint(ClientDTO dTO)
        {
            try
            {
               Client  client = new Client
               {
                   Name =dTO.Name,
                   Address =dTO.Address
               };
                _context.Clients.Add(client);
                int State = await _context.SaveChangesAsync();

                return State == 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> SaveCompany(CompanyDTO dTO)
        {
            try
            {
                Company company = new Company
                {
                    Name = dTO.Name,
                    Address = dTO.Address,
                    ClientId = dTO.ClientId
                };

                _context.Company.Add(company);
                int State = await _context.SaveChangesAsync();
                return State ==1 ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      
    }
}
