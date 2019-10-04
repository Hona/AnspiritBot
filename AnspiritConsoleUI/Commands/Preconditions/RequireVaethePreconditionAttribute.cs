using AnspiritConsoleUI.Constants;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace AnspiritConsoleUI.Commands.Preconditions
{
    public class RequireVaethePreconditionAttribute : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            // Check if this user is a Guild User, which is the only context where roles exist
            if (context.User is SocketUser user)
            {
                if (user.Id == DiscordConstants.VaetheDiscordID)
                {
                    return Task.FromResult(PreconditionResult.FromSuccess());
                }
                else
                {
                    return Task.FromResult(PreconditionResult.FromError($"You must be Vaethe to use this command...!"));
                }
            }
            else
            {
                return Task.FromResult(PreconditionResult.FromError("You must be a user"));
            }
        }
    }
}
