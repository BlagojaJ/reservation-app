using Reservation.App.Application.Contracts.Persistence;
using Reservation.App.Domain.Entities;

namespace Reservation.App.Infrastructure.Persistence.Repositories;

public class AgentRepository(ReservationDbContext dbContext)
    : BaseRepository<Agent>(dbContext),
        IAgentRepository { }
