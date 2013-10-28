using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core;

namespace Douth.Core
{
    public class ContractExpiredSearcher : MetadataSearcher
    {
        public ContractExpiredSearcher()
        {

        }

        public override bool Accord(Metadata metadata)
        {
            Contract contract = metadata as Contract;
            if (contract == null)
            {
                throw new ArgumentNullException("contract");
            }

            return contract.Expired;
        }
    }
}
