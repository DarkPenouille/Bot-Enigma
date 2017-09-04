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
        
        //public async Task StartAsync(IDialogContext context)
        //{
        //var msg = context.MakeMessage();
        //msg.Attachments.Add(getLocalisation());
        //await context.PostAsync(msg);
        //context.Wait(start);
         //}

    public async Task StartAsync(IDialogContext context)
    {
        var msg = context.MakeMessage();
        msg.Attachments.Add(getLocalisation());
        

        await context.PostAsync("Vous avez choisi de partir à l'aventure!");
        await context.PostAsync("A tout moment vous pouvez taper \"temps\" pour consulter le chrono.");
        await context.PostAsync("J'ai besoin de savoir où vous êtes pour trouver les quêtes proches de vous!");

        

        await context.PostAsync(msg);


        context.Wait(this.menuQuete);
    }

    public async Task menuQuete(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        var message = await argument as Activity;
        var reply = message.CreateReply();
        reply.Attachments = new List<Attachment>();

        HeroCard hc = new HeroCard()
        {
            Title = "Voici les quêtes les plus proches de vous",
            Subtitle = "",
            Buttons = new List<CardAction> { new CardAction(ActionTypes.PostBack, "Quête des wallons", value: "Quête des wallons"),
            new CardAction(ActionTypes.PostBack, "Tour du lac", value: "Tour du lac"),
            new CardAction(ActionTypes.PostBack, "Brasse-temps", value: "Brasse-temps")}
        };

        reply.Attachments.Add(hc.ToAttachment());
        await context.PostAsync(reply);

        context.Wait(this.firstLocalisation);
    }

    public async Task Challenges(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        var message = await argument as Activity;

        await context.PostAsync("Vous avez choisi la quête "+message.Text+"!");

        var reply = message.CreateReply();
        reply.Attachments = new List<Attachment>();
        reply.Attachments.Add(getLocalisation());
        await context.PostAsync(reply);

        context.Wait(this.firstLocalisation);
    }
    

    public async Task firstLocalisation(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        var message = await argument as Activity;
        var reply = message.CreateReply();

        await context.PostAsync("Vous avez choisi la quête \"" + message.Text + "\"!");

        await context.PostAsync("Voici la route à suivre jusqu'à la première épreuve");

        var heroCard = new HeroCard
        {
            Title = "Suivez ce chemin.",
            //Subtitle = "Your bots — wherever your users are talking",
            Images = new List<CardImage> { new CardImage("https://imagizer.imageshack.us/v2/594x420q90/924/ejJpDP.png") }
        };
        
        
        reply.Attachments = new List<Attachment>();
        reply.Attachments.Add(heroCard.ToAttachment());
        await context.PostAsync(reply);

        await context.PostAsync("Actualisez votre position pour connaitre le chemin à suivre.");

        reply.Attachments = new List<Attachment>();
        reply.Attachments.Add(getLocalisation());
        await context.PostAsync(reply);

        context.Wait(this.bite);
    }

    public async Task bite(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        var message = await argument as Activity;
        var reply = message.CreateReply();

        if (message.Text.StartsWith("localisation"))
        {
            var heroCard = new HeroCard
            {
                Title = "Suivez ce chemin.",
                //Subtitle = "Your bots — wherever your users are talking",
                Images = new List<CardImage> { new CardImage("https://imagizer.imageshack.us/v2/594x420q90/924/ejJpDP.png") }
            };

            reply.Attachments = new List<Attachment>();
            reply.Attachments.Add(heroCard.ToAttachment());
            await context.PostAsync(reply);

            await context.PostAsync("Actualisez votre position pour connaitre le chemin à suivre.");

            reply.Attachments = new List<Attachment>();
            reply.Attachments.Add(getLocalisation());
            await context.PostAsync(reply);

            context.Wait(this.bite);
        //}else(message.Text.StartsWith("temps"))
        //{
            //await context.PostAsync("Vous jouez depuis 35 secondes.");
            //context.Wait(this.bite);
        }else
        {
            context.Call(new menuP(1), after);

        }
       // else
        //{
         //   await context.PostAsync("Je n'ai pas compris votre message, vous pouvez envoyer 'menu' pour retourner sur le menu.");
          //  context.Wait(this.bite);
        //}

        context.Wait(this.bite);
    }



    public async Task secondLocalisation(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        var message = await argument as Activity;
        var reply = message.CreateReply();

        var heroCard = new HeroCard
        {
            Title = "Suivez ce chemin.",
            //Subtitle = "Your bots — wherever your users are talking",
            Images = new List<CardImage> { new CardImage("https://imagizer.imageshack.us/v2/522x438q90/922/x61eEs.png") }
        };

        reply.Attachments = new List<Attachment>();
        reply.Attachments.Add(heroCard.ToAttachment());
        await context.PostAsync(reply);

        await context.PostAsync("Vous êtes arrivé!");
        await context.PostAsync("Voici la première épreuve:");
        await context.PostAsync("A quel hauteur, en mètres, s'élève le clocher de l'église?");

        context.Wait(this.reponseOne);
    }

    public async Task reponseOne(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        await context.PostAsync("Faux!");
        context.Wait(this.indice);
    }

    public async Task indice(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        await context.PostAsync("Chacune des marches de la tour fait 21 centimètres.");
        context.Wait(this.secondEpreuve);
    }

    public async Task secondEpreuve(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        await context.PostAsync("Correct!");
        await context.PostAsync("Voici la route jusqu'à la seconde épreuve:");

        var message = await argument as Activity;
        var reply = message.CreateReply();

        var heroCard = new HeroCard
        {
            Title = "Suivez ce chemin.",
            //Subtitle = "Your bots — wherever your users are talking",
            Images = new List<CardImage> { new CardImage("https://imagizer.imageshack.us/v2/660x460q90/923/FqyH4x.png") }
        };

        reply.Attachments = new List<Attachment>();
        reply.Attachments.Add(heroCard.ToAttachment());
        await context.PostAsync(reply);

        await context.PostAsync("Actualisez votre position pour connaitre le chemin à suivre.");

        reply.Attachments = new List<Attachment>();
        reply.Attachments.Add(getLocalisation());
        await context.PostAsync(reply);

        context.Wait(this.chrono);
    }

    public async Task chrono(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        var message = await argument as Activity;
        var reply = message.CreateReply();

        await context.PostAsync("Vous jouez depuis 2 minutes et 35 secondes.");

        await context.PostAsync("Actualisez votre position pour connaitre le chemin à suivre.");

        reply.Attachments = new List<Attachment>();
        reply.Attachments.Add(getLocalisation());
        await context.PostAsync(reply);

        context.Wait(this.lastLocalisation);
    }

    public async Task lastLocalisation(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        var message = await argument as Activity;
        var reply = message.CreateReply();
        

        var heroCard = new HeroCard
        {
            Title = "Suivez ce chemin.",
            //Subtitle = "Your bots — wherever your users are talking",
            Images = new List<CardImage> { new CardImage("https://imagizer.imageshack.us/v2/628x401q90/924/C5AxYb.png") }
        };

        reply.Attachments = new List<Attachment>();
        reply.Attachments.Add(heroCard.ToAttachment());
        await context.PostAsync(reply);

        await context.PostAsync("Actualisez votre position pour connaitre le chemin à suivre.");

        reply.Attachments = new List<Attachment>();
        reply.Attachments.Add(getLocalisation());
        await context.PostAsync(reply);

        context.Wait(this.lastLocalisationEver);
    }

    public async Task lastLocalisationEver(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        var message = await argument as Activity;
        var reply = message.CreateReply();


        var heroCard = new HeroCard
        {
            Title = "Suivez ce chemin.",
            //Subtitle = "Your bots — wherever your users are talking",
            Images = new List<CardImage> { new CardImage("https://imagizer.imageshack.us/v2/519x445q90/923/BKo21u.png") }
        };

        reply.Attachments = new List<Attachment>();
        reply.Attachments.Add(heroCard.ToAttachment());
        await context.PostAsync(reply);

        await context.PostAsync("Vous êtes arrivé!");
        await context.PostAsync("Voici la seconde épreuve:");
        await context.PostAsync("En quelle année le kiosque de la place de Rome a-t-il été construit?");

        context.Wait(this.endQuest);
    }

    public async Task endQuest(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        var message = await argument as Activity;
        var reply = message.CreateReply();

        await context.PostAsync("Correct!");
        await context.PostAsync("Vous avez terminé la quête de Malmedy en 4 minutes et 45 secondes!");
        await context.PostAsync("Votre score est de 17 points!");

        await context.PostAsync("Sur une échelle de 1 à 10, comment noteriez-vous cette quête?");
        
        context.Wait(this.remerciements);
    }

    public async Task remerciements(IDialogContext context, IAwaitable<IMessageActivity> argument)
    {
        await context.PostAsync("Merci pour votre appréciation et à bientôt!");
        context.Call(new menuP(1), this.after);
    }

    private async Task after(IDialogContext context, IAwaitable<object> result)
    {
        try
        {
            var message = await result;
        }
        catch (Exception ex)
        {
            await context.PostAsync($"Failed with message: {ex.Message}");
        }
        finally
        {
        }
    }

    private static Attachment getLocalisation()
    {
        var heroCard = new HeroCard
        {
            Title="Envoyez votre localisation",
            //Subtitle = "Your bots — wherever your users are talking",

            Buttons = new List<CardAction> { new CardAction(ActionTypes.PostBack, "Envoyer", value: "localisation") }
        };

        return heroCard.ToAttachment();
    }

        
    }
