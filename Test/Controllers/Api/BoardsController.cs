using Aspor.EF;
using Aspor.EF.Extensions;
using Authorization.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using System;
using System.Linq;
using System.Threading.Tasks;
using Test.Model;

namespace Test.Controllers.Api
{

    [Route("api/boards")]
    public class BoardsController : EFODataController<DatabaseContext>
    {

        public BoardsController(DatabaseContext database) : base(database) {}

        [HttpGet]
        [EnableQuery]
       // [RequiredPermission("project.read")]
        public IQueryable<Board> GetBoards()
        {
            return DbContext.Boards;
        }

        [HttpGet("{projectId}")]
        [EnableQuery]
      //  [RequiredPermission("project.read")]
        public SingleResult<Project> GetProject([FromRoute] Guid projectId)
        {
            return SingleResult.Create(DbContext.Projects.Active().Where(p => p.Id == projectId));
        }

        /* Project Manipulation */

        [HttpPost]
        [EnableQuery]
   //     [RequiredPermission("project.create")]
        public async Task<IActionResult> PostProject([FromBody] Project project)
        {
            return await PostEntityAsync(project);
        }

        [HttpPatch("{projectId}")]
        [EnableQuery]
     //   [RequiredPermission("project.patch", "projectId")]
        public async Task<IActionResult> PatchProject([FromRoute] Guid projectId, [FromBody] Delta<Project> delta)
        {
            return await PatchEntityAsync(delta,DbContext.Projects.Active().Where(p => p.Id == projectId));
        }

        [HttpPut("{projectId}")]
        [EnableQuery]
        public async Task<IActionResult> PutProject([FromRoute] Guid workspaceId, [FromRoute] Guid projectId, [FromBody] Delta<Project> delta)
        {
            return await PutEntityAsync(delta, DbContext.Projects.Active().Where(p => p.Id == projectId));
        }

        [HttpDelete("{projectId}")]
        [EnableQuery]
        public async Task<IActionResult> DeleteProject([FromRoute] Guid projectId)
        {
            return await DeleteEntityAsync(DbContext.Projects.Active().Where(p => p.Id == projectId));
        }

    }
}
