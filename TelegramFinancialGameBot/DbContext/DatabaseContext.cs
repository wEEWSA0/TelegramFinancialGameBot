using Microsoft.EntityFrameworkCore;
using TelegramFinancialGameBot.Model;

namespace TelegramFinancialGameBot.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasKey(u => new { u.AccountChatId, u.RoomId });
        }*/

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Buisness> Buisnesses { get; set; }
        public virtual DbSet<Dream> Dreams { get; set; }
        public virtual DbSet<FinancialDirector> FinancialDirectors { get; set; }
        public virtual DbSet<GeneralDirector> GeneralDirectors { get; set; }
        public virtual DbSet<Knowledge> Knowledges { get; set; }
        public virtual DbSet<ManagerStaff> ManagerStaffs { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }
        public virtual DbSet<UserBuisness> UserBuisnesses { get; set; }
        public virtual DbSet<UserKnowledge> UserKnowledges { get; set; }
        public virtual DbSet<UserProperty> UserProperties { get; set; }
        public virtual DbSet<Work> Works { get; set; }
        public virtual DbSet<WorkPosition> WorkPositions { get; set; }

        public virtual DbSet<SetupCharacter> SetupCharacters { get; set; }
        public virtual DbSet<UserWorkPosition> UserWorkPositions { get; set; }
        public virtual DbSet<RegionalDirector> RegionalDirectors { get; set; }
        public virtual DbSet<BuisnessRegionalDirector> BuisnessRegionalDirectors { get; set; }
        public virtual DbSet<BuisnessManagerStaff> BuisnessManagerStaffs { get; set; }
        public virtual DbSet<UserStaff> UserStaffs { get; set; }

        public virtual DbSet<UserDreamExpectation> UserDreamExpectations { get; set; }
        public virtual DbSet<Accident> Accidents { get; set; }
        public virtual DbSet<UserAccident> UserAccidents { get; set; }
    }
}
