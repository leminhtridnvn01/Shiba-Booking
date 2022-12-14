using Booking.Domain;
using Booking.Domain.Entities;
using Booking.Domain.Interfaces.Repositories.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Infrastructure.Data.Repositories.Rooms
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Room> GetAsync(int roomId, int locationId)
        {
            return await GetAsync(_ => _.Id == roomId
                    && !_.IsDelete
                    && _.LocationId == locationId);
        }

        public async Task<Room> GetAsync(int roomId)
        {
            return await GetAsync(_ => _.Id == roomId
                    && !_.IsDelete);
        }

        public IQueryable<Room> GetByFilter(int? locationId
            , string? name
            , int? fromCapacity
            , int? toCapacity
            , int? fromPrice
            , int? toPrice)
        {
            return GetQuery(_ => (name == null
                                   || _.Name.ToLower().Contains(name.ToLower())
                                 )
                                 && (!locationId.HasValue 
                                   || _.LocationId == locationId.Value 
                                 )
                                 && (!fromCapacity.HasValue
                                   || _.Capacity >= fromCapacity.Value
                                 )
                                 && (!toCapacity.HasValue
                                   || _.Capacity <= toCapacity.Value
                                 )
                                 && (!fromPrice.HasValue
                                   || _.Price >= fromPrice.Value
                                 )
                                 && (!toPrice.HasValue
                                   || _.Price >= toPrice.Value
                                 )
                            );
        }

        public async Task<bool> IsExistsNameRoom(string name)
        {
            return await AnyAsync(_ => _.Name == name && !_.IsDelete);
        }
    }
}
