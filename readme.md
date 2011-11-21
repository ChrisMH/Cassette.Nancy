#Cassette
Cassette integration into the Nancy web framework

#Installation

Cassette on Nancy currently works with the Razor View Engine.

1. Create an empty web project
2. Install the Cassette.Nancy NuGet package
3. Install the Cassette.Web package
4. Install the Nancy.ViewEngines.Razor and Nancy.Hosting.AspNet packages
5. Modify the CassetteConfiguration (see http://http://getcassette.net/).  An empty configuration is added by Cassette's NuGet package.
6. Add configuration to pull the Cassette namespace into the Razor view engine:
  <pre>&lt;configSections&gt;
      &lt;section name="razor" type="Nancy.ViewEngines.Razor.RazorConfigurationSection, Nancy.ViewEngines.Razor"/&gt;
    &lt;/configSections&gt;
    &lt;razor disableAutoIncludeModelNamespace="true"&gt;
      &lt;assemblies&gt;
        &lt;add assembly="Cassette"/&gt;
        &lt;add assembly="Cassette.Views"/&gt;
      &lt;/assemblies&gt;
      &lt;namespaces&gt;
        &lt;add namespace="Cassette.Views"/&gt;
      &lt;/namespaces&gt;
    &lt;/razor&gt;</pre>
7. Start including and rending modules in your Razor views.  Once again, see: http://http://getcassette.net/.  It works the same as is ASP.NET MVC3 Razor views, although you may need to @include Cassette.Views and @include Nancy at the top of your .cshtml to keep the preprocessor happy.

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
