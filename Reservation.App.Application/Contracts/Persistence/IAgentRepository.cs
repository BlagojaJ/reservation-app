using Reservation.App.Domain.Entities;

namespace Reservation.App.Application.Contracts.Persistence;

public interface IAgentRepository : IAsyncRepository<Agent> { }
