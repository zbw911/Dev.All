namespace Dev.Demo.Mapping
{
    using Dev.Demo.Entities.UserAgg;

    public class UserMapping : EntityMappingBase<User>
    {
        public UserMapping()
        {
            this.Property(x => x.UserName);
            this.Property(x => x.DisplayName);
            this.Property(x => x.Password);
            this.Property(x => x.Email);

            this.ToTable("User");
        }
    }
}