using LastDanceAPI.DTO;
using LastDanceAPI.Entities;
using LastDanceAPI.Services;

public static class ClientEndpoints
{
    public static void MapClientEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/clientsNotDeleted", async (IClientService service) =>
        {
            var clients = await service.GetAllClientsNotDeletedAsync();

            return clients.Any()
                ? Results.Ok(clients)
                : Results.NoContent();
        })
        .WithName("GetClients")
        .WithOpenApi()
        .Produces<IEnumerable<Clients>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status204NoContent);

        app.MapPost("/clients", async (ClientDto dto, IClientService service) =>
        {
            var entity = new Clients
            {
                cltName = dto.cltName,
                cltSurname = dto.cltSurname,
                cltEmail = dto.cltEmail,
                cltPhoneNumber = dto.cltPhoneNumber,
                cltGender = dto.cltGender
            };

            var created = await service.SaveAsync(entity);
            return created != null
                ? Results.Created($"/clients/{created.cltID}", created)
                : Results.BadRequest();
        })
        .WithName("CreateClient")
        .WithOpenApi()
        .Produces<Clients>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        app.MapPut("/clients/{id:int}", async (int id, ClientUpdateDto dto, IClientService service) =>
        {
            var clientEntity = new Clients
            {
                cltID = id,
                cltName = dto.cltName,
                cltSurname = dto.cltSurname,
                cltEmail = dto.cltEmail,
                cltPhoneNumber = dto.cltPhoneNumber,
                cltGender = dto.cltGender,
                cltActive = dto.cltActive,
                cltStatus = dto.cltStatus
            };

            var updatedClient = await service.UpdateAsync(clientEntity);

            if (updatedClient == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(updatedClient);
        })
        .WithName("UpdateClient")
        .WithOpenApi()
        .Produces<Clients>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        app.MapDelete("/clients/{id:int}", async (int id, IClientService service) =>
        {

            var dto = new ClientDeleteDto
            {
                cltID = id,
                cltStatus = "removido"
            };

            var deleted = await service.SoftDeleteAsync(dto);
            if (deleted)
                return Results.NoContent();
            else
                return Results.NotFound();
        })
        .WithName("SoftDeleteClient")
        .WithOpenApi()
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}