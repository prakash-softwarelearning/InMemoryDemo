using InMemoryDemo.Middleware;
using InMemoryDemo.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace InMemoryDemo
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
            services.AddHttpContextAccessor();
            services.AddDbContext<EmployeeDbContext>(option => option.UseInMemoryDatabase(Configuration.GetConnectionString("MyDb")));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "InMemoryDemo", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InMemoryDemo v1"));
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            
            app.UseRouting();
            app.UseMiddleware<LogHeaderMiddleware>();
            app.UseMiddleware < RequestResponseLoggerMiddleware>();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



            var scope = app.ApplicationServices.CreateScope();

            var context = scope.ServiceProvider.GetService<EmployeeDbContext>();
            SeedData(context);
        }

        public static void SeedData(EmployeeDbContext context)
        {
            Employee emp1 = new Employee
            {
                Id = 1,
                Name = "Nishant",
                age = 30,
                Gender = "Male",
                salary = 30000
            };
            Employee emp2 = new Employee
            {
                Id = 2,
                Name = "Prakash",
                age = 32,
                Gender = "Male",
                salary = 20000
            };
            Employee emp3 = new Employee
            {
                Id = 3,
                Name = "Hiren",
                age = 31,
                Gender = "Male",
                salary = 40000
            };
            context.Employees.Add(emp1);
            context.Employees.Add(emp2);
            context.Employees.Add(emp3);
            context.SaveChanges();
        }
    }
}
