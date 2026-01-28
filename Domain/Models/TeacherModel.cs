using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public record TeacherModel
    (
        string Name,
        string Email,
        string Subject
    );
}
