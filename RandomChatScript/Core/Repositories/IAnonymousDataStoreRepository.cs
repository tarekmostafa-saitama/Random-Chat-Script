using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RandomChatScript.Core.Repositories
{
    public interface IAnonymousDataStoreRepository
    {
        void AddToWaitingList(string connectionId);
        void RemoveFromWaitingList(string connectionId);
        bool CheckExistingWaitingList(string connectionId);
        string GetFirstWaitingList();
        void RemoveFirstWaitingList();
        Dictionary<string, string> GetPairsData();
        void RemoveFromPairsData(string key);
        void AddToPairsData(string key, string value);
    }
}
