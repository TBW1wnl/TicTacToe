var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.API>("api");

builder.AddProject<Projects.MauiApp>("mauiapp");

builder.Build().Run();
