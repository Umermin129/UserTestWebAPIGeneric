using Application.DTOs;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace UsertestWebAPI.Controllers
{
    public class TeacherController : BaseController<Teacher,TeacherResponse,TeacherModel>
    {
        public TeacherController(IService<Teacher, TeacherResponse, TeacherModel> service)
: base(service)
        {
        }
    }
}
