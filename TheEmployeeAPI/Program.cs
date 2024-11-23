var builder = WebApplication.CreateBuilder(args);

var employees = new List<Employee>();

employees.Add(new Employee { Id = 1, FirstName = "John", LastName = "Doe" });
employees.Add(new Employee { Id = 2, FirstName = "Jane", LastName = "Doe" });

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

var employeeRoute = app.MapGroup("employees");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


employeeRoute.MapGet("/", () =>
{
    return Results.Ok(employees);
});

employeeRoute.MapGet("/{id}", (int id) =>
{
    var employee = employees.SingleOrDefault(e => e.Id == id);
    if (employee == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(employee);
});

employeeRoute.MapPost("/", (Employee employee) =>
{
    employee.Id = employees.Max(e => e.Id) + 1;
    employees.Add(employee);
    return Results.Created($"/employees/{employee.Id}", employee);
});

app.Run();

public partial class Program { }