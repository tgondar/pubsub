using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using src.Domain.Entities;
using src.Infrastructure.Identity;

namespace src.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole("Administrator");

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

        if (!_context.Product.Any())
        {
            _context.Product.Add(new Products { Reference = "REF1", Designation = "Designation 1", Price = 10, CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow });
            _context.Product.Add(new Products { Reference = "REF2", Designation = "Designation 2", Price = 20, CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow });
            _context.Product.Add(new Products { Reference = "REF3", Designation = "Designation 3", Price = 30, CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow });
            _context.Product.Add(new Products { Reference = "REF4", Designation = "Designation 4", Price = 40, CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow });
            _context.Product.Add(new Products { Reference = "REF5", Designation = "Designation 5", Price = 50, CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow });
            _context.Product.Add(new Products { Reference = "REF6", Designation = "Designation 6", Price = 60, CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow });
            _context.Product.Add(new Products { Reference = "REF7", Designation = "Designation 7", Price = 70, CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow });
            _context.Product.Add(new Products { Reference = "REF8", Designation = "Designation 8", Price = 80, CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow });
            _context.Product.Add(new Products { Reference = "REF9", Designation = "Designation 9", Price = 90, CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow });
            _context.Product.Add(new Products { Reference = "REF10", Designation = "Designation 10", Price = 100, CreateDate = DateTime.UtcNow, UpdateDate = DateTime.UtcNow });

            _context.SaveChanges();
        }
    }
}
