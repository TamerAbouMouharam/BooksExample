using Books.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

List<Book> books = new()
{
    new Book{Id = 1, Title = "C# Docs", Author = "Microsoft"},
    new Book{Id = 2, Title = "Java Docs", Author = "Oracle"}
};

app.MapGet("books", () =>
{
    return Results.Ok(books);
});

app.MapGet("books/{id}", (int id) =>
{
    var book = books.FirstOrDefault(book => book.Id == id);

    if(book != null)
    {
        return Results.Ok(book);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapPost("/books", (Book book) =>
{
    book.Id = books.Max(book => book.Id) + 1;

    books.Add(book);

    return Results.Created();
});

app.MapPut("/books/{id}", (int id, Book newInfo) =>
{
    var book = books.FirstOrDefault(book => book.Id == id);

    if(book != null)
    {
        book.Id = id;
        book.Title = newInfo.Title;
        book.Author = newInfo.Author;

        return Results.NoContent();
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapDelete("/books/{id}", (int id) =>
{
    var book = books.FirstOrDefault(book => book.Id == id);

    if(book != null)
    {
        books.Remove(book);
    }

    return Results.NoContent();

});

app.Run();
