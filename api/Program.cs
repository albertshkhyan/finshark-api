var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

/*
 * C# Inferred Types
 * compiler to automatically determine the type of a variable based on the value assigned to it.

 * How `var` Works
   When you use var, the compiler infers the type of the variable based on the expression used to initialize it.
   This means the type is determined during the compilation process, not during the execution.

 * new[]
 * The new[] syntax is used to create a new array instance. The type of the array is inferred from the elements
 * provided in the initializer list.
 * It's a way to quickly create an array without explicitly specifying the type.
 *
 var numbers = new[] { 1, 2, 3, 4, 5 }; // Inferred as int[]
 var greetings = new[] { "hello", "world" }; // Inferred as string[]
 var mixedNumbers = new[] { 1, 2.0, 3.5 }; // Inferred as double[]

Using Object for Mixed Types (Not Recommended for Type Safety)
var mixed = new object[] { 1, 2, 3, 4, 5, "hello" }; // Inferred as object[]

the line var numbers = new[] { 1, 2, 3, 4, 5, "hello" }; is not correct because it mixes integers and a string in the
array initializer. In C#, all elements in an array must be of the same type or implicitly convertible to a common type.
Since int and string are not compatible types, the compiler cannot infer a common type for the array.
 */

/*
   ### 1. Using `var` with Type Inference

   ```csharp
   var summaries = new[]
   {
       "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
   };
   ```
   - **Pros:**
     - **Simplicity:** The code is concise and easy to read. 😊
     - **Less Redundancy:** No need to specify the type multiple times. ✔️
     - **Modern Style:** Fits well with modern C# practices of type inference. 🆕

   - **Cons:**
     - **Explicitness:** The type of `summaries` is not immediately obvious without understanding type inference. 🤔

   ### 2. Explicit Type with Inferred Initialization

   ```csharp
   string[] summaries = new[]
   {
       "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
   };
   ```
   - **Pros:**
     - **Clarity:** The type of `summaries` is explicitly stated, making it clear to the reader. 🔍
     - **Type Safety:** Ensures that the variable is of the correct type. ✔️

   - **Cons:**
     - **Redundancy:** Slightly redundant because the type is specified twice (both in the variable declaration and inferred during initialization). 😐

   ### 3. Fully Explicit Type Declaration

   ```csharp
   string[] summaries = new string[]
   {
       "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
   };
   ```
   - **Pros:**
     - **Explicitness:** Completely explicit, leaving no doubt about the type of `summaries`. ✅
     - **Clarity:** The type and initialization are very clear. 🔍

   - **Cons:**
     - **Verbosity:** More verbose than necessary, which can make the code slightly less readable and more cumbersome to write. 🥱

   ### Best Practice Recommendation

   **Option 2: Explicit Type with Inferred Initialization**

   ```csharp
   string[] summaries = new[]
   {
       "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
   };
   ```

   ### Reasoning

   - **Balance:** This approach strikes a good balance between clarity and brevity. ⚖️
   - **Explicitness:** Clearly shows the type of the variable without being overly verbose. 📝
   - **Modern and Readable:** Adopts modern C# practices while remaining readable and easy to understand. 📖

   ### Summary

   - **Use `var` (Option 1):** If you prefer conciseness and the type is clear from the context. 😊
   - **Explicit Type with Inferred Initialization (Option 2):** Recommended for a balance of clarity and brevity. ✔️
   - **Fully Explicit (Option 3):** Use when you want maximum explicitness, but it may be more verbose than necessary. 🥱

   This format should help you visually understand the pros and cons of each approach more easily. If you have any further questions or need more details, feel free to ask!
 */
string[] summaries = new string[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

// New temperature statistics endpoint
app.MapGet("/temperaturestats", () =>
    {
        var forecast = Enumerable.Range(1, 10).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();

        // Using LINQ methods to calculate max, min, and average temperature
        var highestTemperature = forecast.Max(f => f.TemperatureC);
        var lowestTemperature = forecast.Min(f => f.TemperatureC);
        var averageTemperature = forecast.Average(f => f.TemperatureC);

        // Outputting the results
        Console.WriteLine($"Highest Temperature: {highestTemperature}C");
        Console.WriteLine($"Lowest Temperature: {lowestTemperature}C");
        Console.WriteLine($"Average Temperature: {averageTemperature}C");

        // Swagger (via Swashbuckle in ASP.NET Core) has trouble generating schemas for anonymous types because they
        // don't have a defined structure at compile time.

        // var stats
        //     = new
        // {
        //     HighestTemperature = highestTemperature,
        //     LowestTemperature = lowestTemperature,
        //     AverageTemperature = averageTemperature
        // };

        // Create an instance of the TemperatureStats class and initialize its properties
        var stats = new TemperatureStats
        {
            // Set the HighestTemperature property to the highest temperature from the forecast data
            HighestTemperature = highestTemperature,

            // Set the LowestTemperature property to the lowest temperature from the forecast data
            LowestTemperature = lowestTemperature,

            // Set the AverageTemperature property to the average temperature from the forecast data
            AverageTemperature = averageTemperature
        };

        // Return the stats object to the client
        return stats;
    })
    // While the name itself might not be directly visible in the Swagger UI, it can help organize and reference
    // endpoints. The names make it easier to identify and understand the purpose of each endpoint in your documentation and codebase.
    .WithName("GetTemperatureStats")// improving code readability
    /*
     * Swashbuckle: A .NET tool that generates interactive API documentation.
       OpenAPI: A standard format for describing APIs.
       Together: They create easy-to-understand, testable API documentation.
     */
    .WithOpenApi();// The annotated endpoint will appear in the Swagger UI

app.Run();


/*
 * Immutable Objects
 *  are objects whose state cannot be changed after they are created. Once you create an immutable
 * object, you cannot modify its properties or fields (Unchangeable State).
 *
 * Records are often used to create immutable objects. By default, properties in a record are immutable (read-only),
 * but you can define mutable properties if needed.
 */


/*
 * Records
 *  are often used to create immutable objects. By default, properties in a record are immutable (read-only),
 * but you can define mutable properties if needed.
 */
/*
 * Analogous Concepts in Nest.js
   In Nest.js, you would typically use TypeScript classes to define data models and DTOs (Data Transfer Objects)
   similarly to how you use records and classes in ASP.NET Core.
 */
//WeatherForecast DTO
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
// that automatically creates properties for Date, TemperatureC, and Summary.
// The properties Date, TemperatureC, and Summary are immutable by default.
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

// TemperatureStats DTO
// Define a named class for temperature statistics
public class TemperatureStats
{
    public int HighestTemperature { get; set; }
    public int LowestTemperature { get; set; }
    public double AverageTemperature { get; set; }
}
