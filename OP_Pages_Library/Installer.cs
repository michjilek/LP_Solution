using Microsoft.Extensions.DependencyInjection;
using OP_Pages_Library.ViewModel;
using OP_Pages_Library.ViewModel.Interface;

namespace OP_Pages_Library;
public class Installer
{
    public void Install(IServiceCollection services)
    {
        if (services is null) return;

        //services.AddScoped<IMV_OP_Template_Page, MV_OP_Template_Page>();
    }
}
