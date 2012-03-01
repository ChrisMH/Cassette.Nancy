#Cassette.Nancy
Cassette integration into the Nancy web framework

#Installation

NOTE: Dependency on Cassette.Views has been removed.  If upgrading an existing installation, you can remove that Nuget package.

Cassette on Nancy currently works with the Razor View Engine.

1. Create an empty web project

2. Install the Cassette.Nancy Nuget package (this references Cassette, Nancy and Nancy.ViewEngines.Razor, so those will be installed, too)

4. Install the  Nancy.Hosting.AspNet packages

5. Modify the CassetteConfiguration (see http://getcassette.net/).  A default configuration should be added by the Nuget package.

6. The web.config file should have been modified to include the Cassette & Cassette.Nancy namespaces, looking like:

  <pre>&lt;configSections&gt;
      &lt;section name="razor" type="Nancy.ViewEngines.Razor.RazorConfigurationSection, Nancy.ViewEngines.Razor"/&gt;
    &lt;/configSections&gt;
    &lt;razor disableAutoIncludeModelNamespace="true"&gt;
      &lt;assemblies&gt;
        &lt;add assembly="Cassette"/&gt;
        &lt;add assembly="Cassette.Nancy"/&gt;
      &lt;/assemblies&gt;
      &lt;namespaces&gt;
        &lt;add namespace="Cassette.Nancy"/&gt;
      &lt;/namespaces&gt;
    &lt;/razor&gt;</pre>
    
  Alternatively, you could implement the IRazorConfiguration interface and configure appropriately there.
  
7. Start including and rendering modules in your Razor views (see: http://getcassette.net/).  It works the same as is ASP.NET MVC3 Razor views, although you may need to @include Cassette.Nancy and at the top of your .cshtml to keep the preprocessor happy.

#Customization

By default, the output from Cassette is not optimized.  When output is optimized, Cassette modules are combined (bundled) based on the configuration and sent to the client as a single lump per bundle instead of lots of individual files.

To turn optimized output on, set the CassetteStartup.ShouldOptimizeOutput attribute to true.  I'd suggest doing this in the constructor of a custom Nancy bootstrapper to ensure that it is set before it is used:

  <pre>public class Bootstrapper : DefaultNancyBootstrapper
  {
    public Bootstrapper()
    {
      CassetteStartup.ShouldOptimizeOutput = true;
    }
  } 
  </pre>

TODO: This should be a configuration setting, but I'm not sure how I want to do that yet.

#Testing

If you use Nancy's testing infrastructure, Cassette will not be able to find your assets because the unit test's project root will be different than your project's root.  Luckily, Nancy uses a pluggable object that serves up a root path.  In your test project, implement a bootstrapper, derived from your web application's bootstrapper if it has one, like so:

  public class TestNancyBootstrapper : Client.Web.NancyBootstrapper 
  {
    public TestNancyBootstrapper()
    {
      // Set the root path for scripts, styles, views, etc. 
      // FakeRootPathProvider is the IRootPathProvider Nancy uses when it is running in test mode
      FakeRootPathProvider.RootPath = Utility.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Client.Web");
    }
  }
  
  Use this bootstrapper in calls to Nancy.Testing's Browser class:
  
  var browser = new Browser(new TestNancyBootstrapper());
  
  In the above example I'm using a utility class that combines and canonicalize the path, but you can set it however you like.
  
  