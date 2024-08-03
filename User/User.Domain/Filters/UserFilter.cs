using HealthMed.Domain.Filters;

namespace User.Domain.Filters;

public class UserFilter : _BaseFilter
{
    public string FirstName { get; set; }
    public string LastName { get; set; }    
    public bool? Active { get; set; }
}
