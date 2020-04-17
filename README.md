# layeredsettings
.Net Core lib which enables **Layered Environment Settings** when loading config.
![.NET Core](https://github.com/sidecus/layeredsettings/workflows/.NET%20Core/badge.svg?branch=master)

## Background
.Net Core default host builder introduces the capability of adding [environment based settings](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1) for example ```appsettings.Development.json```. It also loads ```appsettings.json``` before the environment specific settings. This is a huge time saver for developers and avoids having to repeat  configuration values among different setting files.
However, sometimes this is still not sufficient.

Assume I have this environment setup:
1. A Production environment which overrides ```appsettings.json``` with production specific settings
1. A dummy data backed Pre Production environment (PPE) deployed to Azure which overrides ```appsettings.json``` with PPE specific settings
1. Local Development environment which uses most of the PPE settings with several special local dev environment specific settings

In this case, I'll have to repeat a lot of settings from #1 and #2 in both ```appsettings.PPE.json``` and ```appsettings.Development.json```. This can cause maintenance issues and in the worse case scenarios it ends up with some settings out of sync and it's hard to trouble shoot.

This library enables **Layered Environment Settings** when loading settings. You can define your environments with inheritance hierarchy, and tell the host builder to inherit settings based on the hierarchy - or in other words, you build your environment specific settings like buliding docker images. You get something like below:

![environment setup](https://github.com/sidecus/layeredsettings/blob/master/sample/wwwroot/env.png "Layered environment settings")
This solves the issue of having to repeat the same settings among similar environments. It also has the potential benefit to help you shift settings/variables from your dev ops pipeline into the settings file so that you can have better history tracking in the same way as your dev code.

Check [SampleEnvironments.cs](https://github.com/sidecus/layeredsettings/blob/master/sample/SampleEnvironments.cs) to see how layered settings environments can be defined, and [Program.cs](https://github.com/sidecus/layeredsettings/blob/master/sample/Program.cs) about how it's being used from the sample folder.

## how to run locally
Easiest way to mimic different environments is to use VSCode to mimic different environments. I have different configurations in ```launch.json``` to mimic each environment.

![Run locally](https://github.com/sidecus/layeredsettings/blob/master/sample/wwwroot/vscoderun.png "How to run locally")

## things to keep in mind
When you have multiple custom environments beside just Development and Production (by specifying the ```ASPNETCORE_ENVIRONMENT``` environment variable to custom values), ***DO NOT*** do things like below:
 ```
 if (!env.IsProduction)
 {
     // expose some development only features
 }
 ```
 ```IHostEnvironment.IsProduction``` won't be true unless ```ASPNETCORE_ENVIRONMENT``` is set to "Production". So in the example above, the PPE environment can also have the development only feature turned on even though it's deployed to Azure. This is not expected.
 You should always use an environment "white list" for the check not a "black list".
