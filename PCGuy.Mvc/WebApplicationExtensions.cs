namespace PCGuy.Mvc;

public static class WebApplicationExtensions
{
    public static WebApplication MapGets(this WebApplication app)
    {
        // app.MapGet("/", () => "Hello from a native AOT minimal API web service.");
        // app.MapGet("/products", GetProducts);
        // app.MapGet("/products/{minimumUnitPrice:decimal?}", GetProducts);
        return app;
    }

    public static WebApplication MapControllerRoutes(this WebApplication app)
    {
        // app.MapControllerRoute(
//     name: "products",
//     pattern: "products/peripherals",
//     defaults: new { controller = "Product", action = "Index", id = 3 });
// app.MapControllerRoute(
//     name: "products",
//     pattern: "products/pc-parts",
//     defaults: new { controller = "Product", action = "Index", id = 2 });
// app.MapControllerRoute(
//     name: "products",
//     pattern: "products/software",
//     defaults: new { controller = "Product", action = "Index", id = 1 });
// app.MapControllerRoute(
//     name: "products",
//     pattern: "products/{id?}",
//     defaults: new { controller = "Product", action="Index" });
// app.MapControllerRoute(
//     name: "products",
//     pattern: "products",
//     defaults: new { controller = "Product", action="Index", id = 0 });
        return app;
    }
}