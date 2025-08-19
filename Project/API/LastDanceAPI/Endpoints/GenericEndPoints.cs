using LastDanceAPI.Repositories;
using Microsoft.Data.SqlClient;


namespace LastDanceAPI.Endpoints
{
    public static class GenericEndpoints
    {
        public static void MapGenericEndpoints<T>(
            this IEndpointRouteBuilder app,
            string route) where T : class
        {

            //endpoint for getting all items

            app.MapGet($"/{route}", async (IGenericRepository<T> repository) =>
            {
                var items = await repository.GetAll();
                return Results.Ok(items);
            })
            .WithName($"Get{typeof(T).Name}s")
            .WithOpenApi()
            .Produces<IEnumerable<T>>(StatusCodes.Status200OK);

            //endpoint for getting an item by ID

            app.MapGet($"/{route}/{{id:int}}", async (int id, IGenericRepository<T> repository) =>
            {
                var item = await repository.GetByID(id);
                return item != null ? Results.Ok(item) : Results.NotFound();
            })
            .WithName($"Get{typeof(T).Name}ById")
            .WithOpenApi()
            .Produces<T>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
        }
    }
}

