using Discord;

namespace AnspiritConsoleUI.Services
{
    public static class EmbedUtilities
    {
        public static Embed CreateEmbed(string text, Color color)
            => new EmbedBuilder
            {
                Description = text,
                Color = color
            }.Build();
    }
}
