using Messenger.Client.Services;
using Messenger.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Messenger.Client.Pages
{
    public partial class Conversation : IDisposable
    {
        [Parameter]
        public string id { get; set; }


        [Inject]
        public IUserService _user { get; set; }

        [Inject]
        public IMainService<User> _user2 { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        private List<Conv> convs { get; set; } = new List<Conv>();
        private string newMessage { get; set; }
        private CancellationTokenSource cancellationTokenSource;
        private string idemet;
        private string userName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var loggedInUser = await _user.GetLoggedInUser();
            idemet =  loggedInUser.Id ;
            string idrec = id;
            await _user.Seen($"api/Users/seen/{idemet}/{idrec}");

            convs = await _user.GetConv($"api/Users/Conversation/{idemet}/{idrec}");
            
            User user = await _user2.GetRow($"/api/Users/GetUser/{idrec}");
            userName = $"{user.Prenom} {user.Nom}";
            cancellationTokenSource = new CancellationTokenSource();
            _ = Task.Run(() => PollForNewMessages(cancellationTokenSource.Token));
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await ScrollToBottom();
        }

        private async Task ScrollToBottom()
        {
            await JSRuntime.InvokeVoidAsync("scrollToBottom", chatMessagesRef);
        }

        private async Task PollForNewMessages(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(500);
                await _user.Seen($"api/Users/seen/{idemet}/{id}");
                convs = await _user.GetConv($"api/Users/Conversation/{idemet}/{id}");
               
                   
                    StateHasChanged();
                    await ScrollToBottom();
                
            }
        }

        private async Task SendMessage()
        {
            var loggedInUser = await _user.GetLoggedInUser();
            if (!string.IsNullOrWhiteSpace(newMessage))
            {

                var newConv = new Conv
                {
                    IdEmet = loggedInUser.Id, // Replace with actual user ID
                    IdRec = id,
                    Message = newMessage,
                    Etat = 0,
                    Timestamp = DateTime.Now
                };
                await _user.SendMessage(newConv, "/api/Users/Sendmsg");
                convs.Add(newConv);
                newMessage = string.Empty;
                StateHasChanged();
                await ScrollToBottom();

            }
        }

        private async Task HandleKeyPress(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await SendMessage();
            }
        }

        public void Dispose()
        {
            cancellationTokenSource?.Cancel();
        }
    }
}