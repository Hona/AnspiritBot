using Discord;

namespace AnspiritConsoleUI.Utilities
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
