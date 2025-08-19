using LastDanceAPI.Endpoints;
using LastDanceAPI.Entities;
using LastDanceAPI.Repositories;
using LastDanceAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Swagger

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Clients generico

builder.Services.AddScoped<IGenericRepository<Clients>>(sp =>
    new GenericRepository<Clients>(
        sp.GetRequiredService<IConfiguration>(),
        "ld.tClients", "cltID"));

builder.Services.AddScoped<IGenericService<Clients>, GenericService<Clients>>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();

// Orders generico

builder.Services.AddScoped<IGenericRepository<Orders>>(sp =>
    new GenericRepository<Orders>(
        sp.GetRequiredService<IConfiguration>(),
        "ld.tOrders", "ordID"));


builder.Services.AddScoped<IGenericService<Orders>, GenericService<Orders>>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IOrdersService, OrdersService>();

var app = builder.Build();

// Ativa Swagger

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Endpoints
app.MapGenericEndpoints<Clients>("clients");
app.MapGenericEndpoints<Orders>("orders");
app.MapClientEndpoints();
app.MapOrdersEndpoints();

app.Run();
