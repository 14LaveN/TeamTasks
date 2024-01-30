using System.Security.Authentication;
using TeamTasks.Micro.Identity.Commands.Login;
using TeamTasks.Micro.Identity.Models.Identity;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Quartz.Util;
using TeamTasks.Database.Common.Abstractions;
using TeamTasks.Database.Identity.Data.Interfaces;
using TeamTasks.Domain.Core.Exceptions;
using TeamTasks.Domain.Core.Primitives.Result;
using TeamTasks.Domain.Entities;
using TeamTasks.Domain.Enumerations;
using TeamTasks.Domain.ValueObjects;
using TeamTasks.Email.Emails;

namespace TeamTasks.Micro.Identity.Commands.Register;

internal class RegisterCommandHandler(ILogger<RegisterCommandHandler> logger,
        IValidator<RegisterCommand> validator,
        UserManager<User> userManager,
        ISender mediator,
        IEmailService emailService,
        SignInManager<User> signInManager)
    : IRequestHandler<RegisterCommand, LoginResponse<User>>
{
    private readonly SignInManager<User> _signInManager = signInManager ?? throw new ArgumentNullException();
    
    public async Task<LoginResponse<User>> Handle(RegisterCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"Request for login an account - {request.UserName} {request.LastName}");
            
            var errors = await validator.ValidateAsync(request, cancellationToken);

            if (errors.Errors.Count is not 0)
            {
                logger.LogWarning($"You have errors - {errors.Errors}");
                throw new ValidationException($"You have errors - {errors.Errors}");
            }
            
            Result<FirstName> firstNameResult = FirstName.Create(request.FirstName);
            Result<LastName> lastNameResult = LastName.Create(request.LastName);
            Result<Domain.ValueObjects.EmailAddress> emailResult = Domain.ValueObjects.EmailAddress.Create(request.Email);
            Result<Password> passwordResult = Password.Create(request.Password);
            
            var user = await userManager.FindByNameAsync(request.UserName);

            if (user is not null)
            {
                logger.LogWarning("User with the same name already taken");
                throw new NotFoundException(nameof(user), "User with the same name");
            }

            user = User.Create(firstNameResult.Value, lastNameResult.Value, emailResult.Value, passwordResult.Value);
            
            var result = await userManager.CreateAsync(user, request.Password);
            
            LoginResponse<User> loginResponse = new LoginResponse<User>
            {
                Description = "",
                StatusCode = StatusCode.TaskIsHasAlready
            };

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                loginResponse = await mediator.Send(new LoginCommand()
                {
                    UserName = request.UserName,
                    Password = request.Password
                }, cancellationToken);

                 if (!user.EmailAddress.Value.IsNullOrWhiteSpace() && user.UserName is not null)
                     await emailService.SendEmailAsync(new( user.EmailAddress,
                         user.UserName,
                         "You authorized to TeamTasks"));

                logger.LogInformation($"User authorized - {user.UserName} {DateTime.UtcNow}");
            }
            return new LoginResponse<User>
            {
                Description = "Register account",
                StatusCode = StatusCode.Ok,
                Data = user,
                AccessToken = loginResponse.AccessToken, 
                RefreshToken = loginResponse.RefreshToken,
                RefreshTokenExpireAt = loginResponse.RefreshTokenExpireAt
            };
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"[RegisterCommandHandler]: {exception.Message}");
            throw new AuthenticationException(exception.Message);
        }
    }
}