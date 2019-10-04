using AnspiritConsoleUI.Services.Database;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AnspiritConsoleUI.Commands.Preconditions
{
    public class RequireAnspiritDiscordOfficerPreconditionAttribute : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            // Check if this user is a Guild User, which is the only context where roles exist
            if (context.User is SocketGuildUser user)
            {
                // If this command was executed by a user with a role containing 'officer', return a success
                if (user.Roles.Any(x => x.Name.ToLower().Contains("officer")))
                {
                    return Task.FromResult(PreconditionResult.FromSuccess());
                }
                else
                {
                    return Task.FromResult(PreconditionResult.FromError($"You must be an Anzac Discord officer to use this command"));
                }
            }
            else
            {
                return Task.FromResult(PreconditionResult.FromError("You must be a user"));
            }
        }
    }
}
