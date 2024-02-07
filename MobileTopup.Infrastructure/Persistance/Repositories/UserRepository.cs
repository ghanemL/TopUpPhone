using Microsoft.EntityFrameworkCore;
using MobileTopup.Application.Common.Interfaces.Persistance;
using MobileTopup.Domain.UserAggregate;

namespace MobileTopup.Infrastructure.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public User GetById(Guid userId)
        {
            return _context.Users
                .Include(u => u.Beneficiaries)
                .ThenInclude(u => u.Transactions)
                .SingleOrDefault(u => u.Id == userId);
        }
        

        public User GetByName(string name)
        {
            return _context.Users
                .Include(u => u.Beneficiaries)
                .ThenInclude(u => u.Transactions)
                .SingleOrDefault(u => u.Name == name);
        }

        public async Task SaveAsync(User user)
        {
            if (user.Id == Guid.Empty)
            {
                await _context.Users.AddAsync(user);
            }
            else
            {
                _context.Users.Update(user);
            }

            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
