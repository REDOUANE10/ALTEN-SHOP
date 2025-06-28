using System.Text.Json.Serialization;
using WebProduct.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
try
	{
    builder.Services

     .AddControllers()

     .AddJsonOptions(options =>

     {

         options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

     });
 

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/
    builder.Services.AddApplicationServices(builder.Configuration);
    builder.Services.AddIdentityServices(builder.Configuration);


    builder.Services.AddOpenApi();
    builder.Services.AddSwaggerGen();
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.WithOrigins("http://localhost:4200") // URL de ton front Angular
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
    });
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
        {
        app.UseDeveloperExceptionPage();
        
        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();
        }

    app.UseHttpsRedirection();
  
    app.UseCors();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

    }
catch (Exception ex)
	{
    Console.WriteLine($"Une erreur est survenue : {ex.Message}");

    throw;
	}


