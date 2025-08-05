using EventPlannerRSVPTracker.App.Abstractions.Persistence;
using EventPlannerRSVPTracker.App.Abstractions.Repositories;
using EventPlannerRSVPTracker.App.Abstractions.Services;
using EventPlannerRSVPTracker.App.DTOs;
using EventPlannerRSVPTracker.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EventPlannerRSVPTracker.Database.Services;

public class UsersService : IUsersService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UsersService> _logger;
    private readonly IUserRepository _userRepository;

    public UsersService(IUnitOfWork unitOfWork,
        ILogger<UsersService> logger,
        IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task<ResultModel<Guid>> Login(string username, CancellationToken cancellationToken = default)
    {
        List<ErrorModel> errors = new();

        try
        {
            var cleansedUsername = username.Trim().ToLower();

            if(string.IsNullOrWhiteSpace(cleansedUsername))
            {
                ErrorModel error = new(nameof(ArgumentException), "Username cannot be empty.");

                errors.Add(error);

                return ResultModel<Guid>.Fail(errors);
            }

            var user = await _userRepository.GetByUsername(cleansedUsername, cancellationToken, true);

            if(user is null)
            {
                var newUser = User.Create(cleansedUsername);

                newUser.LastLoginDate = DateTime.UtcNow;

                using var txn = _unitOfWork.BeginTransaction();

                try
                {
                    newUser = await _userRepository.InsertAsync(newUser, cancellationToken);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await txn.CommitAsync(cancellationToken);

                    return ResultModel<Guid>.Success(newUser.Id);
                }
                catch (Exception)
                {
                    await txn.RollbackAsync(cancellationToken);

                    throw;
                }
            }
            else
            {
                user.LastLoginDate = DateTime.UtcNow;

                using var txn = _unitOfWork.BeginTransaction();

                try
                {
                    await _userRepository.UpdateAsync(user, cancellationToken);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await txn.CommitAsync(cancellationToken);

                    return ResultModel<Guid>.Success(user.Id);
                }
                catch (Exception)
                {
                    await txn.RollbackAsync(cancellationToken);

                    throw;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging in user: {Username}", username);

            ErrorModel error = new(nameof(Exception), "Error logging in");

            errors.Add(error);
            
            return ResultModel<Guid>.Fail(errors);
        }
    }
}
