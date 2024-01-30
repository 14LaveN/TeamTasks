using System.Security.Authentication;
using TeamTasks.Micro.Identity.Models.Identity;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Database.Identity.Data.Interfaces;
using TeamTasks.Domain.Core.Exceptions;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Enumerations;
using TeamTasks.Micro.Identity.Extensions;

namespace TeamTasks.Micro.Identity.Commands.Login;

public sealed class LoginCommandHandler(ILogger<LoginCommandHandler> logger,
        IValidator<LoginCommand> validator,
        IUserUnitOfWork unitOfWork,
        UserManager<User> userManager,
        IOptions<JwtOptions> jwtOptions,
        SignInManager<User> signInManager)
    : IRequestHandler<LoginCommand, LoginResponse<User>>
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    private readonly SignInManager<User> _signInManager = signInManager ?? throw new ArgumentNullException();
    
    public async Task<LoginResponse<User>> Handle(LoginCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"Request for login an account - {request.UserName}");

            var errors = await validator.ValidateAsync(request, cancellationToken);

            if (errors.Errors.Count is not 0)
            {
                throw new ValidationException($"You have errors - {errors.Errors}");
            }
            
            var user = await userManager.FindByNameAsync(request.UserName);

            if (user is null)
            {
                logger.LogWarning("User with the same name not found");
                throw new NotFoundException(nameof(user), "User with the same name");
            }

            if (!await userManager.CheckPasswordAsync(user, request.Password))
            {
                logger.LogWarning("The password does not meet the assessment criteria");
                throw new AuthenticationException();
            }
            
            var result = await _signInManager.PasswordSignInAsync(request.UserName,
                request.Password, false, false);

            var (refreshToken, refreshTokenExpireAt) = user.GenerateRefreshToken(_jwtOptions);
            
            if (result.Succeeded)
            {
                user.RefreshToken = refreshToken;
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
            
            logger.LogInformation($"User logged in - {user.UserName} {DateTime.UtcNow}");
        
            return new LoginResponse<User>
            {
                Description = "Login account",
                StatusCode = StatusCode.Ok,
                Data = user,
                AccessToken = user.GenerateAccessToken(_jwtOptions), 
                RefreshToken = refreshToken,
                RefreshTokenExpireAt = refreshTokenExpireAt
            };
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"[LoginCommandHandler]: {exception.Message}");
            throw new AuthenticationException(exception.Message);
        }
    }
}