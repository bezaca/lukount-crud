using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lukount.Repositories;
using Lukount.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Lukount
{
	public class Startup
	{
		private readonly string _cors = "cors";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IMongoClient>(
				ServiceProvider =>
				{
					var settings = Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
					return new MongoClient(settings.ConnectionString);
				}
			);

			services.AddSingleton<IPersonRepository, MongoDbPersons>();

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lukount", Version = "v1" });
			});
			services.AddControllers(options =>
			{
				options.SuppressAsyncSuffixInActionNames = false;
			});

			services.AddCors(options =>
			{
				options.AddPolicy(name: _cors, builder =>
				{
					builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
					.AllowAnyHeader().AllowAnyMethod();
				});
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseHttpsRedirection();
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lukount v1"));
			}


			app.UseRouting();

			app.UseCors(_cors);

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}

