using LastDanceAPI.DTO;
using LastDanceAPI.Entities;
using LastDanceAPI.Services;

namespace LastDanceAPI.Endpoints
{
    public static class OrdersEndpoints
    {
        public static void MapOrdersEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/orders", async (OrdersDto dto, IOrdersService service) =>
            {
                var entity = new Orders
                {
                    ordClientID = dto.ordClientID,
                    ordName = dto.ordName,
                    ordTotalAmount = dto.ordTotalAmount,
                    ordDescription = dto.ordDescription,
                };

                var created = await service.SaveAsync(entity);
                return created != null
                    ? Results.Created($"/orders/{created.ordID}", created)
                    : Results.BadRequest();
            })
            .WithName("CreateOrder")
            .WithOpenApi()
            .Produces<Orders>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

            app.MapGet("/ordersNotDeleted", async (IOrdersService service) =>
            {
                var orders = await service.GetAllOrdersNotDeletedAsync();
                return orders.Any()
                    ? Results.Ok(orders)
                    : Results.NoContent();
            })
            .WithName("GetOrders")
            .WithOpenApi()
            .Produces<IEnumerable<Orders>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent);

            app.MapPut("/orders/{id:int}", async (int id, OrdersUpdateDto dto, IOrdersService service) =>
            {
                var entity = new Orders
                {
                    ordID = id,
                    ordClientID = dto.ordClientID,
                    ordName = dto.ordName,
                    ordStatus = dto.ordStatus ?? "em processamento",
                    ordTotalAmount = dto.ordTotalAmount,
                    ordDescription = dto.ordDescription,
                    ordDelivered = dto.ordDelivered
                };

                var updated = await service.UpdateAsync(entity);
                return updated != null
                    ? Results.Ok(updated)
                    : Results.NotFound();
            })
            .WithName("UpdateOrder")
            .WithOpenApi()
            .Produces<Orders>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

            app.MapDelete("/orders/{id:int}", async (int id, IOrdersService service) =>
            {
                var dto = new OrdersDeleteDto
                {
                    ordID = id,
                    ordStatus = "removido"
                };

                var deleted = await service.SoftDeleteAsync(dto);
                if (deleted)
                    return Results.NoContent();
                else
                    return Results.NotFound();
            })
            .WithName("SoftDeleteOrder")
            .WithOpenApi()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}
