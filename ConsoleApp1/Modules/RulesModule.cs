using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFiremind.Models;

namespace TheFiremind.Modules
{
    /// <summary>
    /// Module facilitating a rules command
    /// </summary>
    public class RulesModule : InteractionModuleBase<SocketInteractionContext<SocketSlashCommand>>
    {
        readonly InteractionContext _interactionContext;
        readonly DiscordSocketClient _socketClient;
        readonly RestClient _restClient;
        readonly ConfigurationManager _configurationManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="restClient"></param>
        /// <param name="interactionContext"></param>
        /// <param name="socketClient"></param>
        /// <param name="configurationManager"></param>
        public RulesModule(RestClient restClient, InteractionContext interactionContext, DiscordSocketClient socketClient, ConfigurationManager configurationManager)
        {
            this._restClient = restClient;
            this._interactionContext = interactionContext;
            this._socketClient = socketClient;
            this._configurationManager = configurationManager;
        }

        /// <summary>
        /// 
        /// </summary>
        [SlashCommand("rules", "")]
        public async void RulesAsync(string cardName)
        {
            if (this._interactionContext.User.IsBot)
            {
                return;
            }

            RestRequest request = new()

            var card = this._restClient.GetAsync<Card>()
        }
    }
}
