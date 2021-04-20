// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.12.2

using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace ExplanyBot.Bots
{
    public class EchoBot : ActivityHandler
    {

        public static Dictionary<string, string> speak;

        public EchoBot()
        {
            GetLibrary().Wait();
        }

        static async Task GetLibrary()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://localhost:44342/Library");

            var message = response.Content.ReadAsStringAsync();
            speak = JsonConvert.DeserializeObject<Dictionary<string,string>>(message.Result);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            string msg = turnContext.Activity.Text.ToLower();

            foreach(var s in speak)
            {
                if (msg.ToLower().Contains(s.Key.ToLower()))
                {
                    var reply = $"What you said contains: {s.Value}";
                    await turnContext.SendActivityAsync(MessageFactory.Text(reply, reply), cancellationToken);
                    break;
                }
            }

            //var replyText = $"Echo: {turnContext.Activity.Text}";
            //await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
