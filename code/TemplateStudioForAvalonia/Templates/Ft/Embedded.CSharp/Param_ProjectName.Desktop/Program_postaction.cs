//^^
//{[{
using System.Linq;
using System.Threading;
//}]}

namespace Param_RootNamespace.Desktop;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    //^^
    //{[{
    public static int Main(string[] args)
    {
        var builder = BuildAvaloniaApp();
        if(args.Contains("--drm"))
        {
            SilenceConsole();
                
            // If Card0, Card1 and Card2 all don't work. You can also try:                 
            // return builder.StartLinuxFbDev(args);
            // return builder.StartLinuxDrm(args, "/dev/dri/card1");
            return builder.StartLinuxDrm(args, "/dev/dri/card1", 1D);
        }

        return builder.StartWithClassicDesktopLifetime(args);
    }
    //}]}

    //{--{
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);
    //}--}

    //^^
    //{[{
    private static void SilenceConsole()
    {
        new Thread(() =>
            {
                Console.CursorVisible = false;
                while(true)
                    Console.ReadKey(true);
            })
            { IsBackground = true }.Start();
    }
    //}]}
}
