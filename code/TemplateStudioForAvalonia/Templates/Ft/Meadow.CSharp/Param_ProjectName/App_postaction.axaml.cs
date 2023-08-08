using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Param_RootNamespace.ViewModels;
using Param_RootNamespace.Views;
//^^
//{[{
using Meadow;
using Meadow.Pinouts;
using System.Threading.Tasks;
//}]}

namespace Param_RootNamespace;

//{--{
public partial class App : Application
{
//}--}

//^^
//{[{
public partial class App : AvaloniaMeadowApplication<Linux<RaspberryPi>>
{
//}]}

//^^
//{[{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        LoadMeadowOS();
    }

    public override Task InitializeMeadow()
    {
        var r = Resolver.Services.Get<IMeadowDevice>();

        if (r == null)
        {
            Resolver.Log.Info("IMeadowDevice is null");
        }
        else
        {
            Resolver.Log.Info($"IMeadowDevice is {r.GetType().Name}");
        }

        return Task.CompletedTask;
//}]}

//{--{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

//}--}
