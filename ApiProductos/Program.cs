using Microsoft.EntityFrameworkCore;
using ApiProductos.Data;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
// Consola funciona en App Service y no requiere permisos del Event Log de Windows.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "No se configuró ConnectionStrings:DefaultConnection.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
// EF puede registrar consultas y detalles de la conexion en sus excepciones.
// El manejador registra solamente tipo de error e identificador de solicitud.
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.None);

var app = builder.Build();

app.UseExceptionHandler(new ExceptionHandlerOptions
{
    SuppressDiagnosticsCallback = _ => true,
    ExceptionHandler = async context =>
    {
        var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var sqlError = error as SqlException ?? error?.InnerException as SqlException;
        app.Logger.LogError("Error {ErrorType}. Codigo SQL {SqlNumber}. Solicitud {TraceId}",
            error?.GetType().Name, sqlError?.Number, context.TraceIdentifier);
        await Results.Problem(statusCode: 500,
            title: "No fue posible completar la solicitud.",
            extensions: new Dictionary<string, object?> { ["traceId"] = context.TraceIdentifier })
            .ExecuteAsync(context);
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
