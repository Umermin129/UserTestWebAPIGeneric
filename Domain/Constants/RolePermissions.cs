using Domain.Enums;

namespace Domain.Constants
{
    public static class RolePermissions
    {
        private static readonly Dictionary<Role, List<Permission>> _rolePermissions = new()
        {
            {
                Role.Admin,
                new List<Permission>
                {
                    Permission.ViewUsers,
                    Permission.CreateUser,
                    Permission.EditUser,
                    Permission.DeleteUser,
                    Permission.ViewStudents,
                    Permission.CreateStudent,
                    Permission.EditStudent,
                    Permission.DeleteStudent,
                    Permission.ViewTeachers,
                    Permission.CreateTeacher,
                    Permission.EditTeacher,
                    Permission.DeleteTeacher,
                    Permission.ManageRoles,
                    Permission.ManagePermissions
                }
            },
            {
                Role.Teacher,
                new List<Permission>
                {
                    Permission.ViewUsers,
                    Permission.ViewTeachers,
                }
            },
            {
                Role.Student,
                new List<Permission>
                {
                    Permission.ViewUsers,
                    Permission.ViewStudents,
                }
            }
        };

        public static List<Permission> GetPermissionsForRole(Role role)
        {
            return _rolePermissions.TryGetValue(role, out var permissions) 
                ? permissions 
                : new List<Permission>();
        }

        public static List<string> GetPermissionNamesForRole(Role role)
        {
            return GetPermissionsForRole(role)
                .Select(p => p.ToString())
                .ToList();
        }
    }
}
