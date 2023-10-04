using Microsoft.AspNetCore.Hosting;
using NLog.Web;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Stock.Common;
using Stock.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.ClearProviders();
builder.Host.UseNLog();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
builder.Services.AddOptions().Configure<Configs>(e => { builder.Configuration.GetSection("Config").Bind(e); });
builder.Services.AddHttpClient();
builder.Services.AddDbContext<StockDbContext>();
builder.Services.AddHostedServices();
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule<AutofacModule>();
});
var app = builder.Build();
IocManager.InitContainer(app.Services.GetAutofacRoot());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();