using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Coldew.Core.Organization;

namespace Coldew.Core.Search
{
    public class FavoriteSearcher : MetadataSearcher
    {
        MetadataFavoriteManager _favoriteManager;
        public FavoriteSearcher(MetadataFavoriteManager favoriteManager)
        {
            this._favoriteManager = favoriteManager;
        }

        public override bool Accord(User opUser, Metadata metadata)
        {
            return this._favoriteManager.IsFavorite(opUser, metadata);
        }
    }
}
