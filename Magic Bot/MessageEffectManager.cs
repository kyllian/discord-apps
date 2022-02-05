using Discord.WebSocket;
using Magic_Bot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Magic_Bot
{
    class MessageEffectManager
    {
        DiscordSocketClient _client;
        public MessageEffectManager(DiscordSocketClient client)
        {
            _client = client;
            _client.MessageReceived += onMessage;
        }

        public async Task onMessage(SocketMessage message)
        {
            var contents = message.Content;
            if (message.Author.IsBot || !(contents.Contains('[') && contents.Contains(']'))) return;
            var cards = new List<string>();
            bool bracket = false;
            var builder = new StringBuilder();
            foreach(var c in contents)
            {
                if (bracket)
                {
                    if(c == ']')
                    {
                        cards.Add(builder.ToString());
                        bracket = false;
                        builder = builder.Clear();
                        continue;
                    }
                    builder.Append(c);
                }
                else
                {
                    if(c == '[')
                    {
                        bracket = true;
                    }
                }
            }

            foreach (var s in cards.Distinct())
            {
                var searchTerm = s.ToString().Replace(" ", "+");
                var response = await APICall.GetCardAsync(searchTerm);
                if (response != default(Card))
                {
                    var imageUri = response.uris;
                    string url;
                    if (imageUri.png != null) url = imageUri.png;
                    else if (imageUri.border_crop != null) url = imageUri.border_crop;
                    else if (imageUri.normal != null) url = imageUri.normal;
                    else url = "";
                    await message.Channel.SendMessageAsync(url);
                }
            }
        }
    }
}
