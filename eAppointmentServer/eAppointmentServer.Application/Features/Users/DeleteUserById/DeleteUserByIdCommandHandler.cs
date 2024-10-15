using eAppointmentServer.Domain.Entities;
using eAppointmentServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eAppointmentServer.Application.Features.Users.DeleteUserById;

internal sealed class DeleteUserByIdCommandHandler(UserManager<AppUser> userManager, IUserRoleRepository userRoleRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
    {
        AppUser? appuser = await userManager.FindByIdAsync(request.Id.ToString());

        if (appuser == null)
        {
            return Result<string>.Failure("User is not found!");
        }

        IdentityResult result = await userManager.DeleteAsync(appuser);
        if (!result.Succeeded)
        {
            return Result<string>.Failure(result.Errors.Select(s => s.Description).First());
        }

        List<AppUserRole> userRole = await userRoleRepository.Where(p => p.UserId == request.Id).ToListAsync();
        userRoleRepository.DeleteRange(userRole);
        await unitOfWork.SaveChangesAsync(cancellationToken);



        return "User delete is successful";
    }
}
