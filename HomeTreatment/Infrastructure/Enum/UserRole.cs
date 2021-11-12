namespace HomeTreatment.Web.Infrastructure
{   
    public enum UserRole
    {       
        Admin = 1,
        User = 2,
        Patient = 3,
        Doctor = 4       
    }
}

// nikoga da ne slagam 0 za default value, poneve ako deserilize nqma ekvivalentna stoinost v JSON-a
