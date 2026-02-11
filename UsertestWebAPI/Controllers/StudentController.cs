using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace UsertestWebAPI.Controllers
{

    public class StudentController : BaseController<Student, StudentResponse, StudentModel>
    {
        public StudentController(IService<Student, StudentResponse, StudentModel> service)
    : base(service)
        {
        }
    }
}
