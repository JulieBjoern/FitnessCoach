var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.FitnessCoach_Web>("fitnesscoach-web")
    .WithHttpHealthCheck("/health");

builder.Build().Run();
