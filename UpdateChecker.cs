namespace WoonFrieslandUpdate
{
    public class UpdateChecker
    {
        public string CurrentHash = "";
        public string PreviousHash = "";
        public DiscordBot Dbot;
        public ScriptInfo SInfo = Program.SInfo;
        public void Initialize(string URL, DiscordBot _Dbot)
        {
            if (Dbot == null)
            {
                Dbot = _Dbot;
                Dbot.Initialize().GetAwaiter().GetResult();
                Dbot.SendMessageToChannel("Bot is Connected");
            }
            CheckForUpdate(URL).GetAwaiter().GetResult();
            TimeSpan timer = new TimeSpan(0, 10, 0);
            Thread.Sleep(timer);
            Initialize(URL);
        }

        public void Initialize(string URL)
        {
            Dbot.SendMessageToChannel("Checking for new updates");
            CheckForUpdate(URL).GetAwaiter().GetResult();
            TimeSpan timer = new TimeSpan(0, 10, 0);
            Thread.Sleep(timer);
            Initialize(URL);
        }

        public async Task CheckForUpdate(string URL)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(URL);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Dbot.SendMessageToChannel($"{responseBody}");

                CurrentHash = CreateMD5(responseBody);

                PreviousHash = CurrentHash;
                SInfo.GetData(responseBody);
                if (string.IsNullOrEmpty(PreviousHash))
                {
                    PreviousHash = CreateMD5(responseBody);
                }
                Dbot.SendMessageToChannel($"Found {SInfo.containers.Count} places");
                //Send messages to bot.
                for (int i = 0; i < SInfo.containers.Count; i++)
                {
                    string message = $"{SInfo.containers[i].Address} in {SInfo.containers[i].City} \nPrice:{SInfo.containers[i].Price}";
                    Dbot.SendMessageToChannel(message);
                }
            }
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes); // .NET 5 +
            }
        }
    }
}