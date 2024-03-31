using ChartsProject.Services;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the IHttpClientFactory in the typed way

IHttpClientBuilder httpClientBuilder = builder.Services.AddHttpClient<GithubHttpService>(client =>
{
    client.BaseAddress = new Uri("https://api.github.com/");
    client.DefaultRequestHeaders.UserAgent.ParseAdd("chartsProject");
});

IHttpClientBuilder httpClientBuilder2 = builder.Services.AddHttpClient<NpmHttpService>(client =>
{
    client.BaseAddress = new Uri("https://api.npmjs.org/downloads/range/2023-01-01:2024-01-01/");
    client.DefaultRequestHeaders.UserAgent.ParseAdd("chartsProject");
});

httpClientBuilder.AddResilienceHandler("GithubServicePipeline", builder =>
{
    // Add Retry Strategies
    HttpRetryStrategyOptions retryOptions = new() {

        MaxRetryAttempts = 5,
        BackoffType = DelayBackoffType.Exponential,
        UseJitter = true,
        OnRetry = static args => 
        {
            Console.WriteLine("OnRetry, attempt {0}: ", args.AttemptNumber);
            return default;
        }
    };

    builder.AddRetry(retryOptions);

    // Add Timeout Strategies
    HttpTimeoutStrategyOptions timeOutOptions = new()
    {
        Timeout = TimeSpan.FromSeconds(3),
        OnTimeout = static args => {

            Console.WriteLine("Timeout limit has been exceeded");
            return default;
        }
    };
    builder.AddTimeout(timeOutOptions);
});

httpClientBuilder.AddResilienceHandler("NpmServicePipeline", builder =>
{
    // Add Retry Strategies
    HttpRetryStrategyOptions retryOptions = new()
    {

        MaxRetryAttempts = 5,
        BackoffType = DelayBackoffType.Exponential,
        UseJitter = true,
        OnRetry = static args =>
        {
            Console.WriteLine("OnRetry, attempt {0}: ", args.AttemptNumber);
            return default;
        }
    };

    builder.AddRetry(retryOptions);

    // Add Timeout Strategies
    HttpTimeoutStrategyOptions timeOutOptions = new()
    {
        Timeout = TimeSpan.FromSeconds(3),
        OnTimeout = static args => {

            Console.WriteLine("Timeout limit has been exceeded");
            return default;
        }
    };
    builder.AddTimeout(timeOutOptions);
});

// build the app
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
