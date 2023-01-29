using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SleekFlowTodoListCore.Domain.Contexts;
using SleekFlowTodoListCore.Domain.Database.Users;
using System.Web.Helpers;

namespace SleekFlowTodoListCore.Domain.Database
{
    public class DatabaseService
    {
        private readonly DatabaseContext _dbContext;
        private readonly CurrentContext _userContext;
        private readonly DatabaseOptions _options;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<DatabaseService> _logger;

        // Constructor
        public DatabaseService(DatabaseContext dbContext, CurrentContext userContext, UserManager<User> userManager, IOptions<DatabaseOptions> options, ILogger<DatabaseService> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _options = options.Value;
            _logger = logger;
            _userContext = userContext;
        }

        #region Create and update admin user on startup

        public void UserAdmin()
        {
            // Check option for seeding admin
            if (string.IsNullOrEmpty(_options.AdminEmail) || string.IsNullOrEmpty(_options.AdminPassword))
                throw new ArgumentException("Admin email/password cannot be null");

            // Check if configured admin email exist
            var admin = _userManager.FindByEmailAsync(_options.AdminEmail).Result;
            if (admin == null)
            {
                // Check if theres pre-existing admin user, create new if no and overwrite if yes
                var AdminCheck = _userManager.Users.Where(x => x.Admin == true).FirstOrDefault();
                if (AdminCheck == null)
                {
                    admin = new User
                    {
                        Email = _options.AdminEmail,
                        DisplayName = "admin",
                        EmailConfirmed = true,
                        Admin = true
                    };

                    var result = _userManager.CreateAsync(admin, _options.AdminPassword).Result;

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Admin user created successfully.");
                    }
                    else
                    {
                        _logger.LogError("Failed to create Admin user: {0}", result.Errors.ToString());
                    }
                }
                else
                {
                    AdminCheck.Email = _options.AdminEmail;
                    AdminCheck.PasswordHash = Crypto.HashPassword(_options.AdminPassword);
                    var result = _userManager.UpdateAsync(AdminCheck).Result;

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Admin user updated successfully.");
                    }
                    else
                    {
                        _logger.LogError("Failed to update Admin user: {0}", result.Errors.ToString());
                    }
                }
            }
        }

        #endregion
    }
}
