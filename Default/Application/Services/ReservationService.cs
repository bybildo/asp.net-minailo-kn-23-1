using Default.Application.DTOs.Requests;
using Default.Application.Exceptions;
using Default.Application.Exeptions;
using Default.Infrastructure;
using Default.Models;
using Default.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace Default.Application.Services
{
    public class ReservationService
    {
        private readonly AppDbContext _context;

        public ReservationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddReservation(AddReservationRequest request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            foreach (var tableId in request.TableIds)
            {
                var table = await _context.Tables.FindAsync(tableId);
                if (table == null)
                {
                    throw new NotFoundException("Table with id " + tableId + " not found");
                }
            }

            var conflictExists = await _context.Reservations
                .Where(r => r.StartDate < request.EndDate && r.EndDate > request.StartDate)
                .AnyAsync(r => r.Tables.Any(t => request.TableIds.Contains(t.Id)));

            if (conflictExists)
                throw new IncorrectDataEnteredException("One or more tables are already reserved for this time.");

            var tables = await _context.Tables.Where(t => request.TableIds.Contains(t.Id)).ToListAsync();

            var reservation = new Reservation
            {
                UserId = (Guid)request.UserId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Tables = tables
            };

            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReservation(Guid reservationId)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);

            if (reservation == null)
                throw new NotFoundException("Reservation not found");

            _context.Reservations.Remove(reservation);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateReservation(UpdateReservationRequest request)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Tables)
                .FirstOrDefaultAsync(r => r.Id == request.ReservationId);

            if (reservation == null)
                throw new NotFoundException("Reservation not found");

            var table = await _context.Tables.FindAsync(request.TableId);

            if (table == null)
                throw new NotFoundException("Table not found");

            if (!Enum.TryParse(request.Operation, true, out Operation operation) || !Enum.IsDefined(typeof(Operation), operation))
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

            await _context.SaveChangesAsync();
        }

        public async Task<List<Reservation>> GetAllReservations()
        { 
            return await _context.Reservations
            .Include(r => r.Tables)
            .Include(r => r.User)
            .ToListAsync();
        }

        public async Task<Reservation?> GetReservationById(Guid id)
        {
            return await _context.Reservations
                .Include(r => r.Tables)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
