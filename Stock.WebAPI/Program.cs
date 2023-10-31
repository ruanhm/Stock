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
//ע��signalR
builder.Services.AddSignalR();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//ע��NLog���
builder.Logging.ClearProviders();
builder.Host.UseNLog();

//ʹ��autofac����ע��
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//�ÿ�����ʵ������������
builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

//ע�������ļ�
builder.Services.AddOptions().Configure<Configs>(e => { builder.Configuration.GetSection("Config").Bind(e); });

//ע��IHttpClientFactory
builder.Services.AddHttpClient();

//ע�����ݿ�����
builder.Services.AddDbContext<StockDbContext>();

//ע���̨����
builder.Services.AddHostedServices();

//ע�����
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule<AutofacModule>();
});

var app = builder.Build();

//��ȡ����ע����������ֶ���ȡʵ��
IocManager.InitContainer(app.Services.GetAutofacRoot());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

//���signalR����
app.MapHub<StockRealTimeHub>("/Hubs/StockRealTimeHub");

app.MapControllers();

app.Run();