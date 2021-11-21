﻿using Aspor.Authorization.Extensions;
using Aspor.Authorization.User;
using Aspor.Common.Extensions;
using Aspor.Validation.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Aspor.EF
{

    public class EFODataController<TContext> : EFODataController where TContext : DbContext
    {

        public EFODataController(TContext dbContext) : base(dbContext) { }

        public TContext DbContext { get => _dbContext as TContext; }

    }

    public class EFODataController : ODataController
    {

        protected readonly DbContext _dbContext;

        public EFODataController(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public new AsporUser User { get => HttpContext.GetUser(); }

        protected SingleResult<TEntity> Single<TEntity>(IQueryable<TEntity> queryable) where TEntity : class
        {
            return SingleResult.Create(queryable);
        }

        protected async Task<IActionResult> PostEntityAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await HttpContext.ValidateRulesAsync(entity);
            if (!ModelState.IsValid) return BadRequest(ModelState);


            if (entity is IEntityExecutors executorEntity)
            {
                AsporUser user = HttpContext.GetUserOrDefault();
                if (user != null) executorEntity.CreatedBy = executorEntity.ModifiedBy = user.Id;
            }

            if(entity is IEntityTenancy entityTenancy)
            {
                AsporUser user = HttpContext.GetUserOrDefault();
                if (user != null) entityTenancy.TenantId = user.TenantId;
            }

            _dbContext.Add(entity);
            await _dbContext.SaveChangesAsync();

            return Ok(entity);
        }

        protected async Task<IActionResult> PatchEntityAsync<TEntity>(Delta<TEntity> delta, IQueryable<TEntity> queryable) where TEntity : class
        {
            if (HttpContext.IFMatch(queryable as IQueryable<object>)) return StatusCode(StatusCodes.Status412PreconditionFailed);

            TEntity entity = await queryable.SingleAsync();
            if (entity == null) return NotFound();

            delta.Patch(entity);

            if (entity is IEntityExecutors executorEntity)
            {
                AsporUser user = HttpContext.GetUserOrDefault();
                if (user != null) executorEntity.ModifiedBy = user.Id;
            }

            await HttpContext.ValidateRulesAsync(entity);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();

            return Updated(entity);
        }

        protected async Task<IActionResult> PutEntityAsync<TEntity>(Delta<TEntity> delta, IQueryable<TEntity> queryable) where TEntity : class
        {
            if (HttpContext.IFMatch(queryable as IQueryable<object>)) return StatusCode(StatusCodes.Status412PreconditionFailed);

            TEntity entity = await queryable.SingleAsync();
            if (entity == null) return NotFound();

            delta.Put(entity);

            if (entity is IEntityExecutors executorEntity)
            {
                AsporUser user = HttpContext.GetUserOrDefault();
                if (user != null) executorEntity.ModifiedBy = user.Id;
            }

            await HttpContext.ValidateRulesAsync(entity);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();

            return Updated(entity);
        }

        protected async Task<IActionResult> DeleteEntityAsync<TEntity>(IQueryable<TEntity> queryable) where TEntity : class
        {
            if (HttpContext.IFMatch(queryable as IQueryable<object>)) return StatusCode(StatusCodes.Status412PreconditionFailed);
            TEntity entity = await queryable.SingleAsync();
            if (entity == null) return NotFound();

            if (entity is IEntityExecutors executorEntity)
            {
                AsporUser user = HttpContext.GetUserOrDefault();
                if (user != null) executorEntity.DeletedBy = user.Id;
            }

            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();

            return Ok(entity);
        }
    }
}
