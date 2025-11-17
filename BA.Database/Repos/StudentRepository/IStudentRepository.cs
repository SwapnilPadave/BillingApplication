using BA.Database.Infra;
using BA.Entities.Student;

namespace BA.Database.Repos.StudentRepository
{
    public interface IStudentRepository : IRepository<Student>
    {
    }

    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private readonly BAContext _context;
        public StudentRepository(BAContext context) : base(context)
        {
            _context = context;
        }
    }
}
