using AnspiritConsoleUI.Services;
using Discord.Commands;

namespace AnspiritConsoleUI.Commands
{
    public abstract class AnspiritModuleBase : ModuleBase<SocketCommandContext>
    {
        public LogService Logger { get; set; }
    }
}
