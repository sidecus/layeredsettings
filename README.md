# layeredsettings
.net core lib which supports layered json settings (e.g. a Azure hosted testing settings named PPE, and then a dev setting inherited from it with overrides)

.Net Core default host builder introduces the capability of adding environment based settings (```appsettings.Development.json``` for exmaple). It also loads ```appsettings.json``` first before the environment specific settings which can save us time from repeating the same setting in different setting files.

However, sometimes this is not enough in real world.

Assume I have this environment setup:
1. A Production environment which overrides ```appsettings.json``` with production specific settings
1. A dummy data backed Pre Production environment (PPE) deployed to Azure which overrides ```appsettings.json``` with PPE specific settings
1. Local Development environment which uses most of the PPE settings with several special local dev environment specific settings

In this case, I'll have to repeat the needed PPE settings in ```appsettings.Development.json```. And this can cause maintenance issues and even worse out of sync settings.

This library enables **Environment Inheritance**. You can define your environments with inheritance hierarchy, and tell the host builder to inherit settings based on the hierarchy.
