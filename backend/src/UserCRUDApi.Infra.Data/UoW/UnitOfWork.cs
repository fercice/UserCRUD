using UserCRUDApi.Domain.Interfaces;
using UserCRUDApi.Infra.Data.Context;

namespace UserCRUDApi.Infra.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        public UserCRUDContext _context;

        public UnitOfWork(UserCRUDContext context)
        {
            _context = context;
        }

        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
