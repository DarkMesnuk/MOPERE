using AutoMapper;
using Base.Domain.Exceptions;
using Base.Domain.Extensions;
using Base.Domain.Schemas;
using Base.Infrastructure.Database.PostgreSQL.Extensions;
using Base.Infrastructure.Database.PostgreSQL.Repositories;
using Domain.Interfaces.Repositories.Users;
using Domain.Models.Users;
using Domain.Schemas.Users;
using Infrastructure.Database.PostgreSQL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Database.PostgreSQL.Repositories;

public class UsersRepository(
	MopereIdentityContext context,
	ILogger<UsersRepository> logger,
	IMapper mapper
) : BaseRepository<UserEntity, UserModel, string>(context, logger, mapper), IUsersRepository
{
	protected override IQueryable<UserEntity> GetBase => base.GetBase
		.Where(x => !x.IsDeleted);
	
	public Task<bool> ExistsByEmailAsync(string email)
	{
		var normalizedEmail = email.ToNormalized();
		return GetBase.AnyAsync(x => x.NormalizedEmail == normalizedEmail);
	}
	
	public Task<bool> ExistsByUserNameAsync(string userName)
	{
		var normalizedUserName = userName.ToNormalized();
		return GetBase.AnyAsync(x => x.NormalizedUserName == normalizedUserName);
	}

	public async Task<UserModel> GetWithIncludesByIdAsync(string id, CancellationToken cancellationToken = default)
	{
		var entity = await GetBase
			.Include(x => x.UserRoles)!
				.ThenInclude(x => x.Role)
			.Include(x => x.UserClaims)!
				.ThenInclude(x => x.Claim)
			.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

		if (entity.IsNullOrDefault())
			throw new NotFoundException(nameof(UserModel), id);

		return MapToModel(entity);
	}

	public Task<List<string>> GetIdsByRoleAsync(string roleId, CancellationToken cancellationToken = default)
	{
		return GetBase
			.Include(x => x.UserRoles)
			.Where(x => x.UserRoles != null && x.UserRoles.Any(r => r.RoleId == roleId))
			.Select(x => x.Id)
			.ToListAsync(cancellationToken);
	}

	public async Task UpdateBySchemaAsync(UserModel user, IUpdateUserSchema schema)
	{
		if (schema.UserName != null)
		{
			var existsUserName = await ExistsByUserNameAsync(schema.UserName);

			if (!existsUserName)
			{
				user.UserName = schema.UserName ?? user.UserName;	
			}
			else
			{
				throw new AlreadyExistsException("UserName already exists");
			}
		}

		user.FirstName = schema.FirstName ?? user.FirstName;
		user.LastName = schema.LastName ?? user.LastName;

		if (schema.DeleteAvatar == true)
		{
			user.AvatarUrl = null;
		}
        
		await UpdateAsync(user);
	}

	public async Task<UserModel> GetByEmailAsync(string email)
	{
		var emailNormalized = email.ToNormalized();
		var user = await GetBase.FirstOrDefaultAsync(x => x.NormalizedEmail == emailNormalized);
		
		if (user.IsNullOrDefault())
			throw new NotFoundException(nameof(UserModel), email);
		
		return MapToModel(user);
	}

	public async Task<PaginatedResponse<UserModel>> GetAsync(IGetUsersAdminSchema schema, CancellationToken cancellationToken = default)
	{
		var query = GetBase
			.Include(x => x.UserRoles)!
				.ThenInclude(x => x.Role)
			.Include(x => x.UserClaims)!
				.ThenInclude(x => x.Claim)
			.AsQueryable();

		if (!string.IsNullOrWhiteSpace(schema.Search))
		{
			query = query.Where(x => EF.Functions.ILike(x.Email!, $"{schema.Search}%") ||
			                         x.Id == schema.Search);
		}

		if (!string.IsNullOrWhiteSpace(schema.Email))
		{
			query = query.Where(x => EF.Functions.ILike(x.Email!, $"{schema.Email}%"));
		}

		if (!string.IsNullOrWhiteSpace(schema.UserName))
		{
			query = query.Where(x => EF.Functions.ILike(x.UserName!, $"{schema.UserName}%"));
		}

		if (schema.CreatedFrom.HasValue)
		{
			query = query.Where(x => x.CreatedAt >= schema.CreatedFrom);
		}

		if (schema.CreatedTo.HasValue)
		{
			query = query.Where(x => x.CreatedAt <= schema.CreatedTo);
		}

		var users = await query
			.SortByTargetOrDefault(schema)
			.Page(schema)
			.ToPaginatedAsync(query, cancellationToken);

		return MapToModel(users);
	}

	public async Task<PaginatedResponse<UserModel>> GetAsync(IGetUsersSchema schema, CancellationToken cancellationToken = default)
	{
		var query = GetBase
			.AsQueryable();

		if (!string.IsNullOrWhiteSpace(schema.Search))
		{
			query = query.Where(x => EF.Functions.ILike(x.Email!, $"{schema.Search}%") ||
			                         x.Id == schema.Search);
		}

		if (!string.IsNullOrWhiteSpace(schema.Email))
		{
			query = query.Where(x => EF.Functions.ILike(x.Email!, $"{schema.Email}%"));
		}

		if (!string.IsNullOrWhiteSpace(schema.UserName))
		{
			query = query.Where(x => EF.Functions.ILike(x.UserName!, $"{schema.UserName}%"));
		}

		var users = await query
			.SortByTargetOrDefault(schema)
			.Page(schema)
			.ToPaginatedAsync(query, cancellationToken);

		return MapToModel(users);
	}
}