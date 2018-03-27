using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace motBot2.Modules
{
    public class HelpModule:ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task GetHelp()
        {
            await ReplyAsync("", false, Services.HelpService.Helpper());
        }
    }
}
