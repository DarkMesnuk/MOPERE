using Base.Infrastructure.Database.Redis.Repositories;
using Domain.Interfaces.Repositories.Users;
using Domain.Models.Users;
using StackExchange.Redis;

namespace Infrastructure.Database.Redis.Repositories;

public class NewUserEmailsRepository(
    IConnectionMultiplexer redis
) : BaseRedisRepository<NewUserEmailModel>(redis, "NewUserEmail"), INewUserEmailsRepository
{
    protected override TimeSpan Expiry => TimeSpan.FromMinutes(15);   
}