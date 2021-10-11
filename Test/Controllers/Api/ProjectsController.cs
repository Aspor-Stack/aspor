using Aspor.EF;
using Aspor.EF.Extensions;
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

    [Route("api/projects")]
    public class ProjectsController : EFODataController<DatabaseContext>
    {

        public ProjectsController(DatabaseContext database) : base(database) {}

        [HttpGet]
        [EnableQuery]
        public IQueryable<Project> GetProjects()
        {
            return DbContext.Projects.Active();
        }

        [HttpGet("{projectId}")]
        [EnableQuery]
        public SingleResult<Project> GetProject([FromRoute] Guid projectId)
        {
            return SingleResult.Create(DbContext.Projects.Active().Where(p => p.Id == projectId));
        }

        /* Project Manipulation */

        [HttpPost]
        [EnableQuery]
        public async Task<IActionResult> PostProject([FromBody] Project project)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            project.Name = "Test";
            return await PostEntityAsync(project);
        }

        [HttpPatch("{projectId}")]
        [EnableQuery]
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
