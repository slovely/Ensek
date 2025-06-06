var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("db")
        .AddDatabase("ensek-db")
    ;

var api = builder.AddProject<Ensek_Api>("api")
        .WithReference(db)
        .WaitFor(db)
    ;

builder.Build().Run();