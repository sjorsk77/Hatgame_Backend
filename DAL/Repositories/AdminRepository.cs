using DLL.Entities;
using DLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AdminRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Admin> GetAdminByEmailAsync(string email)
    {
        var admin = await _dbContext.Admins.FirstOrDefaultAsync(admin => admin.Email == email);
        
        if (admin == null)
            throw new ArgumentException("Admin not found");
        
        return admin;
    }

    public async Task<Admin> CreateAdminAsync(Admin admin)
    {
        var createdAdmin = await _dbContext.Admins.AddAsync(admin);
        
        await _dbContext.SaveChangesAsync();
        
        return await _dbContext.Admins.FirstOrDefaultAsync(admin => admin.Id == createdAdmin.Entity.Id);
    }
}