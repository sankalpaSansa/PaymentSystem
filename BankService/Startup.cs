using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PaymentSystem.Repo;
using PaymentSystem.Repo.Entities;
using PaymentSystem.Repo.Interfaces;
using PaymentSystem.Service;
using PaymentSystem.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //services.AddDbContext<EntityDBContex>();

            //DI service
            services.AddScoped<IPayService, PayService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITopupService, TopupServices>();

            services.AddMvc(
               options => options.EnableEndpointRouting = false
           ).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            //DI Repo

            services.AddScoped<IPayRepo, PayRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<ITopupRepo, TopupRepo>();

            services.AddDbContext<EntityDBContex>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("PaymentSystem")));

            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);

                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, EntityDBContex db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            DbInitializer.Initialize(db);
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                //_appConfiguration["App:ServerRootAddress"]
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }

}

public static class DbInitializer
{
    public static void Initialize(EntityDBContex context)
    {
        //context.Database.EnsureCreated();

        //// Look for any students.
        //if (context.Wallets.Any())
        //{
        //    return;   // DB has been seeded
        //}

        //var Users = new User[]
        //{
        //         new User() { UserId = Guid.Parse("ee26f298-2d5f-4ea1-9219-a5f1573e48e9"), Username = "Alice" },
        //        new User() { UserId = Guid.Parse("bbc8b783-6ecc-44a3-99b1-bbd830202720"), Username = "Bob" }
        //};
        //foreach (User u in Users)
        //{
        //    context.Users.Add(u);
        //}
        //context.SaveChanges();

        //var wallets = new Wallet[]
        //{
        //    new Wallet() { WalletId = Guid.Parse("7814dc3c-84a1-4e37-945d-60aa0d980f78"),  UserId = Guid.Parse("ee26f298-2d5f-4ea1-9219-a5f1573e48e9"), Amount =100},
        //   new Wallet() { WalletId = Guid.Parse("4cb7be70-a084-4e3f-9600-9e5cb567f344"), UserId = Guid.Parse("bbc8b783-6ecc-44a3-99b1-bbd830202720"), Amount = 50 }
        //};
        //foreach (Wallet w in wallets)
        //{
        //    context.Wallets.Add(w);
        //}
        //context.SaveChanges();



    }
}
