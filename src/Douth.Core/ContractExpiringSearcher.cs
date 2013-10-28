using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core;

namespace Douth.Core
{
    public class ContractExpiringSearcher : MetadataSearcher
    {
        public ContractExpiringSearcher()
        {

        }

        public override bool Accord(Metadata metadata)
        {
            Contract contract = metadata as Contract;
            if (contract == null)
            {
                throw new ArgumentNullException("contract");
            }

            return contract.Expiring;
        }
    }
}
