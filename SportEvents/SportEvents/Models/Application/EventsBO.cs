using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportEvents.Models.Application
{
    public class EventsBO
    {
        DataContext db = new DataContext();

        public IQueryable<Group> GetGroupsWhereUserIsAdmin(User user)
        {
            var groups = db.Groups.Where(x => x.CreatorId == user.UserId);

            return groups;
        } 
    }
}