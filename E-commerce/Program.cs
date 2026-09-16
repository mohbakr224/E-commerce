
using E_commerce.Background;
using E_commerce.Data;
using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_commerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<ApplicationDB>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddHangfire(config => config.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));
            builder.Services.AddHangfireServer();
            builder.Services.AddScoped<OrderBackgroundWorker>();

            builder.Services.AddMediatR(typeof(Program).Assembly);

            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            app.UseHangfireDashboard("/hangfire");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            BackgroundJob.Enqueue<OrderBackgroundWorker>(
                x => x.GetOrderBackgroud()
                );
            app.Run();
        }
    }
}
