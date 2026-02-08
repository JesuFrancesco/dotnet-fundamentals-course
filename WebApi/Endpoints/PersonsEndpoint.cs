using Application.DTOs.Persons;
using Application.UseCases.Persons;

namespace WebApi.Endpoints
{
    public static class PersonsEndpoint
    {
        public static void MapPersonsEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("/persons").WithTags("Persons");

            group.MapGet("/{id:guid}", async (Guid id, GetPersonByIdUseCase useCase) =>
            {
                try
                {
                    var p = await useCase.ExecuteAsync(id);
                    return Results.Ok(p);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: ex.StackTrace);
                }
            }).WithName("GetPersonByIdEndpoint").WithDescription("Obtener persona por id").Produces(200).Produces(404);

            group.MapGet("/code/{code}", async (string code, GetPersonByCodeUseCase useCase) =>
            {
                try
                {
                    var p = await useCase.ExecuteAsync(code);
                    return Results.Ok(p);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: ex.StackTrace);
                }
            }).WithName("GetPersonByCodeEndpoint").WithDescription("Obtener persona por su código").Produces(200).Produces(404);

            group.MapGet("/", async (GetAllPersonsUseCase useCase) =>
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
            }).WithName("GetAllPersonsEndpoint").WithDescription("Obtener todas las personas").Produces(200).Produces(500);

            group.MapPost("/", async (CreatePersonDTO dto, CreatePersonUseCase useCase) =>
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
                }

            })
            .WithName("CreatePersonEndpoint")
            .WithDescription("Crea una persona")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            group.MapPut("/{id:guid}", async (Guid id, UpdatePersonDTO dto, UpdatePersonUseCase useCase) =>
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

            group.MapDelete("/{id:guid}", async (Guid id, DeletePersonUseCase useCase) =>
            {
                try
                {
                    await useCase.ExecuteAsync(id);
                    return Results.NoContent();
                }
                catch (ArgumentException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.StackTrace);
                }
            }).WithName("DeletePersonEndpoint")
                .WithDescription("Delete a person")
                .Produces(204)
                .Produces(404)
                .Produces(500);
        }
    }

}
