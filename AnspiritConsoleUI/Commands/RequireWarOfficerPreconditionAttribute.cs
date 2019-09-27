using AnspiritConsoleUI.Constants;
using AnspiritConsoleUI.Services.Database;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AnspiritConsoleUI.Commands
{
    public class RequireWarOfficerPreconditionAttribute : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            // Check if this user is a Guild User, which is the only context where roles exist
            if (context.User is SocketUser user)
            {
                var dbService = services.GetService(typeof(AnspiritDatabaseService)) as AnspiritDatabaseService;

                // If this command was executed by a user with the appropriate role, return a success
                if (dbService.GetWarOfficers().Any(x => x.DiscordId == user.Id))
                {
                    return Task.FromResult(PreconditionResult.FromSuccess());
                }
                else
                {
                    return Task.FromResult(PreconditionResult.FromError($"You must be an Anzac Spirit war officer to use this command"));
                }
            }
            else
            {
                return Task.FromResult(PreconditionResult.FromError("You must be a user"));
            }
        }
    }
}
