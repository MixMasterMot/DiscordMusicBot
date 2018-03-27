using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace motBot2.Modules
{
    public class HangmanModule : ModuleBase<SocketCommandContext>
    {
        [Command("hang")]
        public async Task HangManStart(string ltr = "new")
        {
            if (ltr == "new")
            {
                await ReplyAsync("Hang the hangman", false, Services.HangmanService.NewGame());
                return;
            }
            else if (ltr == "hint")
            {
                Tuple<string, Embed> tpl = Services.HangmanService.Hint();
                if (tpl.Item1 != "")
                {
                    await ReplyAsync(tpl.Item1);
                }
                else
                {
                    await ReplyAsync("", false, tpl.Item2);
                }
            }
            else if (ltr.Length > 1)
            {
                await ReplyAsync("Only one leter at a time");
            }
            else
            {
                Tuple<string, Embed> tpl = Services.HangmanService.HangManWordContains(ltr);
                if (tpl.Item1 != "")
                {
                    await ReplyAsync(tpl.Item1);
                }
                else
                {
                    await ReplyAsync("", false, tpl.Item2);
                }
            }
        }
    }
}
