using Autofac;
using Autofac.Core.Activators.Reflection;
using Autofac.Extensions.DependencyInjection;
using EasyNetQ;
using RabbitMQ.Consumer;
using RabbitMQ.Consumer.Consume;
using Shared.RabbitMQ.Manager;
using Microsoft.AspNetCore.Builder;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ��l�ƨëإߤ@�ӹ��
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
// ���Uautofac�o�Ӯe��
//builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacModuleRegister()));

// RabbitMQ DI
builder.Services.AddSingleton(new RabbitMQConsumerService(RabbitHutch.CreateBus(builder.Configuration["RabbitMqTcpConnectionString"])));

// �ܧ�DI IMessageConsume
builder.Services.AddTransient<CommonMessageConsume>();
builder.Services.AddTransient<FanoutMessageConsume>();

builder.Services.AddTransient<SubscribeService>();

builder.Services.AddLogging(loggingBuilder => { loggingBuilder.AddSeq(builder.Configuration.GetSection("Seq")); });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// �q�\���wRabbitMQ Queue
app.MessageQueueSubscribe();

Consts.IocContainer = app.Services.GetAutofacRoot();

app.Run();
