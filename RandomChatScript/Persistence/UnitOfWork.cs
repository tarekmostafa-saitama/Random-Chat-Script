using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RandomChatScript.core;
using RandomChatScript.Core.Repositories;
using RandomChatScript.persistence.Repositories;


namespace RandomChatScript.persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        
        public IAnonymousDataStoreRepository AnonymousHubDataRepository { get; set; }

        public UnitOfWork()
        {
            AnonymousHubDataRepository = new AnonymousDataStoreRepository();
        }
        public void Complete()
        {
            throw new NotImplementedException();
        }
    }
}
