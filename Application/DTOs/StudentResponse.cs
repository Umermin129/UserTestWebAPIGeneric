using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public record StudentResponse
    (
        Guid? Id,
       string? Name = null,
       string? Email = null,
       string?  RollNo = null
        );
}
