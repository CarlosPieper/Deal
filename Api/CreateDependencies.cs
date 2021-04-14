namespace Api
{
    public class CreateDependencies
    {
        public static void AddSingletons(IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<ICryptographyService, CryptographyService>();
        }
    }
}