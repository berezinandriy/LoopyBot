using LoopyBot.Models;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Luis.Models;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.FormFlow;

namespace LoopyBot.Dialogs
{
    [LuisModel("ad91e92d-bfcc-4d9e-ae78-966627d4d7c1", "abb84cd6730241618ab218f52b9dbc2d")]//app id, api key
    [Serializable]
    public class LUISDialog:LuisDialog<RoomReservation>
    {
        private readonly BuildFormDelegate<RoomReservation> ReserveRoom;

        public LUISDialog(BuildFormDelegate<RoomReservation> reserveRoom)
        {
            this.ReserveRoom = reserveRoom;
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result) {
            await context.PostAsync("I'm sorry I don't know what you mean.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            context.Call(new GreetingDialog(), Callback);
        }

        [LuisIntent("ReserveRoom")]
        public async Task RoomReservation(IDialogContext context, LuisResult result)
        {
            var enrollmentForm = new FormDialog<RoomReservation>(new RoomReservation(), this.ReserveRoom, FormOptions.PromptInStart);
            context.Call<RoomReservation>(enrollmentForm, Callback);
        }

        [LuisIntent("QueryAmenities")]
        public async Task QueryAmenities(IDialogContext context, LuisResult result)
        {
            foreach (var entity in result.Entities.Where(Entity=> Entity.Type == "Amenity"))
            {
                var value = entity.Entity.ToLower();
                if (value=="pool" || value == "gym" || value == "wifi" || value == "towels")
                {
                    await context.PostAsync("Yes, we have that!");
                    context.Wait(MessageReceived);
                    return;
                }
                else
                {
                    await context.PostAsync("I'm sorry we don't have that!");
                    context.Wait(MessageReceived);
                    return;
                }
            }
            await context.PostAsync("I'm sorry we don't have that!");
            context.Wait(MessageReceived);
            return;
        }

        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);
        }
    }
}