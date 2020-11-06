using System;

namespace WizardGrenade2
{
#if WINDOWS || LINUX

    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new WGGame())
                game.Run();
        }
    }
#endif
}
