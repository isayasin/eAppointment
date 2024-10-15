using eAppointmentServer.Domain.Entities;

namespace eAppointmentServer.Application;
public static class Constants
{
    public static List<AppRole> GetRoles()
    {
        List<string> roles = new()
        {
            "Admin",
            "Doctor",
            "Personel",
            "Patient"
        };

        return roles.Select(s => new AppRole() { Name = s }).ToList();
    }


    /*public static List<AppRole> Roles = new()
    {
        new()
        {
            Name = "Admin"
        },
        new()
        {
            Name = "Doctor"
        },
        new()
        {W
            Name = "Personel"
        }
    };*/
}




