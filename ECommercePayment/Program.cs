using ECommercePayment.Application.Interfaces;
using ECommercePayment.Application.Services;
using ECommercePayment.Infrastructure.BalanceManagement;
using ECommercePayment.Infrastructure.Persistence;
using ECommercePayment.Infrastructure.Resilience;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ECommerce Payment API",
        Version = "v1",
        Description = "A clean architecture-based e-commerce payment API that integrates with Balance Management service"
    });
});

// Database Context
builder.Services.AddDbContext<ECommerceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// HttpClient for Balance Management with Resilience Policies
builder.Services.AddHttpClient<IBalanceManagementClient, BalanceManagementClient>(client =>
{
    client.BaseAddress = new Uri("https://balance-management-pi44.onrender.com");
    client.Timeout = TimeSpan.FromSeconds(30); // Overall timeout
})
.AddPolicyHandler(ResiliencePolicies.GetCombinedPolicy()); // Retry + Circuit Breaker + Timeout

// Register services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce Payment API v1");
        c.RoutePrefix = string.Empty; // Swagger UI at root URL
        c.DocumentTitle = "ECommerce Payment API";
        c.DefaultModelsExpandDepth(-1); // Hide models section by default
    });
}

app.UseMiddleware<ECommercePayment.Middleware.ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
