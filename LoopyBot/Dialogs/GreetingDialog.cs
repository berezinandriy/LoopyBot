using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;

namespace LoopyBot.Dialogs
{
    [Serializable]
    public class GreetingDialog : IDialog
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Hi, I'm Loopy Bot");
            await Respond(context);

            context.Wait(MessageRecievedAsync);
        }

        private static async Task Respond(IDialogContext context) {
            var userName = string.Empty;
            context.UserData.TryGetValue<string>("Name", out userName);
            if (string.IsNullOrEmpty(userName))
            {
                await context.PostAsync("What is your name");
                context.UserData.SetValue<bool>("GetName", true);
            }
            else
            {
                await context.PostAsync($"Hi {userName}. How can I help you today?");
            }
        }

        private async Task MessageRecievedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            #region beginning
            //var message = await result;
            //var userName = String.Empty;
            ////check if name is part of the context property bag
            //context.UserData.TryGetValue<string>("Name", out userName);

            //if (string.IsNullOrEmpty(userName))
            //{
            //    await context.PostAsync("What is your name");
            //    userName = message.Text;
            //    context.UserData.SetValue<string>("Name", userName);
            //}
            //else {
            //    await context.PostAsync($"Hi {userName}. How can I help you today?");
            //}
            //context.Wait(MessageRecievedAsync);
            #endregion

            #region correct
            //var message = await result;
            //var userName = String.Empty;
            //var getName = false;
            ////check if name is part of the context property bag
            //context.UserData.TryGetValue<string>("Name", out userName);
            //context.UserData.TryGetValue<bool>("GetName", out getName);

            //if (getName)
            //{
            //    userName = message.Text;
            //    context.UserData.SetValue<string>("Name", userName);
            //    context.UserData.SetValue<bool>("GetName", false);
            //}

            //if (string.IsNullOrEmpty(userName))
            //{
            //    await context.PostAsync("What is your name");
            //    context.UserData.SetValue<bool>("GetName", true);
            //}
            //else
            //{
            //    await context.PostAsync($"Hi {userName}. How can I help you today?");
            //}
            //context.Wait(MessageRecievedAsync);
            #endregion

            var message = await result;
            var userName = String.Empty;
            var getName = false;
            //check if name is part of the context property bag
            context.UserData.TryGetValue<string>("Name", out userName);
            context.UserData.TryGetValue<bool>("GetName", out getName);

            if (getName)
            {
                userName = message.Text;
                context.UserData.SetValue<string>("Name", userName);
                context.UserData.SetValue<bool>("GetName", false);
            }

            await Respond(context);
            context.Done(message);
        }
    }
}