using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Interfaces;

namespace Application.Services
{
    public class TeacherService(
        IUnitOfWork unitOfWork,
        IRepository<Teacher> teacherRepository
    ) : IService<Teacher, TeacherResponse, TeacherModel>
    {
        private readonly IRepository<Teacher> _teacherRepository = teacherRepository;

        public async Task<TeacherResponse> CreateAsync(TeacherModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Email = model.Email
            };

            var teacher = new Teacher
            {
                Id = Guid.NewGuid(),
                Subject = model.Subject,
                User = user,
                UserId = user.Id
            };

            user.Teacher = teacher;

            var response = await _teacherRepository.Create(teacher);
            await unitOfWork.SaveChanges();

            return new TeacherResponse(
                response.Id,
                response.User.Name,
                response.User.Email,
                response.Subject
            );
        }

        public async Task<TeacherResponse> GetAsync(Guid id)
        {
            var teacher = await _teacherRepository.GetAsync(id);

            if (teacher == null)
                throw new ApplicationException("Teacher not found");

            return new TeacherResponse(
                teacher.Id,
                teacher.User.Name,
                teacher.User.Email,
                teacher.Subject
            );
        }

        public async Task<List<TeacherResponse>> GetAllAsync()
        {
            var teachers = await _teacherRepository.GetAllAsync();

            return teachers.Select(t => new TeacherResponse(
                t.Id,
                t.User.Name,
                t.User.Email,
                t.Subject
            )).ToList();
        }
    }
}