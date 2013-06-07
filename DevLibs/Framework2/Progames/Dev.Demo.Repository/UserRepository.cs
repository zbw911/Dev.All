namespace Dev.Demo.Repository
{
    using System;
    using System.Data.Entity;

    using Dev.Data;
    using Dev.Demo.Entities.UserAgg;

    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        //private readonly IEncrypting _encryptor;

        //public UserRepository(IEncrypting encryptor)
        //    : this(DependencyResolver.Current.GetService<MyDbContext>(), encryptor)
        //{
        //}

        //public UserRepository(MyDbContext context, IEncrypting encryptor)
        //    : base(context)
        //{
        //    _encryptor = encryptor;
        //}

        #region Public Methods and Operators

        /// <summary>
        ///     Initializes a new instance of the <see cref="Repository&lt;TEntity&gt;" /> class.
        /// </summary>
        //public GenericRepository()
        //    : this(string.Empty)
        //{
        //}

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public UserRepository(string connectionStringName)
            : base(connectionStringName)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UserRepository(DbContext context)
            : base(context)
        {
        }


        public int CreateUser(
            string userName, string displayName, string password, string email, int role, string createdBy)
        {
            string hashPassword = ""; // _encryptor.Encode(password);

            return this.Save(UserFactory.Create(userName, displayName, hashPassword, email, role, createdBy)).Id;
        }

        public User GetUserByUserName(string userName)
        {
            return this.FindOne(new Specification.UserByNameSpecification(userName));
            return this.FindOne<User>(x => x.UserName.Equals(userName, StringComparison.InvariantCulture));
        }

        public bool ValidateUser(string userName, string password)
        {
            User user = this.GetUserByUserName(userName);

            if (user == null)
            {
                return false;
            }

            return user.UserName.Equals(userName, StringComparison.InvariantCulture)
                   && user.Password.Equals(password, StringComparison.InvariantCulture);
        }

        #endregion

        //public UserInfo GetUserInfoByUserName(string userName)
        //{
        //    return this.GetUserByUserName(userName).MapTo<UserInfo>();
        //}
    }
}