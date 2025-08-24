using Base.Domain.Infrastructure.Interfaces.Repositories.Redis;
using Domain.Models.Users;

namespace Domain.Interfaces.Repositories.Users;

public interface INewUserEmailsRepository : IBaseRedisRepository<NewUserEmailModel>;