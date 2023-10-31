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
//注册signalR
builder.Services.AddSignalR();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//注册NLog组件
builder.Logging.ClearProviders();
builder.Host.UseNLog();

//使用autofac依赖注入
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//让控制器实例由容器创建
builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

//注册配置文件
builder.Services.AddOptions().Configure<Configs>(e => { builder.Configuration.GetSection("Config").Bind(e); });

//注册IHttpClientFactory
builder.Services.AddHttpClient();

//注册数据库配置
builder.Services.AddDbContext<StockDbContext>();

//注册后台服务
builder.Services.AddHostedServices();

//注入服务
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule<AutofacModule>();
});

var app = builder.Build();

//获取所有注入服务，用于手动获取实例
IocManager.InitContainer(app.Services.GetAutofacRoot());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

//添加signalR总线
app.MapHub<StockRealTimeHub>("/Hubs/StockRealTimeHub");

app.MapControllers();

app.Run();