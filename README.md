ProceXss
========

ProceXSS is an Asp.NET Http module to prevent to xss attacks.

[Nuget Package](http://nuget.org/packages/ProceXSS) ```Install-Package ProceXSS```

##Basic usage##
Add following line below the node <configSections> in web.config file

```xml
<section name="antiXssModuleSettings" type="ProceXSS.Configuration.XssConfigurationHandler, ProceXSS"/>
```

and Add the following configurations below the node <configuration>,

```xml
<antiXssModuleSettings redirectUrl="/home" log="False" mode="Ignore" isActive="True"
controlRegex="(javascript[^*(%3a)]*(\%3a|\:))
|(\%3C*|\&lt;)[\/]*script|(document[\.])
|(window[^a-zA-Z_0-9]*[\%2e|\.])|
(setInterval[^a-zA-Z_0-9]*(\%28|\())
|(setTimeout[^a-zA-Z_0-9]*(\%28|\())|(alert[^a-zA-Z_0-9]*(\%28|\())|
eval[^a-zA-Z_0-9]*(\%28|\()|(((\%3C) &lt;)[^\n]+((\%3E) &gt;))">
    <excludeUrls>
      <add name="url1" value="/"/>
      <add name="url2" value="/default.aspx"/>
    </excludeUrls>
</antiXssModuleSettings>
```

There is a two option for **mode** property. These are **Ignore** and **Redirect**. When the redirect mode is active system will redirect the request to value of **RedirectUri**.

Nuget package creates **XSSConfig.cs** to **App_Start** folder to register module dynamically.
```csharp
[assembly: PreApplicationStartMethod(typeof(XSSConfig), "Start")]
namespace AcmeWeb.WebForms
{
    public class XSSConfig
    {
        public static void Start()
        {
            ProceXSSModule.SetLogger(new MyLogger()); //Register your ILogger implementation.
            Microsoft.Web.Infrastructure
                         .DynamicModuleHelper
                         .DynamicModuleUtility.RegisterModule(typeof(ProceXSSModule));
        }
    }
}
```
Or add the following configurations below <system.web> <httpModules>
```xml
<add name="ProceXSSModule" type="ProceXSS.ProceXSSModule, ProceXSS, Version=your assembly version, Culture=neutral" />
```


For more detailed information about XSS visit [owasp web site](https://www.owasp.org/index.php/XSS)
