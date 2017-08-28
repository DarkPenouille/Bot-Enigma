using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;


    [Serializable]
    public class explicationDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            

            await context.PostAsync("Je suis un bot proposant des jeux de pistes accessibles à tous!");
            await context.PostAsync("En choisissant \"Quête\" dans le menu principal, vous pourrez visualiser toutes les quêtes qui sont à proximité de votre emplacement.");
            await context.PostAsync("Chaque quête est constituée d'une suite d'épreuves et d'énigmes situées à différents endroits. Pour connaitre la route, envoyez moi votre localisation et je vous guiderai.");
            await context.PostAsync("Si une énigme vous pose problème, envoyez \"indice\" pour recevoir de l'aide ou \"j'abandonne\" pour passer à l'épreuve suivante.");
            await context.PostAsync("A tout moment, vous pouvez envoyer \"menu\" pour revenir au menu ou \"temps\" pour savoir depuis combien de temps vous jouez.");

            context.Call(new menuP(1), this.after);

        }

        public async Task envoiMenu(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument as Activity;


            //context.Wait(MessageReceivedAsync);

            var menu = message.CreateReply();

            context.Call(new menuP(1), after);
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
    }
