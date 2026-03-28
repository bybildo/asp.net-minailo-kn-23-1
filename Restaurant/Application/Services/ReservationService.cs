using Application.DTOs.Responses;
using Restaurant.Application.DTOs.Requests;
using Restaurant.Application.Exceptions;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;

namespace Restaurant.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITableRepository _tableRepository;

        public ReservationService(
            IReservationRepository reservationRepository,
            IUserRepository userRepository,
            ITableRepository tableRepository)
        {
            _reservationRepository = reservationRepository;
            _userRepository = userRepository;
            _tableRepository = tableRepository;
        }

        public async Task AddReservation(AddReservationRequest request)
        {
            var user = await _userRepository.GetByIdAsync((Guid)request.UserId!);
            if (user == null)
                throw new NotFoundException("User not found");

            foreach (var tableId in request.TableIds)
            {
                var table = await _tableRepository.GetByIdAsync(tableId);
                if (table == null)
                    throw new NotFoundException($"Table with id {tableId} not found");
            }

            var conflictExists = await _reservationRepository
                .HasConflictAsync(request.StartDate, request.EndDate, request.TableIds);

            if (conflictExists)
                throw new IncorrectDataEnteredException("One or more tables are already reserved for this time.");

            var tables = await _tableRepository.GetByIdsAsync(request.TableIds);

            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                UserId = (Guid)request.UserId!,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Tables = tables
            };

            await _reservationRepository.AddAsync(reservation);
            await _reservationRepository.SaveChangesAsync();
        }

        public async Task UpdateReservation(UpdateReservationRequest request)
        {
            var reservation = await _reservationRepository.GetByIdWithTablesAsync(request.ReservationId);
            if (reservation == null)
                throw new NotFoundException("Reservation not found");

            var table = await _tableRepository.GetByIdAsync(request.TableId);
            if (table == null)
                throw new NotFoundException("Table not found");

            if (!Enum.TryParse(request.Operation, true, out Operation operation)
                || !Enum.IsDefined(typeof(Operation), operation))
                throw new IncorrectDataEnteredException($"Operation '{request.Operation}' is invalid.");

            if (operation == Operation.Add)
            {
                if (reservation.Tables.Any(t => t.Id == request.TableId))
                    throw new IncorrectDataEnteredException("Table already added to reservation");

                reservation.Tables.Add(table);
            }

            if (operation == Operation.Remove)
            {
                if (!reservation.Tables.Any(t => t.Id == request.TableId))
                    throw new IncorrectDataEnteredException("Table not found in reservation");

                reservation.Tables.Remove(table);
            }

            await _reservationRepository.SaveChangesAsync();
        }

        public async Task DeleteReservation(Guid reservationId)
        {
            var reservation = await _reservationRepository.GetByIdAsync(reservationId);
            if (reservation == null)
                throw new NotFoundException("Reservation not found");

            _reservationRepository.Remove(reservation);
            await _reservationRepository.SaveChangesAsync();
        }

        public async Task<List<ReservationResponse>> GetAllReservations()
        {
            var reservations = await _reservationRepository.GetAllWithDetailsAsync() ?? new List<Reservation>();
            return reservations.Select(r => new ReservationResponse(r)).ToList();
        }

        public async Task<ReservationResponse?> GetReservationById(Guid id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            return reservation == null ? null : new ReservationResponse(reservation);
        }
    }
}
