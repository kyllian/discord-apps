using Discord.Commands;
using Discord.WebSocket;
using Magic_Bot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Magic_Bot
{
    class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _command;
        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            _client = client;
            _command = commands;
        }
        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _command.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
                                           services: null);
        }
        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            var messageContent = message.Content;

            if(message.Author.IsBot) { return; }
            var context = new SocketCommandContext(_client, message);

            if (messageContent.StartsWith("/oracle")) await OracleCommand(messageContent, context);
            if (messageContent.StartsWith("/rulings")) await RulingsCommand(messageContent, context);
            if (messageContent.StartsWith("/help")) await HelpCommand(messageContent, context);
        }

        private async Task HelpCommand(string messageContent, SocketCommandContext context)
        {
            var msg = @"[cardname] anywhere in your message will pull up an image of cardname
/oracle <card> will return the oracle text of <card>, where <card> is the name of the card
/rulings <card> will return all rulings, their source, and the date they went into effect";
            await context.Channel.SendMessageAsync(msg);
        }

        private async Task RulingsCommand(string messageContent, SocketCommandContext context)
        {
            var name = messageContent.Replace("/rulings ", "");
            var response = await APICall.GetCardAsync(name.Replace(" ", "+"));
            var uri = response.rulingsUri;
            var rulingsResponse = await APICall.GetRulings(uri.Replace("rulings", ""));
            if(rulingsResponse.rulings.Length == 0)
            {
                await context.Channel.SendMessageAsync("Could not find rulings for card " + name);
            }
            else
            {
                var msg = "";
                foreach(var s in rulingsResponse.rulings)
                {
                    msg += $"From {s.source}: \n{s.comment} \nDated {s.date} \n\n";
                }
                await context.Channel.SendMessageAsync(msg.Replace("12:00:00 AM", ""));
            }
        }
        private async Task OracleCommand(string messageContent, SocketCommandContext context)
        {

            var name = messageContent.Replace("/oracle ", "");
            var response = await APICall.GetCardAsync(name.Replace(" ", "+"));
            if (response == default(Card))
            {
                await context.Channel.SendMessageAsync("Could not find card " + name);
            }
            else
            {
                await context.Channel.SendMessageAsync(response.name + " " + response.mana +":\n" + response.oracleText);
            }
        }
    }
}
