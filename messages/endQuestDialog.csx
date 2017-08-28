using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;


    [Serializable]
    public class questDialog : IDialog<object>
    {
        public DateTime time;
        public dataQuest quest;
        public int challengeCounter;

        public questDialog()
        {
            this.challengeCounter = 0;
        }
        

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Vous avez choisi de partir à l'aventure!");
            await context.PostAsync("A tout moment vous pouvez taper \"temps\" pour consulter le chrono.");
            await context.PostAsync("Voici la route menant à votre première épreuve!");
            
            context.Wait(this.Challenges);
        }

        public async Task Challenges(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument as Activity;

            var reply = message.CreateReply();
            reply.Attachments = new List<Attachment>();

            HeroCard hc = new HeroCard()
            {
                Title = "titre",
                Subtitle = "sous-titre"
            };

            List<CardImage> images = new List<CardImage>();

            CardImage ci = new CardImage("C:\\Users\\Florian\\Documents\\quest\\images\\epToep.png");
            images.Add(ci);

            hc.Images = images;

            reply.Attachments.Add(hc.ToAttachment());
            await context.PostAsync(reply);

            challengeCounter++;

            
            
            context.Wait(Challenges);
        }
    }
