using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Magic_Bot
{
    class Program
    {
        public static Task Main(string[] args) => new Program().MainAsync();
        private DiscordSocketClient _client;
        private CommandService _commands;
        private CommandHandler _handler;
        private MessageEffectManager _messageHandler;
        public async Task MainAsync()
        {
            
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            

            _client.Log += Log;
            var token = File.ReadAllText("token");
            _handler = new CommandHandler(_client, _commands);
            _messageHandler = new MessageEffectManager(_client);
            
            await _handler.InstallCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
