
using Confluent.Kafka;
using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producer;
using MongoDB.Bson.Serialization;
using Post.Cmd.Api.Commands;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Config;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Cmd.Infrastructure.Handler;
using Post.Cmd.Infrastructure.Producer;
using Post.Cmd.Infrastructure.Repositories;
using Post.Cmd.Infrastructure.Stores;
using Post.Common.Events;

var builder = WebApplication.CreateBuilder(args);

BsonClassMap.RegisterClassMap<BaseEvents>();
BsonClassMap.RegisterClassMap<PostCreatedEvent>();
BsonClassMap.RegisterClassMap<MessageUpdatedEvent>();
BsonClassMap.RegisterClassMap<PostLikedEvent>();
BsonClassMap.RegisterClassMap<CommentAddedEvent>();
BsonClassMap.RegisterClassMap<CommentUpdatedEvent>();
BsonClassMap.RegisterClassMap<CommentRemovedEvent>();
BsonClassMap.RegisterClassMap<PostRemovedEvent>();

// Add services to the container.
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventProducer, EventProducer>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourceHandler<PostAggregate>, EventSourcingHanlder>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();

// register command handler methods
var commandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var dispatcher = new CommandDispatcher();
 dispatcher.RegisterHandler<NewPostCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<EditMessageCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<LikePostCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<AddCommentCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<EditCommentCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<RemoveCommentCommand>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<DeletePostCommand>(commandHandler.HandleAsync);
//dispatcher.RegisterHandler<RestoreReadDbCommand>(commandHandler.HandleAsync);
builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();






















//using Confluent.Kafka;
//using CQRS.Core.Domain;
//using CQRS.Core.Handlers;
//using CQRS.Core.Infrastructure;
//using CQRS.Core.Producer;
//using Post.Cmd.Api.Commands;
//using Post.Cmd.Domain.Aggregates;
//using Post.Cmd.Infrastructure.Config;
//using Post.Cmd.Infrastructure.Dispatchers;
//using Post.Cmd.Infrastructure.Handler;
//using Post.Cmd.Infrastructure.Producer;
//using Post.Cmd.Infrastructure.Repositories;
//using Post.Cmd.Infrastructure.Stores;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
//builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
//builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
//builder.Services.AddScoped<IEventProducer, EventProducer>();
//builder.Services.AddScoped<IEventStore, EventStore>();
//builder.Services.AddScoped<IEventSourceHandler<PostAggregate>, EventSourcingHanlder>();
//builder.Services.AddScoped<ICommandHandler, CommandHandler>();


////register Command handler methods
//var commandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
//var dispatcher = new CommandDispatcher();

//dispatcher.RegisterHandler<NewPostCommand>(commandHandler.HandlerAsync);
//dispatcher.RegisterHandler<EditMessageCommand>(commandHandler.HandlerAsync);
//dispatcher.RegisterHandler<LikePostCommand>(commandHandler.HandlerAsync);
//dispatcher.RegisterHandler<AddCommentCommand>(commandHandler.HandlerAsync);
//dispatcher.RegisterHandler<EditCommentCommand>(commandHandler.HandlerAsync);
//dispatcher.RegisterHandler<RemoveCommentCommand>(commandHandler.HandlerAsync);
//dispatcher.RegisterHandler<DeletePostCommand>(commandHandler.HandlerAsync);

//builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);


//builder.Services.AddControllers();
////builder.Services.AddHostedService<ConsumerHostedService>();

//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}


//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();

//record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}
