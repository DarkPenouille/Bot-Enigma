using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

[Serializable]
    public class questDialog : IDialog<object>
    {
        public DateTime time;
        public int challengeCounter;

        public questDialog()
        {
            time = DateTime.Now;
        this.challengeCounter = 0;
        }
        
        public async Task StartAsync(IDialogContext context)
        {
        context.Wait(start);
      }

    public async Task start(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        var message = await argument as Activity;
        var reply = message.CreateReply();
        reply.Attachments = new List<Attachment>();

        await context.PostAsync("Vous avez choisi de partir à l'aventure!");
        await context.PostAsync("A tout moment vous pouvez taper \"temps\" pour consulter le chrono.");
        await context.PostAsync("Voici les quêtes proches de vous");


        HeroCard hc = new HeroCard()
        {
            Title = "Voici les quêtes les plus proches de vous",
            Subtitle = "",
            Buttons = new List<CardAction> { new CardAction(ActionTypes.PostBack, "Quête de Malmedy", value: "1"),
            new CardAction(ActionTypes.PostBack, "Quête du café de la place", value: "2"),
            new CardAction(ActionTypes.PostBack, "Parours de la cathédrale", value: "3"),
            new CardAction(ActionTypes.PostBack, "Quête des champignons", value: "4") }
        };

        reply.Attachments.Add(hc.ToAttachment());
        await context.PostAsync(reply);

        context.Wait(this.Challenges);
    }

        public async Task Challenges(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            
        }
    }
