using Microsoft.Extensions.DependencyInjection;
using OP_Shared_Library.Notify;
using OP_Shared_Library.Notify.Interface;
using OP_Shared_Library.Services;

namespace OP_Shared_Library;
public class Installer
{
    public void Install(IServiceCollection services)
    {
        if (services is null) return;

        services.AddSingleton<INotifier, Notifier>();
        services.AddScoped<TranslationsService>();
        services.AddSingleton<EditModeService>();
    }
}
