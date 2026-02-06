using Application.DTOs.Persons;
using Application.UseCases.Persons;

namespace WebApi.Endpoints
{
    public static class PersonsEndpoint
    {
        public static void MapEndpoints(this IEndpointRouteBuilder builder)
        {
            var api = builder.MapGroup("/api/v1");

            var personGroup = api.MapGroup("/persons").WithTags("Persons");

            personGroup.MapGet("/{id:guid}", async (Guid id, GetPersonByIdUseCase useCase) =>
            {
                try
                {
                    var p = await useCase.ExecuteAsync(id);
                    return Results.Ok(p);
                }
                catch (Exception ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            }).WithName("GetPersonById").WithDescription("Obtener persona por id");

            personGroup.MapGet("/", async (GetAllPersonsUseCase useCase) =>
            {
                try
                {
                    var persons = await useCase.ExecuteAsync();
                    return Results.Ok(persons);

                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: ex.StackTrace);
                }
            });

            personGroup.MapPost("/", async (CreatePersonDTO dto, CreatePersonUseCase useCase) =>
            {
                try
                {
                    var person = await useCase.ExecuteAsync(dto);
                    return Results.Created($"/persons/{person.Id}", person);
                }
                catch (InvalidOperationException ex)
                {

                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (ArgumentException ex)
                {

                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                    //throw;
                }

            })
            .WithName("CreatePersonEndpoint")
            .WithDescription("yep")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

            personGroup.MapPut("/{id:guid}", async (Guid id, UpdatePersonDTO dto, UpdatePersonUseCase useCase) =>
            {
                try
                {
                    if (id != dto.Id)
                    {
                        throw new InvalidOperationException("Las id's no corresponden");
                    }
                    var updatedPerson = await useCase.ExecuteAsync(dto);
                    return Results.Ok(updatedPerson);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: ex.StackTrace);
                }
            }).WithName("UpdatePersonEndpoint")
                .WithDescription("Update person info")
                .Produces(200)
                .Produces(400)
                .Produces(500);
        }

    }

}
