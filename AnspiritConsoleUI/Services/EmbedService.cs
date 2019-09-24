using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnspiritConsoleUI.Services
{
    public static class EmbedService
    {
        public static Embed CreateEmbed(string text, Color color)
            => new EmbedBuilder
            {
                Description = text,
                Color = color
            }.Build();
    }
}
