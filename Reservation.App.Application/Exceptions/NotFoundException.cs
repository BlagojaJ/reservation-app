namespace Reservation.App.Application.Exceptions;

public class NotFoundException(string name, object key)
    : Exception($"{name} with ID {key} could not be found") { }
