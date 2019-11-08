Imports System.Reflection
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Hosting
Imports Microsoft.AspNetCore.Mvc.Controllers
Imports Microsoft.Extensions.Configuration
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Logging

Public Class Startup
    Public Sub New(env As IHostingEnvironment)
        Dim builder = (New ConfigurationBuilder()).SetBasePath(env.ContentRootPath).
            AddJsonFile("appsettings.json", optional:=False, reloadOnChange:=True).
            AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional:=True).AddEnvironmentVariables()
        Configuration = builder.Build()
    End Sub

    Public ReadOnly Property Configuration() As IConfigurationRoot

    ' This method gets called by the runtime. Use this method to add services to the container.
    Public Sub ConfigureServices(services As IServiceCollection)
        ' Add framework services.
        services.AddMvc() _
                .ConfigureApplicationPartManager(Sub(apm)
                                                     apm.FeatureProviders.Add(New CustomControllerFeatureProvider)
                                                 End Sub)


    End Sub

    Class CustomControllerFeatureProvider
        Inherits ControllerFeatureProvider

        Protected Overrides Function IsController(typeInfo As TypeInfo) As Boolean
            Return MyBase.IsController(typeInfo) OrElse GetType(Controller).IsAssignableFrom(typeInfo) AndAlso Not typeInfo.IsAbstract
        End Function
    End Class

    ' This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    Public Sub Configure(app As IApplicationBuilder, env As IHostingEnvironment, loggerFactory As ILoggerFactory)

        app.UseMvc()

    End Sub
End Class
