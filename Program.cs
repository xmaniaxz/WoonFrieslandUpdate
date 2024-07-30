using System.Diagnostics;
using System.Windows;

namespace WoonFrieslandUpdate
{
    public class Program
    {
        public static DiscordBot Dbot = new DiscordBot();
        public static ScriptInfo SInfo = new ScriptInfo();
        private static void Main(string[] args)
        {
            Console.CancelKeyPress += OnCancelKeyPress;
            UpdateChecker updateChecker = new UpdateChecker();
            string webURL = "https://www.woonfriesland.nl/wis/publications";
            Debug.WriteLine("Trying to get webdata");
            Dbot.Initialize().GetAwaiter().GetResult();
            //Dbot.SendEmbed("").GetAwaiter().GetResult() ;
            updateChecker.Initialize(webURL, Dbot);
        }


        static void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Closing Bot");
            Dbot.ClearChannel().GetAwaiter().GetResult();
            Dbot.client.StopAsync();
            Dbot.client.LogoutAsync();
            Console.CancelKeyPress -= OnCancelKeyPress;
        }
    }
}

