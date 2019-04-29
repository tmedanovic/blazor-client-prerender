# BlazorClientPrerender

This example demonstrates one of the ways to achive client side blazor prerendering (sometimes known as Server side rendering).
Prerendring is very important for public SPA applications because of it's impact on SEO and initial page load time.

### Project structure description

* **BlazorClientPrerender.Shared** - Shared DTO definitions and service interfaces for server side and client side logic
* **BlazorClientPrerender.Service** - Server side service implementations - used by server side blazor and our REST API
* **BlazorClientPrerender.Api** - Our REST API that our client side blazor project will talk to
* **BlazorClientPrerender.Client** - Client side blazor project
* **BlazorClientPrerender.Startup** - Blazor server side rendering project and the one we will expose to the public

### Running sample application:

* First we have to start our REST API (https://localhost:7001/) 
* Serve our client side files, in this example we are using kestrel but it could be easily be changed to nginx or similar (http://localhost:5000/)
* Run our startup project that will serve the web application (https://localhost:6001/) 

#### Cmd:

    cd BlazorClientPrerender.Api
    dotnet run
    
    cd BlazorClientPrerender.Client
    dotnet run
    
    cd BlazorClientPrerender.Startup
    dotnet run

Open web browser at https://localhost:6001/

### How it works

Our server side blazor project (BlazorClientPrerender.Startup) is rendering component described 
our client side blazor project on initial load and serves it from _Host.cshtml:

    <body>
        <app>@(await Html.RenderComponentAsync<BlazorClientPrerender.Client.App>())</app>
    
        <!-- We replaced blazor.server.js with blazor.webassembly.js because we are going to use client side after initial render -->
        <script src="_framework/blazor.webassembly.js"></script>
        <!-- <script src="_framework/blazor.server.js"></script> -->
    </body>

After the initial load blazor.webassembly.js will try to load mono.wasam and .dll files that run on mono and replace contents of our <app></app>,
to serve client-side dependencies properly we are proxying every request that comes to /_framework path to http://localhost:5000/ 
which exposes client side dependencies:

    // Route _framework requests to client side generated .dlls 
    // ( Our client side blazor is running on localhost:5000 )
    app.MapWhen((context) => context.Request.Path.Value.StartsWith("/_framework"), 
    	builder => builder.RunProxy(new ProxyOptions()
    {
    	Scheme = "http",
    	Host = new HostString("localhost", 5000)
    }));

### Rendering the data:

In this example we are injecting different implementations of IWeatherForecastService depending on project:

In our REST API and in our server side rendering Startup.cs we are registering server side implementation that can access server resources directly (ie. EF DbContext)

    public void ConfigureServices(IServiceCollection services)
    {
    	...
    	services.AddScoped<IWeatherForecastService, WeatherForecastService>();
    }

And in our client side we are injecting implementation of service that is using HttpClient to talk to our REST API and access data:

    public void ConfigureServices(IServiceCollection services)
    {
    	services.AddSingleton<IWeatherForecastService, WeatherForecastClientService>();
    }

