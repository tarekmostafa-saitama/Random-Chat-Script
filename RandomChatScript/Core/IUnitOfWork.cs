using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RandomChatScript.Core.Repositories;


namespace RandomChatScript.core
{
    public interface IUnitOfWork
    {
      
        IAnonymousDataStoreRepository AnonymousHubDataRepository { get; set; }
        void Complete();
    }
}
