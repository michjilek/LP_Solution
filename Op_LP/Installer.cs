//using OP_Blazor_Client.Repository.Base;
//using OP_Blazor_Client.Facades.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OP_Pages_Library.ViewModel;
using OP_Pages_Library.ViewModel.Interface;
using OP_Shared_Library.Notify;
using OP_Shared_Library.Notify.Interface;

namespace Op_LP;
public class Installer
{
    public void Install(WebApplicationBuilder builder)
    {
        // Add LocalDb Service
        //builder.Services.AddSingleton<LocalDb>();

        // SHARED [
        // Add MVs
        //builder.Services.AddScoped<IMV_OP_Template_Page, MV_OP_Template_Page>();

        // Add Notifier
        //builder.Services.AddSingleton<INotifier, Notifier>();
        // ]

        // Register all under IAppFacade
        //builder.Services.Scan(selector =>
        //    selector.FromAssemblyOf<Installer>()
        //        .AddClasses(classes => classes.AssignableTo<IAppFacade>())
        //        .AsSelf() // .AsSelfWithInterfaces()
        //        .WithTransientLifetime());

        //builder.Services.AddScoped(sp => new HttpClient
        //{
        //    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
        //});
    }
}
