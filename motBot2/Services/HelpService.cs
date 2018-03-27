using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace motBot2.Services
{
    class HelpService
    {
        public static Embed Helpper()
        {
            Dictionary<string, string> cmds;
            string json = File.ReadAllText("Configs/cmdList.json");
            var data = JsonConvert.DeserializeObject<dynamic>(json);
            cmds = data.ToObject<Dictionary<string, string>>();
            string stringCmds = null;
            foreach (KeyValuePair<string, string> pair in cmds)
            {
                string tmp = pair.Key + " : " + pair.Value + "\n";
                stringCmds = stringCmds + tmp;
            }

            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle("Commands")
                .WithDescription(stringCmds)
                .WithColor(Color.Blue)
                .WithFooter("Commands are not case sensitive");
            Embed cmp = builder.Build();
            return cmp;
        }
    }
}
