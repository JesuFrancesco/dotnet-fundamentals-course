using Application.UseCases.Persons;
using Application.UseCases.Visits;
using Data;
using WebApi.Endpoints;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Falta la conexión a la db");
            Console.WriteLine(connectionString);

            // evil method extension for IServiceCollection class 
            builder.Services.AddInfrastructure(connectionString);

            //builder.Services.AddScoped<IRepository<PersonEntity, Guid>, PersonRepository>();
            //builder.Services.AddScoped<ICodeRepository<PersonEntity>, PersonRepository>();

            builder.Services.AddScoped<CreatePersonUseCase>();
            builder.Services.AddScoped<DeletePersonUseCase>();
            builder.Services.AddScoped<GetAllPersonsUseCase>();
            builder.Services.AddScoped<GetPersonByCodeUseCase>();
            builder.Services.AddScoped<GetPersonByIdUseCase>();
            builder.Services.AddScoped<UpdatePersonUseCase>();

            builder.Services.AddScoped<GetActiveVisitsUseCase>();
            builder.Services.AddScoped<GetAllVisitsUseCase>();
            builder.Services.AddScoped<GetVisitsByPersonUseCase>();
            builder.Services.AddScoped<RegisterVisitEntryUseCase>();
            builder.Services.AddScoped<RegisterVisitExitUseCase>();

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

            var apiV1 = app.MapGroup("/api/v1");
            apiV1.MapPersonsEndpoints();
            apiV1.MapVisitsEndpoints();

            app.Run();
        }
    }
}
