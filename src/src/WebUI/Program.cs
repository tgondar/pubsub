using EasyCaching.Core.Configurations;
using src.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebUIServices();

builder.Services.AddEasyCaching(option =>
{
    //option.WithJson("myjson");

    // local
    option.UseInMemory("m1");

    // distributed
    option.UseRedis(config =>
    {
        config.DBConfig.Endpoints.Add(new ServerEndPoint("127.0.0.1", 6379));
        config.DBConfig.Database = 0;
        config.SerializerName = "myjson";
    }, "myredis");

    // combine local and distributed
    option.UseHybrid(config =>
    {
        config.TopicName = "test-topic";
        config.EnableLogging = false;

        // specify the local cache provider name after v0.5.4
        config.LocalCacheProviderName = "m1";
        // specify the distributed cache provider name after v0.5.4
        config.DistributedCacheProviderName = "myredis";
    })
    // use redis bus
    .WithRedisBus(busConf =>
    {
        busConf.Endpoints.Add(new ServerEndPoint("127.0.0.1", 6380));

        // do not forget to set the SerializerName for the bus here !!
        busConf.SerializerName = "myjson";
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        scope.ServiceProvider.GetService<ApplicationDbContext>().Database.EnsureCreated();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwaggerUi3(settings =>
{
    settings.Path = "";
    settings.DocumentPath = "/api/specification.json";
});

app.UseRouting();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.Run();