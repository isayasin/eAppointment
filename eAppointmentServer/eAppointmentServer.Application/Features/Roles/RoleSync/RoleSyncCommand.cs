using MediatR;
using TS.Result;

namespace eAppointmentServer.Application.Features.Roles.RoleSync;
public sealed record RoleSyncCommand() : IRequest<Result<string>>;
