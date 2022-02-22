var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var AllowSomeStuff = "_AllowSomeStuff";
builder.Services.AddCors(options => 
{
    options.AddPolicy(name: AllowSomeStuff, builder => {
        builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors(AllowSomeStuff);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

Todo[] items = {
    new() {
        Id = 1,
        Text = "buy dogecoin"
    },
    new() {
        Id = 2,
        Text = "buy btc"
    },
    new() {
        Id = 3,
        Text = "buy tesla"
    }
};

app.MapGet("/todoitems", () => items)
.WithName("GetTodoItems");

app.MapGet("/todoitems/{id}", (int id) => items.First(item => item.Id == id));

app.MapPost("/todoitems", (Todo item) => items = items.Append(item).ToArray());

app.MapPut("/todoitems/{id}", (int id, Todo item) => 
{
    Todo updateThisItem = items.First(item => item.Id == id);
    updateThisItem.Text = item.Text;
    updateThisItem.Done = item.Done;
});

app.MapDelete("/todoitems/{id}", (int id) => 
{
    items = items.Where((Todo item) => item.Id != id).ToArray();
});

app.Run();

class Todo
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public bool Done { get; set; }
}
