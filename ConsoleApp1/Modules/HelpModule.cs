using Discord.Interactions;

namespace TheFiremind.Modules;

/// <summary>
/// Module facilitating a help command
/// </summary>
public class HelpModule : InteractionModuleBase
{
    /// <summary>
    /// Responds with directions on how to use the bot
    /// </summary>
    [SlashCommand("help", "")]
    public async void HelpAsync()
    {
        await RespondAsync(@"[cardname] anywhere in your message will pull up an image of cardname
/oracle <card> will return the oracle text of <card>, where <card> is the name of the card
/rulings <card> will return all rulings, their source, and the date they went into effect");
    }
}
