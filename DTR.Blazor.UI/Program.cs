using DTR.Blazor.UI.Components;
using DTR.Blazor.UI.Service;
using DTR.Blazor.UI.Service.Handler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents();


builder.Services.AddScoped<TokenService>();

//Register Api server that generates token
builder.Services.AddTransient<AuthHeaderHandler>();

builder.Services.AddHttpClient("AuthorizedApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7222/");
})
.AddHttpMessageHandler<AuthHeaderHandler>();
//End Handler


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>();

app.Run();
