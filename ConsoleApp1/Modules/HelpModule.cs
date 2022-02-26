using Discord.Interactions;
using Discord.WebSocket;

namespace TheFiremind.Modules;

/// <summary>
/// Module facilitating a help command
/// </summary>
public class HelpModule : InteractionModuleBase<SocketInteractionContext<SocketSlashCommand>>
{
    /// <summary>
    /// Responds with directions on how to use the bot
    /// </summary>
    [SlashCommand("help", "")]
    public async void HelpAsync()
    {
        await this.RespondAsync(@"[cardname] anywhere in your message will pull up an image of cardname
/oracle <card> will return the oracle text of <card>, where <card> is the name of the card
/rulings <card> will return all rulings, their source, and the date they went into effect");
    }
}
