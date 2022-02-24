using Discord.Commands;
using Discord.WebSocket;
using MagicBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MagicBot
{
    class CommandHandler
    {
        readonly DiscordSocketClient _client;
        readonly CommandService _command;

        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            this._client = client;
            this._command = commands;

            this._client.MessageReceived += this.HandleCommandAsync;
        }

        async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            var messageContent = message.Content;

            if (message.Author.IsBot) { return; }
            var context = new SocketCommandContext(this._client, message);

            //if (messageContent.StartsWith("/oracle")) await this.OracleCommand(messageContent, context);
            //if (messageContent.StartsWith("/rulings")) await this.RulingsCommand(messageContent, context);
            if (messageContent.StartsWith("/help")) await HelpCommand(messageContent, context);
        }

        static async Task HelpCommand(string messageContent, SocketCommandContext context)
        {
            var msg = @"[cardname] anywhere in your message will pull up an image of cardname
/oracle <card> will return the oracle text of <card>, where <card> is the name of the card
/rulings <card> will return all rulings, their source, and the date they went into effect";
            await context.Channel.SendMessageAsync(msg);
        }

        // async Task RulingsCommand(string messageContent, SocketCommandContext context)
        //{
        //    var name = messageContent.Replace("/rulings ", "");
        //    var response = await APICall.GetCardAsync(name.Replace(" ", "+"));
        //    var uri = response.rulingsUri;
        //    var rulingsResponse = await APICall.GetRulings(uri.Replace("rulings", ""));
        //    if(rulingsResponse.rulings.Length == 0)
        //    {
        //        await context.Channel.SendMessageAsync("Could not find rulings for card " + name);
        //    }
        //    else
        //    {
        //        var msg = "";
        //        foreach(var s in rulingsResponse.rulings)
        //        {
        //            msg += $"From {s.source}: \n{s.comment} \nDated {s.date} \n\n";
        //        }
        //        await context.Channel.SendMessageAsync(msg.Replace("12:00:00 AM", ""));
        //    }
        //}
        // async Task OracleCommand(string messageContent, SocketCommandContext context)
        //{

        //    var name = messageContent.Replace("/oracle ", "");
        //    var response = await APICall.GetCardAsync(name.Replace(" ", "+"));
        //    if (response == default(Card))
        //    {
        //        await context.Channel.SendMessageAsync("Could not find card " + name);
        //    }
        //    else
        //    {
        //        await context.Channel.SendMessageAsync(response.name + " " + response.mana +":\n" + response.oracleText);
        //    }
        //}
    }
}
