var builder = WebApplication.CreateBuilder(args);

var employees = new List<Employee>();

employees.Add(new Employee
{
    Id = 1,
    FirstName = "John",
    LastName = "Doe",
    SocialSecurityNumber = "123-45-6789"
});
employees.Add(new Employee
{
    Id = 2,
    FirstName = "Jane",
    LastName = "Doe",
    SocialSecurityNumber = "987-65-4321"
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

var employeeRoute = app.MapGroup("/employees");

employeeRoute.MapGet(string.Empty, () =>
{
    return Results.Ok(employees.Select(employee => new GetEmployeeResponse
    {
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        Address1 = employee.Address1,
        Address2 = employee.Address2,
        City = employee.City,
        State = employee.State,
        ZipCode = employee.ZipCode,
        PhoneNumber = employee.PhoneNumber,
        Email = employee.Email
    }));
});

employeeRoute.MapGet("{id:int}", (int id) =>
{
    var employee = employees.SingleOrDefault(e => e.Id == id);
    if (employee == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(new GetEmployeeResponse
    {
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        Address1 = employee.Address1,
        Address2 = employee.Address2,
        City = employee.City,
        State = employee.State,
        ZipCode = employee.ZipCode,
        PhoneNumber = employee.PhoneNumber,
        Email = employee.Email
    });
});

employeeRoute.MapPost(string.Empty, (CreateEmployeeRequest employee) =>
{
    var newEmployee = new Employee
    {
        Id = employees.Max(e => e.Id) + 1,
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        SocialSecurityNumber = employee.SocialSecurityNumber,
        Address1 = employee.Address1,
        Address2 = employee.Address2,
        City = employee.City,
        State = employee.State,
        ZipCode = employee.ZipCode,
        PhoneNumber = employee.PhoneNumber,
        Email = employee.Email
    };
    employees.Add(newEmployee);
    return Results.Created($"/employees/{newEmployee.Id}", employee);
});

employeeRoute.MapPut("{id}", (UpdateEmployeeRequest employee, int id) =>
{
    var existingEmployee = employees.SingleOrDefault(e => e.Id == id);
    if (existingEmployee == null)
    {
        return Results.NotFound();
    }

    existingEmployee.Address1 = employee.Address1;
    existingEmployee.Address2 = employee.Address2;
    existingEmployee.City = employee.City;
    existingEmployee.State = employee.State;
    existingEmployee.ZipCode = employee.ZipCode;
    existingEmployee.PhoneNumber = employee.PhoneNumber;
    existingEmployee.Email = employee.Email;

    return Results.Ok(existingEmployee);
});


app.Run();

public partial class Program { }