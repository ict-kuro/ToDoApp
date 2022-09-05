using Microsoft.Data.SqlClient;
using ToDo_App.Models;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//セッション保存のため追加
builder.Services.AddDistributedMemoryCache();

//セッション処理の設定
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(180);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//DB接続
builder.Services.AddDbContext<TeamBContext>(provider => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnectionString")));

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

//セッションの使用を設定
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

