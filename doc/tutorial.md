# SignalR with Angular 7 and ASP.NET MVC 5

A few months ago I got a task of implementing a new feature in the ASP.NET MVC project.
For the record, it was a pretty old project, so as you can guess, it was not based on
.NET Core. Anyway, the feature required displaying real-time notifications on a web page,
so SignalR was my choice.

As it's already mentioned, the server side was built with ASP.NET MVC (.NET Framework 4.5.2).
The front-end part was upgraded recently and Angular 7 has been harnessed to do the job. To sum up,
the goal was to send a real-time notification from ASP.NET MVC server to the Angular application.
Easy, right? Well, it turned out to be easy, finally, but it wasn't easy for me at the beginning.

I didn't know Angular very well and the last version I had been using was AngularJS, so I had
to do some research, first. It turend out that the vast majority of articles focus on the
combining SignalR with Angular and ASP.NET Core and I had trouble finding the complete
example with ASP.NET MVC, so I decided to create my own _proof of concept_ and share it
on GitHub: [AspMvcAngularSignalR](https://github.com/mieczyk/AspMvcAngularSignalR).

## Step-by-step guide

Without the further ado, the guide assumes that the project's scaffolding (ASP.NET MVC 5 + Angular 7) 
is already set up. If not, please read one of the following articles:

* [Build a Basic Website with ASP.NET MVC and Angular](https://developer.okta.com/blog/2018/12/21/build-basic-web-app-with-mvc-angular)
* [Using Angular 8 in ASP.NET MVC 5 with Angular CLI and Visual Studio 2017](https://www.mithunvp.com/angular-asp-net-mvc-5-angular-cli-visual-studio-2017/)

**The tutorial assumes that the example solution is built on .NET Framework 4.6.1, but 
it should work with the higher versions as well.**

### 1. Install Microsoft.AspNet.SignalR NuGet package

Once the scaffolding is ready, install the **Microsof.AspNet.SignalR** NuGet package, 
in the ASP.NET MVC project (_ChatWebClient_ in this example):

```
Install-Package Microsoft.AspNet.SignalR -ProjectName ChatWebClient
```

The command is meant to be executed from the _Package Manager Console_, but you
can use the visual packages manager, built into Visual Studio 
(_Tools -> NuGet Package Manager -> Manager NuGet Packages for Solution..._).

The following packages have been installed (see the project's _packages.config_ file):

```
<package id="Microsoft.AspNet.SignalR" version="2.4.1" targetFramework="net461" />
<package id="Microsoft.AspNet.SignalR.Core" version="2.4.1" targetFramework="net461" />
<package id="Microsoft.AspNet.SignalR.JS" version="2.4.1" targetFramework="net461" />
<package id="Microsoft.AspNet.SignalR.SystemWeb" version="2.4.1" targetFramework="net461" />
<package id="Microsoft.Owin" version="2.1.0" targetFramework="net461" />
<package id="Microsoft.Owin.Host.SystemWeb" version="2.1.0" targetFramework="net461" />
<package id="Microsoft.Owin.Security" version="2.1.0" targetFramework="net461" />
<package id="Newtonsoft.Json" version="6.0.4" targetFramework="net461" />
<package id="Owin" version="1.0" targetFramework="net461" />
```

As you can see, apart from the SignalR-related packages, OWIN (Open Web Interface for .NET) packages 
have been installed as well. That's because we can (and we will) use OWIN for SignalR self-hosting. 
Don't worry, it can be freely mixed with ASP.NET hosted application. It's possible to use
SignalR with ASP.NET hosting (without OWIN), but I've never tried.

The required _Newtonsoft.Json_ package, that came in the bundle, is very old, so you'll
probably want to upgrade its version if you need it.

There are two more items that have been added to the project:

* `jquery.signalR-2.4.1.js`
* `jquery.signalR-2.4.1.min.js`

They are very important for the SignalR communication, but we'll get back to it
in a minute.



1) SignalR 2 NuGet package
1) ng2-signalR

References:

* SignalR home page
* https://stackoverflow.com/questions/26273700/signalr-without-owin