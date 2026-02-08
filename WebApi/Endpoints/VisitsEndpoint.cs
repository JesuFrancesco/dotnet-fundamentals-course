using Application.DTOs.Visits;
using Application.UseCases.Visits;

namespace WebApi.Endpoints
{
    public static class VisitsEndpoint
    {
        public static void MapVisitsEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("/visits").WithTags("Visits");

            group.MapPost("/entry", async (RegisterEntryDTO dto, RegisterVisitEntryUseCase useCase) =>
            {
                try
                {
                    var visit = await useCase.ExecuteAsync(dto);
                    return Results.Created($"{visit.Id}", visit);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }).Produces(201).Produces(400).Produces(500);

            group.MapPost("/exit", async (RegisterExitDTO dto, RegisterVisitExitUseCase useCase) =>
            {
                try
                {
                    var visit = await useCase.ExecuteAsync(dto);
                    return Results.Ok(visit);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }).Produces(200).Produces(400).Produces(500);

            group.MapGet("/", async (GetAllVisitsUseCase useCase) =>
            {
                try
                {
                    var visits = await useCase.ExecuteAsync();
                    return Results.Ok(visits);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }).Produces(200).Produces(400).Produces(500);

            group.MapGet("/active", async (GetActiveVisitsUseCase useCase) =>
            {
                try
                {
                    var visits = await useCase.ExecuteAsync();
                    return Results.Ok(visits);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }).Produces(200).Produces(400).Produces(500);

            group.MapGet("/person/{id:guid}", async (Guid id, GetVisitsByPersonUseCase useCase) =>
            {
                try
                {
                    var visits = await useCase.ExecuteAsync(id);
                    return Results.Ok(visits);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            }).Produces(200).Produces(400).Produces(500);
        }
    }
}
