using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DotBased.AspNet.Authority.EFCore.Repositories;

public class AttributeRepository(IDbContextFactory<AuthorityContext> contextFactory) : RepositoryBase, IAttributeRepository
{
    public async Task<ListResult<AuthorityAttributeItem>> GetAttributesAsync(int limit = 20, int offset = 0, string search = "",
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var query = context.Attributes.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => $"{a.AttributeKey} {a.ForeignKey} {a.AttributeValue}".Contains(search, StringComparison.CurrentCultureIgnoreCase));
            }
            
            var total = await query.CountAsync(cancellationToken);
            var select = await query.OrderBy(a => a.AttributeKey).Skip(offset).Take(limit).Select(a => new AuthorityAttributeItem()
            {
                BoundId = a.ForeignKey,
                AttributeKey = a.AttributeKey,
                AttributeValue = a.AttributeValue
            }).ToListAsync(cancellationToken);
            return ListResult<AuthorityAttributeItem>.Ok(select, total, limit, offset);
        }
        catch (Exception e)
        {
            return HandleExceptionListResult<AuthorityAttributeItem>("Failed to get attributes", e);
        }
    }

    public async Task<Result<AuthorityAttribute>> GetAttributeByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var attribute = await context.Attributes.FirstOrDefaultAsync(a => a.AttributeKey == key, cancellationToken);
            return attribute == null ? Result<AuthorityAttribute>.Failed("Attribute not found") : Result<AuthorityAttribute>.Ok(attribute);
        }
        catch (Exception e)
        {
            return HandleExceptionResult<AuthorityAttribute>("Failed to get attribute by id", e);
        }
    }

    public async Task<Result<AuthorityAttribute>> CreateAttributeAsync(AuthorityAttribute attribute, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            if (string.IsNullOrWhiteSpace(attribute.AttributeKey) || attribute.ForeignKey == Guid.Empty)
            {
                return Result<AuthorityAttribute>.Failed("Attribute key and/or bound id is empty");
            }
            var entry = context.Attributes.Add(attribute);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? Result<AuthorityAttribute>.Failed("Failed to create attribute") : Result<AuthorityAttribute>.Ok(entry.Entity);
        }
        catch (Exception e)
        {
            return HandleExceptionResult<AuthorityAttribute>("Failed to create attribute", e);
        }
    }

    public async Task<Result<AuthorityAttribute>> UpdateAttributeAsync(AuthorityAttribute attribute, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var currentAttribute = await context.Attributes.FirstOrDefaultAsync(a => a.AttributeKey == attribute.AttributeKey, cancellationToken);
            if (currentAttribute == null)
            {
                return Result<AuthorityAttribute>.Failed("Attribute not found");
            }

            if (currentAttribute.Version != attribute.Version)
            {
                return Result<AuthorityAttribute>.Failed("Attribute version doesn't match");
            }
            
            var entry = context.Attributes.Update(currentAttribute);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? Result<AuthorityAttribute>.Failed("Failed to update attribute") : Result<AuthorityAttribute>.Ok(entry.Entity);
        }
        catch (Exception e)
        {
            return HandleExceptionResult<AuthorityAttribute>("Failed to update attribute", e);
        }
    }

    public async Task<Result> DeleteAttributeAsync(AuthorityAttribute attribute, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
            var currentAttribute = await context.Attributes.FirstOrDefaultAsync(a => a.AttributeKey == attribute.AttributeKey, cancellationToken);
            if (currentAttribute == null)
            {
                return Result.Failed("Attribute not found");
            }
            context.Attributes.Remove(currentAttribute);
            var saveResult = await context.SaveChangesAsync(cancellationToken);
            return saveResult <= 0 ? Result.Failed("Failed to delete attribute") : Result.Ok();
        }
        catch (Exception e)
        {
            return HandleException("Failed to delete attribute", e);
        }
    }
}