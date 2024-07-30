using Discord;
using Discord.WebSocket;
using System.Threading.Channels;
using System.Windows.Forms.Design;
using Windows.Graphics.Printing.PrintSupport;

namespace WoonFrieslandUpdate
{
    public class DiscordBot
    {
        public DiscordSocketClient client = new DiscordSocketClient();
        public string bot;
        public ulong Channel_ID;


        public async Task Initialize()
        {
            //Close any previous clients. 
            await client.StopAsync();
            //Star config.
            var config = new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };
            client.Ready += () =>
            {
                Console.WriteLine("Bot is connected and ready.");
                return Task.CompletedTask;
            };

            await client.LoginAsync(TokenType.Bot, bot);
            await client.StartAsync();
            
        }

        public async void SendMessageToChannel(string message)
        {
            try
            {
                var channel = await client.GetChannelAsync(Channel_ID) as IMessageChannel;
                await channel!.SendMessageAsync(message);
            }
            catch (Exception ex) 
            {
            }
            
        }

        public async Task SendEmbed(string URL)
        {
            URL = "https://www.woonfriesland.nl/wis/file/image%7C314358.jpg";
            var channel = await client.GetChannelAsync(Channel_ID) as ITextChannel;
            var messages = await channel.GetMessagesAsync().FlattenAsync();
            var embed = new EmbedBuilder
            {
                // Embed property can be set within object initializer
                Title = "Hello world!",
                Description = "I am a description set by initializer.",
                ImageUrl = URL
            }.Build();
            await channel.SendFileAsync(URL, embed: embed);
        }



        public async Task ClearChannel()
        {
            
            var channel = await client.GetChannelAsync(Channel_ID) as ITextChannel;
            var messages = await channel.GetMessagesAsync().FlattenAsync();
            SendMessageToChannel($"Deleting: {messages.Count()} messages");
            await channel.DeleteMessagesAsync(messages);
        }



    }
}