using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Authorization.Permission
{
    public class Permission
    {

        private Permission(Guid setId, byte position, IList<string> scopes)
        {
            SetId = setId;
            Position = position;
            Scopes = scopes;
        }

        public Guid SetId { get; }

        public byte Position { get;}

        public IList<string> Scopes { get;}


        public static Permission Create(byte position, params string[] scopes)
        {
            return Create(Guid.NewGuid(), position, scopes);
        }

        public static Permission Create(Guid setId, byte position, params string[] scopes)
        {
            return new Permission(setId, position, new ReadOnlyCollection<string>(scopes));
        }
    }
}
