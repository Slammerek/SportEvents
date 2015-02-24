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
            : base("SportEventsDB")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }

       

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(a => a.Groups)
                .WithMany()
                .Map(x =>
                {
                    x.MapLeftKey("User_Id");
                    x.MapRightKey("Group_Id");
                    x.ToTable("GroupsUsers");
                });

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

        public bool IsUserCreatorOfGroup(int User_Id, int Grp_Id)
        {
            if (Groups.Any(x => x.Creator == User_Id && x.Id == Grp_Id))
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

        public System.Data.Entity.DbSet<SportEvents.Models.Event> Events { get; set; }
    }
}