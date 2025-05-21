using Watch2getherManage.Services;

namespace Watch2getherManage.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Đăng ký các service tại đây

            //services.AddScoped<IMyService, MyService>();
            //services.AddSingleton<ICacheService, CacheService>();
            //services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<StreamKeyService>();

            // Có thể cấu hình thêm: AutoMapper, FluentValidation, HttpClient, etc.

            return services;
        }
    }
}
