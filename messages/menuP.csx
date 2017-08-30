using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;


[Serializable]
    public class menuP : IDialog<object>
    {
        public string[] choixMenu;
        private int counter;

        public menuP(int count)
        {
            counter = count;
            choixMenu = new string[] {"Quêtes","Explications","Site officiel"};
        }

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument as Activity; 
            
            if(counter == 0)
            {
                await context.PostAsync("Salut");
            }

        //context.Wait(MessageReceivedAsync);
        //await context.PostAsync("Salut! Je suis Enigmaa, es-tu prêt à partir à l'aventure?!");
        var menu = message.CreateReply();

            menu.Attachments = new List<Attachment>();
            menu.Attachments.Add(GetThumbnailCard());
             
            await context.PostAsync(menu);

            context.Wait(this.choice);

        }

        

        public async Task choice(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument as Activity;

            if (message.Text == ("quetes"))
            {
                context.Call(new questDialog(), this.after);
            }
            else if (message.Text == ("explications"))
            {
                context.Call(new explicationDialog(), this.after);
            }
        }

        private async Task after(IDialogContext context, IAwaitable<object> result)
        {

        }

    private static Attachment GetThumbnailCard()
    {
        var heroCard = new HeroCard
        {
            Title = "Que l'aventure commence!",
            //Subtitle = "Your bots — wherever your users are talking",
            Text = "Pour commencer à jouer, choisissez \"Quêtes\" ou \"Explications\" pour apprendre à jouer.",
            Images = new List<CardImage> { new CardImage("https://s-media-cache-ak0.pinimg.com/736x/36/a4/85/36a48536da0dc7609432c61e3e93aba6--jouer-smartphone.jpg") },
            //Buttons = 
            Buttons = new List<CardAction> { new CardAction(ActionTypes.PostBack, "Quêtes", value: "quetes"),
                new CardAction(ActionTypes.PostBack, "Explications", value: "explications"),
                new CardAction(ActionTypes.OpenUrl, "Site officiel", value: "https://docs.microsoft.com/bot-framework")}
        };

        return heroCard.ToAttachment();
    }
}

