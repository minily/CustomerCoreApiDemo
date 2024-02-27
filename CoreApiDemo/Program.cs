using CoreApiDemo.Services;

// test modify
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 允许跨域
builder.Services.AddHttpContextAccessor();

// 健康检查
builder.Services.AddHealthChecks();

//builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 服务注入
builder.Services.AddSingleton<ICustomerCacheService, CustomerCacheService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();

// 设置日志过滤
builder.Logging.AddFilter((provider, category, logLevel) =>
{
    return !new[] { "Microsoft.Hosting", "Microsoft.AspNetCore" }.Any(u => category.StartsWith(u)) && logLevel >= LogLevel.Information;
});

// 设置接口超时时间
builder.Configuration.Get<WebHostBuilder>().ConfigureKestrel(u =>
{
    u.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(30);
    u.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(30);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 健康检查
app.UseHealthChecks("/health", 8000);

app.UseAuthorization();

app.MapControllers();

app.Run();
