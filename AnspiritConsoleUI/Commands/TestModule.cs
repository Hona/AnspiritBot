using Discord.Commands;
using System.Threading.Tasks;

namespace AnspiritConsoleUI.Commands
{
    public class TestModule : AnspiritModuleBase
    {
        [Command("test")]
        public Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
        {
            
            return ReplyAsync(echo);
        }
    }
}
