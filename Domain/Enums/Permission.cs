namespace Domain.Enums
{
    public enum Permission
    {
        // User Management
        ViewUsers = 1,
        CreateUser = 2,
        EditUser = 3,
        DeleteUser = 4,
        
        // Student Management
        ViewStudents = 5,
        CreateStudent = 6,
        EditStudent = 7,
        DeleteStudent = 8,
        
        // Teacher Management
        ViewTeachers = 9,
        CreateTeacher = 10,
        EditTeacher = 11,
        DeleteTeacher = 12,
        
        // Admin Only
        ManageRoles = 13,
        ManagePermissions = 14
    }
}
