using Identity.API.Requests.Authentication;
using Identity.API.Requests.Authentication.Admin;
using Application.Handlers.Authentication;
using Application.Handlers.Authentication.Admin;
using Base.API.Extensions;

namespace Identity.API.Mappings;

public partial class RequestsMappings
{
    private void CreateMapAuthentication()
    {
        CreateMap<LoginRequest, LoginCommandRequest>();
        CreateMap<RefreshTokenRequest, RefreshTokenCommandRequest>();
        
        CreateMap<RegisterRequest, RegisterCommandRequest>();
        CreateMap<ConfirmRegistrationRequest, ConfirmRegistrationCommandRequest>();
        
        CreateMap<ResendCodeRequest, ResendCodeCommandRequest>();
        CreateMap<IsValidCodeRequest, IsValidCodeCommandRequest>();
        
        CreateMap<ForgotPasswordRequest, ForgotPasswordCommandRequest>();
        CreateMap<ResetPasswordRequest, ResetPasswordCommandRequest>();
        
        CreateMap<RestoreUserRequest, RestoreUserCommandRequest>();
        CreateMap<ConfirmRestoreUserRequest, ConfirmRestoreUserCommandRequest>();
        
        this.CreateAuthMap<ChangeEmailRequest, ChangeEmailCommandRequest>();
        this.CreateAuthMap<ConfirmChangeEmailRequest, ConfirmChangeEmailCommandRequest>();
        this.CreateAuthMap<ConfirmNewEmailRequest, ConfirmNewEmailCommandRequest>();
        
        this.CreateAuthMap<ChangePasswordRequest, ChangePasswordCommandRequest>();
        
        this.CreateAuthMap<BlockingUserRequest, BlockingUserCommandRequest>();
    }
}