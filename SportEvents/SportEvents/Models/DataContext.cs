using System;
using System.Globalization;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using SportEvents.Controllers.Utility;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SportEvents.Models
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base("SportEventsFork")
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Article> Articles { get; set; }
        public DbSet<UsersInEvent> UsersInEvents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // M:N mezi Group <-> User
            modelBuilder.Entity<Group>()
                .HasMany(a => a.Users)
                .WithMany(c => c.Groups)
                .Map(x => x.MapLeftKey("GroupId").MapRightKey("UserId").ToTable("UsersInGroups"));

            modelBuilder.Entity<Event>()
                .HasRequired<Group>(a => a.Group)
                .WithMany(a => a.Events)
                .HasForeignKey(a => a.GroupId);

//            // M:N mezi Event <-> User
//            modelBuilder.Entity<Event>()
//                .HasMany(a => a.Users)
//                .WithMany(c => c.Events)
//                .Map(x => x.MapLeftKey("EventId").MapRightKey("UserId").ToTable("EventsUsers"));
        }


        public List<User> AllUsersInGroup(int groupId)
        {
            var list = new List<User>();

            list = Groups.Find(groupId).Users.ToList();
            
            return list;
        } 
        public bool IsEmailInDatabase(string email)
        {
            if (Users.Any(x => x.Email == email))
            {
                return true;
            }
            return false;
        }

        public List<Event> AllEventsWhereIsUserCreator(int userId)
        {
            List<Event> listOfEvents = new List<Event>();
            listOfEvents = Events.Where(x => x.CreatorId == userId).ToList();

            return listOfEvents;
        }

        public List<Group> AllGroupsWhereIsUserCreator(int userId)
        {
            List<Group> listOfGroups = new List<Group>();
            listOfGroups = Groups.Where(x => x.CreatorId == userId).ToList();

            return listOfGroups;
        }

        public List<Group> AllGroupsWhereIsUserMember(int userId)
        {
            var listOfGroups = new List<Group>();

            listOfGroups = Users.Find(userId).Groups.ToList();

            return listOfGroups;
        }

        public bool IsUserCreatorOfGroup(int userId, int groupId)
        {
            if (Groups.Any(x => x.CreatorId == userId && x.GroupId == groupId))
            {
                return true;
            }
            return false;
        }

        public string GetHashedPassword(string email)
        {
            return Users.Where(x => x.Email == email).Select(x => x.Password).Single();
        }

        public bool IsUserRegistered(string email, string hashedFormPassword)
        {
            if (IsEmailInDatabase(email) && UtilityMethods.ComparePasswords(hashedFormPassword, GetHashedPassword(email)))
            {
                return true;
            }
            return false;
        }

        public User GetUserByEmail(string email)
        {
            User user = (User)Users.Where(x => x.Email == email).Single();
            return user;
        }

        public bool IsUserInGroup(int userId, int groupId)
        {
            if (Groups.Find(groupId).Users.Any(x => x.UserId == userId))
            {
                return true;
            }

            return false;
        }
    }
}