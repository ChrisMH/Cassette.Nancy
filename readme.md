#Nancy.Cassette
Cassette integration into the Nancy web framework

Note: This package currently requires a custom build of Cassette, which is installed automatically via the Nancy.Cassette NuGet package.

#Installation

Cassette currently works with the Razor View Engine.

1. Create an empty web project
2. Install the Nancy.Cassette NuGet package
3. Install the Nancy.ViewEngines.Razor and Nancy.Hosting.AspNet packages
4. Modify the CassetteConfiguration (see http://http://getcassette.net/).  An empty configuration is added by the NuGet package.
5. Add configuration to pull the Cassette namespace into the Razor view engine:
  <pre>&lt;configSections&gt;
      &lt;section name="razor" type="Nancy.ViewEngines.Razor.RazorConfigurationSection, Nancy.ViewEngines.Razor"/&gt;
    &lt;/configSections&gt;
    &lt;razor disableAutoIncludeModelNamespace="true"&gt;
      &lt;assemblies&gt;
        &lt;add assembly="Cassette"/&gt;
      &lt;/assemblies&gt;
      &lt;namespaces&gt;
        &lt;add namespace="Cassette.UI"/&gt;
      &lt;/namespaces&gt;
    &lt;/razor&gt;</pre>
6. Start including and rending modules in your Razor views.  Once again, see: http://http://getcassette.net/

#Customization

