using MySql.Data.MySqlClient;

async Task<MySqlConnection> CreateConnection()
{
    MySqlConnectionStringBuilder mscsb = new();
    mscsb.Server = "localhost";
    mscsb.Database = "fib";
    mscsb.Port = 3306;
    mscsb.UserID = "fib";
    mscsb.Password = "fib";

    var conn = new MySqlConnection(mscsb.ConnectionString);
    await conn.OpenAsync();

    return conn;
}

async Task MakeSureTableExists(MySqlConnection conn)
{
    var cmd = conn.CreateCommand();

    cmd.CommandText = "CREATE TABLE IF NOT EXISTS history (id INTEGER PRIMARY KEY AUTO_INCREMENT, n INTEGER, val BIGINT)";

    await cmd.ExecuteNonQueryAsync();
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(opt => { opt.AddDefaultPolicy(bld => bld.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()); });

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/history", async () =>
{
    await using var conn = await CreateConnection();

    await MakeSureTableExists(conn);

    var cmd = conn.CreateCommand();

    cmd.CommandText = "SELECT n, val FROM history ORDER BY id DESC LIMIT 10";

    var r = await cmd.ExecuteReaderAsync();

    List<(int, int)> tuples = new();

    while (await r.ReadAsync())
    {
        tuples.Add((r.GetInt32(0), r.GetInt32(1)));
    }

    return tuples.Select(t => new { n = t.Item1, value = t.Item2 });
});

app.MapPost("/fibonacci", async (FibRequest fibr) =>
{
    if (fibr.n <= 0)
    {
        return new
        {
            success = false,
            error = "n musi byc wieksze od zera",
            fib = -1
        };
    }

    if (fibr.n > 20)
    {
        return new
        {
            success = false,
            error = "n musi byc mniejsze lub rowne 20",
            fib = -1
        };
    }

    int[] fib = new int[fibr.n + 1];

    fib[0] = 0;
    fib[1] = 1;

    for (var i = 2; i <= fibr.n; i++)
    {
        fib[i] = fib[i - 2] + fib[i - 1];
    }

    int nFib = fib[fibr.n];

    await using var conn = await CreateConnection();

    var cmd = conn.CreateCommand();

    cmd.CommandText = "INSERT INTO history (n, val) VALUES (@n, @val)";
    cmd.Parameters.AddWithValue("n", fibr.n);
    cmd.Parameters.AddWithValue("val", nFib);

    await cmd.ExecuteNonQueryAsync();

    return new
    {
        success = true,
        error = null as string,
        fib = nFib
    };
});

app.Run();

internal record FibRequest(int n);