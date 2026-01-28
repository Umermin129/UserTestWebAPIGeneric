using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class StudentService(IUnitOfWork unitOfWork, IRepository<Student> studentRepository) : IService<Student,StudentResponse,StudentModel>
    {
        private readonly IRepository<Student> _studentRepository = studentRepository;

        public async Task<StudentResponse> CreateAsync(StudentModel student)
        {
            if(student== null)
                throw new ArgumentNullException(nameof(student));
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = student.Name,
                Email = student.Email,
            };
            var createStudent = new Student
            {
                Id = Guid.NewGuid(),
                RollNo = student.RollNo,
                User = user,
                UserId = user.Id,
            };
            user.Student = createStudent;
            var response = await _studentRepository.Create(createStudent);
            await unitOfWork.SaveChanges();
            return new StudentResponse(response.Id, response.User.Name, response.User.Email,response.RollNo);
        }
        public async Task<StudentResponse> GetAsync(Guid id)
        {
            var student = await _studentRepository.GetAsync(id);
            if(student == null)
                throw new ApplicationException("Student is not available");
            return new StudentResponse(student.Id, student.User.Name, student.User.Email, student.RollNo);

        }
        public async Task<List<StudentResponse>> GetAllAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            if (students == null)
                throw new ApplicationException("Students are Emty");
            return   students.Select(s => new StudentResponse(s.Id,s.User.Name,s.User.Email,s.RollNo)).ToList();
        }

    }
}
