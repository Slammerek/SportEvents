using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SportEvents.Models.Application
{
    public class GroupsBO
    {
        private DataContext db = new DataContext();

        public List<Group> Index()
        {
            return db.Groups.ToList();
        }

        public List<Group> IndexCreator(int userId)
        {
            return db.AllGroupsWhereIsUserCreator(userId);
        }

        public List<Group> IndexMember(int userId)
        {
            return db.AllGroupsWhereIsUserMember(userId);
        }

        public Group GetGroupById(int? id)
        {
            Group group = db.Groups.Find(id);

            return group;
        }

        public void CreateGroup(Group group, User user)
        {
            group.CreatorId = user.UserId;
            group.CreateTime = DateTime.Now;
            group.StartOfPaymentPeriod = DateTime.Now;
            group.NumberOfUsersInGroup += 1;

            db.Groups.Add(group);
            db.SaveChanges();
        }

        public void AddUserToGroup(Group group, User user)
        {
            if (!db.IsUserInGroup(user.UserId, group.GroupId))
            {
                var groupToBeAddedTo = db.Groups.Find(group.GroupId);
                var userToBeAdded = db.Users.Find(user.UserId);

                groupToBeAddedTo.Users.Add(userToBeAdded);

                db.SaveChanges();
            }
        }

        public void EditGroup(Group group)
        {
            db.Entry(group).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteGroup(Group group)
        {
            db.Groups.Remove(group);
            db.SaveChanges();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}