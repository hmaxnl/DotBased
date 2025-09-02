using DotBased.AspNet.Authority.Models;
using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DotBased.AspNet.Authority.EFCore.Repositories;

public class AttributeRepository(IDbContextFactory<AuthorityContext> contextFactory, ILogger<AttributeRepository> logger) : RepositoryBase, IAttributeRepository
{
    public async Task<QueryItems<AuthorityAttributeItem>> GetAttributesAsync(int limit = 20, int offset = 0, string search = "",
        CancellationToken cancellationToken = default)
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
        return QueryItems<AuthorityAttributeItem>.Create(select, total, limit, offset);
    }

    public async Task<AuthorityAttribute?> GetAttributeByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Attributes.FirstOrDefaultAsync(a => a.AttributeKey == key, cancellationToken);
    }

    public async Task<AuthorityAttribute?> CreateAttributeAsync(AuthorityAttribute attribute,
        CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(attribute.AttributeKey) || attribute.ForeignKey == Guid.Empty)
        {
            throw new Exception($"Attribute {attribute.AttributeKey} not found");
        }

        var entry = context.Attributes.Add(attribute);
        var saveResult = await context.SaveChangesAsync(cancellationToken);
        return saveResult != 0 ? entry.Entity : null;
    }

    public async Task<AuthorityAttribute?> UpdateAttributeAsync(AuthorityAttribute attribute, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var currentAttribute = await context.Attributes.FirstOrDefaultAsync(a => a.AttributeKey == attribute.AttributeKey, cancellationToken);
        if (currentAttribute == null)
        {
            return null;
        }

        if (currentAttribute.Version != attribute.Version)
        {
            logger.LogError("Attribute version validation failed for attribute {attribute}", currentAttribute.AttributeKey);
            return null;
        }
            
        var entry = context.Attributes.Update(currentAttribute);
        var saveResult = await context.SaveChangesAsync(cancellationToken);
        return saveResult != 0 ? entry.Entity : null;
    }

    public async Task<bool> DeleteAttributeAsync(AuthorityAttribute attribute, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var currentAttribute = await context.Attributes.FirstOrDefaultAsync(a => a.AttributeKey == attribute.AttributeKey, cancellationToken);
        
        if (currentAttribute == null)
        {
            logger.LogError("Attribute not found.");
            return false;
        }
        
        context.Attributes.Remove(currentAttribute);
        var saveResult = await context.SaveChangesAsync(cancellationToken);
        return saveResult != 0;
    }
}