using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.SignalR;
using RandomChatScript.core;

namespace RandomChatScript.Hubs
{
    public class AnonymousHub : Hub
    {
        // DI ???
        private readonly IUnitOfWork _unitOfWork;
        public AnonymousHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Routing(string msg)
        {
            msg = HttpUtility.HtmlEncode(msg);
            foreach (KeyValuePair<string, string> i in _unitOfWork.AnonymousHubDataRepository.GetPairsData())
            {
                if (i.Key == Context.ConnectionId || i.Value == Context.ConnectionId)
                {
                    if (i.Key != Context.ConnectionId)
                    {
                        await Clients.Client(i.Key).SendAsync("strangerMessage",
                              new { Type = "T", Message = msg, Date = DateTime.UtcNow });

                    }
                    else
                    {
                        await Clients.Client(i.Value).SendAsync("strangerMessage",
                            new { Type = "T", Message = msg, Date = DateTime.UtcNow });
                    }
                }
            }

        }
        public async Task RoutingImages(string msg)
        {

            foreach (KeyValuePair<string, string> i in _unitOfWork.AnonymousHubDataRepository.GetPairsData())
            {
                if (i.Key == Context.ConnectionId || i.Value == Context.ConnectionId)
                {
                    if (i.Key != Context.ConnectionId)
                    {
                        await Clients.Client(i.Key).SendAsync("strangerMessage",
                            new { Type = "I", Message = msg, Date = DateTime.UtcNow, Sender = "Stranger" });
                    }
                    else
                    {
                        await Clients.Client(i.Value).SendAsync("strangerMessage",
                            new { Type = "I", Message = msg, Date = DateTime.UtcNow, Sender = "Stranger" });
                    }
                }
            }

        }
        public void RegisterAndConnect()
        {
            bool status = ConnectToStranger();
            if (!status)
            {
                RegisterToWaitingList();
                Clients.Caller.SendAsync("serverMessage",
                    "waitingNotify", DateTime.UtcNow);

            }
        }

        public override Task OnConnectedAsync()
        {
            RegisterAndConnect();
            return base.OnConnectedAsync();
        }


        public bool ConnectToStranger()
        {
            var callerId = Context.ConnectionId;
            var connectionToConnect = _unitOfWork.AnonymousHubDataRepository.GetFirstWaitingList();
            if (connectionToConnect != null && connectionToConnect != Context.ConnectionId)
            {
                _unitOfWork.AnonymousHubDataRepository.RemoveFirstWaitingList();
                _unitOfWork.AnonymousHubDataRepository.AddToPairsData(connectionToConnect, callerId);
                var temp = new List<string>
                {
                    connectionToConnect,
                    Context.ConnectionId
                };
                Clients.Clients(temp).SendAsync("serverMessage",
                    "connectedNotify", DateTime.UtcNow);
                Clients.Clients(temp).SendAsync("settingUpSetting");
                return true;
            }

            return false;
        }
        public bool RegisterToWaitingList()
        {
            var callerId = Context.ConnectionId;
            _unitOfWork.AnonymousHubDataRepository.AddToWaitingList(callerId);
            return true;
        }
        //public override Task OnReconnected()
        //{
        //    foreach (KeyValuePair<string, string> i in _unitOfWork.AnonymousHubDataRepository.GetPairsData())
        //    {
        //        if (i.Key == Context.ConnectionId || i.Value == Context.ConnectionId)
        //        {
        //            if (i.Key != Context.ConnectionId)
        //            {
        //                Clients.Client(i.Key).serverMessage(new { Message = "reconnectStranger", Date = DateTime.UtcNow, Sender = "Server" }); ;
        //            }
        //            else
        //            {
        //                Clients.Client(i.Value).serverMessage(new { Message = "reconnectStranger", Date = DateTime.UtcNow, Sender = "Server" }); ;
        //            }

        //        }
        //    }
        //    return base.OnReconnected();
        //}
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Leave();
            return base.OnDisconnectedAsync(exception);
        }


        public void Leave()
        {
            bool status = LeaveFromPairs();

            if (!status)
            {
                LeaveFromWaitingList();
            }
        }
        public bool LeaveFromPairs()
        {
            foreach (var i in _unitOfWork.AnonymousHubDataRepository.GetPairsData())
            {
                if (i.Key == Context.ConnectionId || i.Value == Context.ConnectionId)
                {
                    var temp = new List<string> { i.Value, i.Key };
                    Clients.Clients(temp).SendAsync("serverMessage", "disconnectNotify", DateTime.UtcNow);
                    Clients.Clients(temp).SendAsync("changeTyping", false);
                    _unitOfWork.AnonymousHubDataRepository.RemoveFromPairsData(i.Key);
                    Clients.Clients(temp).SendAsync("settingDownSetting");
                    return true;
                }
            }
            return false;
        }
        public bool LeaveFromWaitingList()
        {

            if (_unitOfWork.AnonymousHubDataRepository.CheckExistingWaitingList(Context.ConnectionId))
            {
                _unitOfWork.AnonymousHubDataRepository.RemoveFromWaitingList(Context.ConnectionId);
                return true;
            }
            return false;
        }
        public void Typing(bool state)
        {
            foreach (KeyValuePair<string, string> i in _unitOfWork.AnonymousHubDataRepository.GetPairsData())
            {
                if (i.Key == Context.ConnectionId || i.Value == Context.ConnectionId)
                {
                    if (i.Key != Context.ConnectionId)
                    {

                        Clients.Client(i.Key).SendAsync("changeTyping", state);

                    }
                    else
                    {

                        Clients.Client(i.Value).SendAsync("changeTyping", state);


                    }
                }
            }
        }
    }
}