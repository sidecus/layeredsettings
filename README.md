# layeredsettings
.Net Core lib which supports **Layered Environment Inheritance** when loading settings.

.Net Core default host builder introduces the capability of adding environment based settings (```appsettings.Development.json``` for exmaple). It also loads ```appsettings.json``` first before the environment specific settings which can save us time from repeating the same setting in different setting files.
However, sometimes this is still not good enough.

Assume I have this environment setup:
1. A Production environment which overrides ```appsettings.json``` with production specific settings
1. A dummy data backed Pre Production environment (PPE) deployed to Azure which overrides ```appsettings.json``` with PPE specific settings
1. Local Development environment which uses most of the PPE settings with several special local dev environment specific settings

In this case, I'll have to repeat a lot of settings from #1 and #2 in both ```appsettings.PPE.json``` and ```appsettings.Development.json```. This can cause maintenance issues and in the worse case scenarios it ends up with some settings out of sync and it's hard to trouble shoot.

This library enables **Layered Environment Inheritance** when loading settings. You can define your environments with inheritance hierarchy, and tell the host builder to inherit settings based on the hierarchy - or in other words, you build your environment specific settings like buliding docker images.

This solves the issue of having to repeat the same settings among similar environments. It also has the potential benefit to help you shift settings/variables from your dev ops pipeline into the settings file so that you can have better history tracking in the same way as your dev code.

Check [AppEnvironment.cs](https://github.com/sidecus/layeredsettings/blob/master/sample/AppEnvironments.cs) to see how layered settings environments can be defined, and [Program.cs](https://github.com/sidecus/layeredsettings/blob/master/sample/Program.cs) about how it's being used from the sample folder.
