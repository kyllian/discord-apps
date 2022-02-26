using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using RestSharp;
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
        public void Rules(string cardName)
        {
            if (this._interactionContext.User.IsBot)
            {
                return;
            }

            RestRequest request = new();
            throw new NotImplementedException();
            //var card = this._restClient.GetAsync<Card>()
        }
    }
}
