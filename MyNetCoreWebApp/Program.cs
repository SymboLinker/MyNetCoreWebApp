using System.Globalization;

CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddIGet();
builder.Services.AddFunDb();

var app = builder.Build();

await PrefillFunDatabase(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

static async Task PrefillFunDatabase(WebApplication app)
{
    var db = app.Services.GetService<IFunDb>() ?? throw new NullReferenceException(nameof(IFunDb));
    await db.InsertAsync(new Workshop
    {
        Name = "Programming for beginners",
        DateTimeStart = DateTime.Now.AddHours(1),
    });
    await db.InsertAsync(new Workshop
    {
        Name = "Chess tactics",
        DateTimeStart = DateTime.Now.AddHours(4),
    });
    await db.InsertAsync(new Workshop
    {
        Name = "Creativity in math",
        DateTimeStart = DateTime.Now.AddHours(1),
    });
    await db.InsertAsync(new Workshop { Name = "Creativity and grit" });
}