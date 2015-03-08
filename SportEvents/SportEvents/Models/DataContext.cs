using SportEvents.Controllers.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

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
        public DbSet<UsersInGroup> UsersInGroups { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>()
            //    .HasMany(a => a.Groups)
            //    .WithMany()
            //    .Map(x =>
            //    {
            //        x.MapLeftKey("User_Id");
            //        x.MapRightKey("Group_Id");
            //        x.ToTable("GroupsUsers");
            //    });

            modelBuilder.Entity<Event>()
                .HasRequired<Group>(a => a.Group)
                .WithMany(a => a.Events)
                .HasForeignKey(a => a.GrpId);
                

            modelBuilder.Entity<Event>()
                .HasMany(a => a.Users)
                .WithMany()
                .Map(x =>
                {
                    x.MapLeftKey("Event_Id");
                    x.MapRightKey("User_Id");
                    x.ToTable("EventsUsers");
                });
        }

        
        public bool IsEmailInDatabase(string email)
        {
            
            if (Users.Any(x => x.Email == email))
            {
                return true;
            }
            return false;
        }

        public List<Event> AllEventsWhereIsUserCreator(int User_Id)
        {
            
            List<Event> listOfEvents = new List<Event>();
            listOfEvents = Events.Where(x => x.CreatorId == User_Id).ToList();
            
            return listOfEvents;
        }

        public List<Group> AllGroupsWhereIsUserCreator(int User_Id)
        {

            List<Group> listOfGroups = new List<Group>();
            listOfGroups = Groups.Where(x => x.Creator == User_Id).ToList();

            return listOfGroups;
        }

        public List<Group> AllGroupsWhereIsUserMember(int userId)
        {

            List<UsersInGroup> listOfGroups = new List<UsersInGroup>();
            var list = new List<Group>();


            listOfGroups = UsersInGroups.Where(x => x.UserID == userId).ToList();

            foreach (UsersInGroup usersInGroup in listOfGroups)
            {
                Group g = Groups.Find(usersInGroup.GroupID);

                list.Add(g);
            }

            return list;
        }

        public bool IsUserCreatorOfGroup(int user_Id, int grp_Id)
        {
            if (Groups.Any(x => x.Creator == user_Id && x.Id == grp_Id))
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
            User user = (User) Users.Where(x => x.Email == email).Single();
            return user;
        }

        public bool IsUserInGroup(int userId, int groupId)
        {

            if (UsersInGroups.Any(x => x.UserID == userId && x.GroupID == groupId))
            {
                return true;
            }
            return false;
        }

        
    }
}