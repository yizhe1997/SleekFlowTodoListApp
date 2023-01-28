using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using SleekFlowTodoListCore.Domain.Database.EntityTypes;
using SleekFlowTodoListCore.Domain.Database.Users;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SleekFlowTodoListCore.Domain.Database.Todos;
using System.Reflection;
using System.Data;

namespace SleekFlowTodoListCore.Domain.Contexts
{
    public class DatabaseContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        private IDbContextTransaction? _currentTransaction;
        private readonly HttpContext? _httpContext;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContext = httpContextAccessor.HttpContext ?? null;
        }

        #region Data Sets

        //#region Users

        //public DbSet<UserPasswordReset> UserPasswordResets { get; set; }

        //#endregion

        #region Todo

        public DbSet<Todo> Todos { get; set; }
        public DbSet<TodoStatus> TodoStatuses { get; set; }

        #endregion

        #endregion

        #region Context Operation Overrides

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define keys for models
            DefineDomainModels(builder);

            // Allows for default behaviours for AuditableEntities during creation
            var configureAuditableMethod = GetType().GetTypeInfo().DeclaredMethods.Single(m => m.Name == nameof(OnCreateAuditableEntity));

            var args = new object[] { builder };

            var auditableEntityTypes = builder.Model.GetEntityTypes().Where(t => typeof(AuditableEntity).IsAssignableFrom(t.ClrType));
            foreach (var entityType in auditableEntityTypes)
            {
                configureAuditableMethod.MakeGenericMethod(entityType.ClrType).Invoke(this, args);
            }
        }

        private void DefineDomainModels(ModelBuilder builder)
        {
            //#region Users

            //builder.Entity<UserPasswordReset>()
            //    .HasOne(bc => bc.User)
            //    .WithMany(b => b.UserPasswordResets)
            //    .HasForeignKey(bc => bc.User.Id);

            //#endregion

            #region Todos

            builder.Entity<Todo>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.Todos)
                .HasForeignKey(bc => bc.UserId);

            #endregion

            #region TodoStatus

            builder.Entity<TodoStatus>()
                .HasIndex(bc => new { bc.Name })
                .IsUnique();

            #endregion
        }

        public override int SaveChanges()
        {
            OnCreateUpdateAuditEntries();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnCreateUpdateAuditEntries();
            return base.SaveChangesAsync(cancellationToken);
        }

        #endregion

        #region Auditable Entity Default Behaviours

        // Behaviour for when creating and updating
        private void OnCreateUpdateAuditEntries()
        {
            // Obtain entities from context in current intance
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableEntity && (
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified ||
                    e.State == EntityState.Deleted));

            // Obtain user ID
            IEnumerable<Claim>? claims = _httpContext?.User?.Claims ?? null;
            var nameClaim = claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            Guid currentUserId = string.IsNullOrEmpty(nameClaim) ? Guid.Empty : Guid.Parse(nameClaim);
            User? currentUser = Users?.FirstOrDefault(u => u.Id == currentUserId) ?? null;
            var identityName = currentUser?.UserName ?? "Anonymous";

            // Track change and create timestamps
            foreach (var entityEntry in entries)
            {
                // General
                var auditableEntity = (AuditableEntity)entityEntry.Entity;

                auditableEntity.UpdatedDate = DateTime.UtcNow;
                auditableEntity.UpdatedBy = auditableEntity.UpdatedBy != null ? auditableEntity.UpdatedBy : identityName;

                // Add
                if (entityEntry.State == EntityState.Added)
                {
                    auditableEntity.CreatedDate = DateTime.UtcNow;
                    auditableEntity.CreatedBy = identityName;
                }
            }
        }

        // Behaviour for when creating models
        private void OnCreateAuditableEntity<TEntity>(ModelBuilder modelBuilder) where TEntity : AuditableEntity
        {
            // Default values for IEntity
            modelBuilder.Entity<TEntity>().Property(e => e.Id).HasDefaultValueSql("newid()");

            // Default values for IAuditableEntity
            modelBuilder.Entity<TEntity>().Property(e => e.CreatedDate).HasDefaultValueSql("getutcdate()");
            modelBuilder.Entity<TEntity>().Property(e => e.CreatedBy).HasDefaultValue("unknown");
            modelBuilder.Entity<TEntity>().Property(e => e.UpdatedDate).HasDefaultValueSql("getutcdate()");
            modelBuilder.Entity<TEntity>().Property(e => e.UpdatedBy).HasDefaultValue("unknown");
        }

        #endregion

        #region Data Entry Transaction Handling/ Pipeline Behaviour

        public void BeginTransaction()
        {
            if (_currentTransaction != null)
            {
                return;
            }
            else
            {
                _currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
            }
        }

        public void CommitTransaction()
        {
            try
            {
                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        #endregion

    }
}
