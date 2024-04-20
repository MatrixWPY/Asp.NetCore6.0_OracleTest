using Oracle.ManagedDataAccess.Client;
using OracleTest.Repositories.Instance;
using OracleTest.Repositories.Interface;
using OracleTest.Services.Instance;
using OracleTest.Services.Interface;
using OracleTest.Utility;
using System.Data;

namespace OracleTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => {
                // XML 檔案: 文件註解標籤
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "OracleTest.xml");
                c.IncludeXmlComments(xmlPath);
            });

            #region 註冊DB連線
            var dbConnectString = (string)builder.Configuration.GetValue(typeof(string), "DBConnectionStrings:Oracle");
            builder.Services.AddScoped<IDbConnection, OracleConnection>(db => new OracleConnection(dbConnectString));
            #endregion

            #region 註冊Repository
            var dbType = builder.Configuration.GetValue(typeof(string), "DbType");
            switch (dbType)
            {
                case "RAW":
                    builder.Services.AddScoped<IProductRepository, ProductRawRepository>();
                    builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRawRepository>();
                    break;

                case "SP":
                    builder.Services.AddScoped<IProductRepository, ProductSPRepository>();
                    builder.Services.AddScoped<IProductCategoryRepository, ProductCategorySPRepository>();
                    break;
            }
            #endregion

            #region 註冊Service
            builder.Services.AddScoped<IProductService, ProductService>();
            #endregion

            #region Dapper自定義欄位對應
            TypeMapper.Initialize("OracleTest.Models.Response");
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
