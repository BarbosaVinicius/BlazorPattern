namespace BlazorPattern.Services
{
    /// <summary>
    /// Dependency-injection registration for the Pattern feature, kept out of Program.cs
    /// so the composition root stays short as features are added.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the Pattern repository and service.
        /// </summary>
        /// <remarks>
        /// The repository is a singleton because the in-memory store must outlive each Blazor Server circuit.
        /// A database-backed repository (e.g. EF Core DbContext) should normally be scoped instead.
        /// The service is scoped: one instance per circuit.
        /// </remarks>
        public static IServiceCollection AddPatternServices(this IServiceCollection services)
        {
            services.AddSingleton<IPatternRepository, InMemoryPatternRepository>();
            services.AddScoped<IPatternService, PatternService>();
            return services;
        }
    }
}
