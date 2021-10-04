using Aspor.Authorization.User;
using System.Collections.Generic;


/*
 * Request related
 * test.user.{id} =>test.user.da095834-f53d-4cb7-ac6c-cdc967c7c4e2
 * test.[user|group].{id} => Path Property
 * test.[user|group].{H:id} => Header Property
 * test.[user|group].{Q:id} => Query Property
 * test.[user|group].{P:id} => Parameter Property
 * test.user.*
 * 
 * Schema related
 * test.user.${id} => test.user.da095834-f53d-4cb7-ac6c-cdc967c7c4e2
 * test.[user|group].${id} => test.user.da095834-f53d-4cb7-ac6c-cdc967c7c4e2 | test.group.da095834-f53d-4cb7-ac6c-cdc967c7c4e2
 * test.user.*
 * test.*.${id} => test.user.da095834-f53d-4cb7-ac6c-cdc967c7c4e2
 */

namespace Aspor.Authorization.Schema.Element
{
    public interface IPermissionElement
    {
        public void AppendPermissions(AsporUser user, IList<string> permissions);

    }
}
