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

### 1) Install Microsoft.AspNet.SignalR NuGet package

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

### 2) Set up the SingalR Hub

What is the *Hub*? Let me quote the official 
[SignalR Hubs API Guide](https://docs.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/hubs-api-guide-server):

---
The SignalR Hubs API enables you to make remote procedure calls (RPCs) from a server to connected clients and from 
clients to the server. In server code, you define methods that can be called by clients, and you call methods that 
run on the client. In client code, you define methods that can be called from the server, and you call methods that 
run on the server.

---

Let's add one, named `MessagesHub`, to the _ChatWebClient_ project:

```
using Microsoft.AspNet.SignalR;

namespace ChatWebClient
{
    public class MessagesHub : Hub
    {
        private readonly IHubContext _context;

        public MessagesHub()
        {
            _context = GlobalHost.ConnectionManager.GetHubContext<MessagesHub>();
        }

        public void Notify(string message)
        {
            _context.Clients.All.onMessagesReceived(message);
        }
    }
}
```

The class provided above contains the minimum necessary to set up the SignalR hub
and establish the real-time communication between the client and server. 

The `Notify(string message)` method's job is to notify all clients that are currently 
connected to the hub about a message. Please, note the `onMessagesReceived(messages)` 
method is called on the `dynamic` object. Actually, you can give the method any name 
you want, as the mentioned call executes the function, with the exact same name, 
on the client side. We'll get back to it soon.

Now, it's time to start the hub with OWIN. Let's define the `Startup` class within 
the ASP.NET MVC projcet, in the `Startup.cs` source file:

```
using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(ChatWebClient.Startup))]

namespace ChatWebClient
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
```

The `app.MapSignalR()` call defines the route that clients will use to connect to the hub.
That's it. We have the SignalR hub set up and now it's time to use it.

### 3) Send a message from the server

Assuming that the ASP.NET MVC routing is already confifured in the `App_Start/RouteConfig.cs`
file, for example:

```
using System.Web.Mvc;
using System.Web.Routing;

namespace ChatWebClient
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Chat", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
```

we can add a controller action that dispatches a message through the SignalR hub:

```
using System.Net;
using System.Web.Mvc;

namespace ChatWebClient.Controllers
{
    public class ChatController : Controller
    {
        private readonly MessagesHub _hub;

        public ChatController()
        {
            _hub = new MessagesHub();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Send(string message)
        {
            if(!string.IsNullOrWhiteSpace(message))
            {
                _hub.Notify(message);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
```

Now, we can POST any message to `http://<HOST_URL>/Chat/Send` and the message will be dispatched
to all SignalR clients that are currently connected. However, please note that we have no
client connected, yet. So, even if we call the controller's action, nothing happens. It's
time to add the SignalR client.

### 4) Install ng2-signalr NPM package in Angular application

To install the [ng2-signalr](https://github.com/HNeukermans/ng2-signalr) NPM package, append the dependency: 

```
"ng2-signalr": "~8.0.2"
```

to the `package.json` file and run the `npm install` command within the directory, the `package.json`
file resides. 

You may wonder why we didn't use the `@aspnet/signalr` package. Well, that's because this package
is meant for communicating with ASP.NET Core and we use ASP.NET MVC on the server side.

You may also get the follow warnings, while installing NPM packages:

```
ng2-signalr@8.0.2 requires a peer of @angular/common@^8.0.3 but none is installed. You must install peer dependencies yourself.
npm WARN ng2-signalr@8.0.2 requires a peer of @angular/core@^8.0.3 but none is installed. You must install peer dependencies yourself.
```

but don't worry. The communication will work anyway. However, if it bothers you, 
downgrade the `ng2-signalr` package version or use Angular 8.

The next step is to add the configuration inside the `app.module.ts` file:

```
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { SignalRModule, SignalRConfiguration } from 'ng2-signalr';

import { ChatWindow } from './chat-window.component';

export function createSignalRConfig(): SignalRConfiguration {
    const config = new SignalRConfiguration();
    config.hubName = 'messagesHub';
    return config;
}

@NgModule({
  declarations: [
    ChatWindow
  ],
  imports: [
      BrowserModule,
      SignalRModule.forRoot(createSignalRConfig)
  ],
  providers: [],
  bootstrap: [ChatWindow]
})

export class AppModule { }
```

First, import the necessary classes from the `ng2-signalr` module:

```
import { SignalRModule, SignalRConfiguration } from 'ng2-signalr';
```

Second, define the function that creates and returns the most basic 
SignalR configuration possible:

```
export function createSignalRConfig(): SignalRConfiguration {
    const config = new SignalRConfiguration();
    config.hubName = 'messagesHub';
    return config;
}

```

Please note that we need to set the `hubName` property to the name we
used while creating the SignalR hub on the server side, but using the
_camelCase_ name this time.

Finally, import the `SignalRModule` with the custom configuration, within
the `@NgModule` decorator:

```
@NgModule({
  imports: [
    SignalRModule.forRoot(createSignalRConfig)
  ]
})

```

If you'd like to know more about the `SignalRModule` configuration, please visit:
https://github.com/HNeukermans/ng2-signalr#setup.

There's one more thing left. Do you remember the jQuery files that were added
to the project while installing the SignalR NuGet package? It's time to include
one of them in the page's scripts section. For example, directly in the `_Layout.cshtml`
template:

```
<script src="~/Scripts/jquery.signalR-2.4.1.min.js"></script>
```

This is the minimized version, but you may want include the full version 
in order to make the debugging process easier. 

References:

* SignalR home page
* https://stackoverflow.com/questions/26273700/signalr-without-owin
* https://docs.microsoft.com/en-us/aspnet/signalr/overview/getting-started/tutorial-getting-started-with-signalr-and-mvc
* https://github.com/aspnet/SignalR/blob/master/specs/HubProtocol.md
* https://www.quora.com/What-is-the-difference-between-web-sockets-and-signalR
