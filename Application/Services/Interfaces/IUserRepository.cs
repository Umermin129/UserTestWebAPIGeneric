using Application.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface  IUserRepository
    {
        CreateUserResponse CreateUser(CreateUserRequest user);
    }
}
