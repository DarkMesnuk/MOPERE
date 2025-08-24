using AutoMapper;
using Base.Domain.Extensions;
using Domain.Models.Authentication;
using Domain.Models.Users;
using Infrastructure.Database.PostgreSQL.Entities;

namespace Infrastructure.Database.PostgreSQL.Mapping;

public class EntitiesMapping : Profile
{
	public EntitiesMapping()
	{
		CreateMap<UserEntity, UserModel>();
		CreateMap<UserModel, UserEntity>()
			.ForMember(x => x.NormalizedEmail, expression => expression.MapFrom(x => x.Email.ToNormalized()))
			.ForMember(x => x.NormalizedUserName, expression => expression.MapFrom(x => x.UserName.ToNormalized()))
			.ForMember(x => x.UserRoles, expression => expression.Ignore())
			.ForMember(x => x.UserClaims, expression => expression.Ignore());
		
		CreateMap<UserRoleEntity, UserRoleModel>()
			.ForMember(x => x.User, expression => expression.MapFrom(x => x.User ?? new UserEntity { Id = x.UserId }))
			.ForMember(x => x.Role, expression => expression.MapFrom(x => x.Role ?? new RoleEntity { Id = x.RoleId }));
		CreateMap<UserRoleModel, UserRoleEntity>()
			.ForMember(x => x.UserId, expression => expression.MapFrom(x => x.User.Id))
			.ForMember(x => x.User, expression => expression.Ignore())
			.ForMember(x => x.Role, expression => expression.Ignore())
			.ForMember(x => x.RoleId, expression => expression.MapFrom(x => x.Role.Id));
        
		CreateMap<RoleEntity, RoleModel>()
			.ForMember(x => x.Claims, expression => expression.MapFrom(x => x.Claims != null ? x.Claims.Select(roleClaimLinkEntity => roleClaimLinkEntity.Claim) : new List<ClaimEntity>()));
		CreateMap<RoleModel, RoleEntity>()
			.ForMember(x => x.Claims, expression => expression.Ignore())
			.ForMember(x => x.UserRoles, expression => expression.Ignore())
			.ForMember(x => x.NormalizedName, expression => expression.MapFrom(x => x.Name.ToNormalized()));
        
		CreateMap<RoleClaimEntity, RoleClaimModel>()
			.ForMember(x => x.Role, expression => expression.MapFrom(x => x.Role ?? new RoleEntity { Id = x.RoleId }))
			.ForMember(x => x.Claim, expression => expression.MapFrom(x => x.Claim ?? new ClaimEntity { Id = x.ClaimId }));
		CreateMap<RoleClaimModel, RoleClaimEntity>()
			.ForMember(x => x.RoleId, expression => expression.MapFrom(x => x.Role.Id))
			.ForMember(x => x.Role, expression => expression.Ignore())
			.ForMember(x => x.Claim, expression => expression.Ignore())
			.ForMember(x => x.ClaimType, expression => expression.Ignore())
			.ForMember(x => x.ClaimValue, expression => expression.Ignore())
			.ForMember(x => x.ClaimId, expression => expression.MapFrom(x => x.Claim.Id));
        
		CreateMap<UserClaimEntity, UserClaimModel>()
			.ForMember(x => x.User, expression => expression.MapFrom(x => x.User ?? new UserEntity { Id = x.UserId }))
			.ForMember(x => x.Claim, expression => expression.MapFrom(x => x.Claim ?? new ClaimEntity { Id = x.ClaimId }));
		CreateMap<UserClaimModel, UserClaimEntity>()
			.ForMember(x => x.UserId, expression => expression.MapFrom(x => x.User.Id))
			.ForMember(x => x.User, expression => expression.Ignore())
			.ForMember(x => x.Claim, expression => expression.Ignore())
			.ForMember(x => x.ClaimType, expression => expression.Ignore())
			.ForMember(x => x.ClaimValue, expression => expression.Ignore())
			.ForMember(x => x.ClaimId, expression => expression.MapFrom(x => x.Claim.Id));
        
		CreateMap<ClaimEntity, ClaimModel>()
			.ForMember(x => x.Roles, expression => expression.MapFrom(x => x.Roles != null ? x.Roles.Select(roleClaimLinkEntity => roleClaimLinkEntity.Role) : new List<RoleEntity>()))
			.ForMember(x => x.Users, expression => expression.MapFrom(x => x.Users != null ? x.Users.Select(userClaimLinkEntity => userClaimLinkEntity.User) : new List<UserEntity>()));
		CreateMap<ClaimModel, ClaimEntity>()
			.ForMember(x => x.Roles, expression => expression.Ignore())
			.ForMember(x => x.Users, expression => expression.Ignore()) ;
	}
}