using System;

namespace Domain.MainBoundedContext.UserProfile.Repository
{
    using System.Collections.Generic;
    using System.Data.Entity;

    using Dev.Data;

    using Domain.Entities.Models;

    public class UserProfileRepository : GenericRepository<UserProfile>, IUserProfileRepository
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string.</param>
        public UserProfileRepository(string connectionStringName)
            : base(connectionStringName)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="GenericRepository&lt;TEntity&gt;" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UserProfileRepository(DbContext context)
            : base(context)
        {
        }


        public string GetPhoneByUserId(int userid)
        {
            var user = this.FindOne(x => x.UserId == userid);
            if (user == null)
                return null;
            return user.Phone;
        }

        public void CreatePhonePasswordResetToken(int userid, string code, int tokenExpirationInMinutesFromNow)
        {
            var user = this.FindOne(x => x.UserId == userid);
            if (user == null)
                throw new Exception("不存在的用户");

            user.PhonePasswordResetToken = code;
            user.PhonePasswordResetTokenExpirationDate = System.DateTime.Now.AddMinutes(tokenExpirationInMinutesFromNow);
            user.LastPhonePasswordResetTokenTime = System.DateTime.Now;
            if (user.LastPhonePasswordResetTokenTime == null || user.LastPhonePasswordResetTokenTime.Value.AddHours(1) < System.DateTime.Now)
                user.PhonePasswordResendCount = 0;

            user.PhonePasswordResendCount = user.PhonePasswordResendCount ?? 0;
            user.PhonePasswordResendCount++;
            this.Update(user);
            this.UnitOfWork.SaveChanges();
        }

        public void ResetTokenCount(string userName)
        {
            var user = this.FindOne(x => x.UserName == userName);
            if (user == null)
                throw new Exception("不存在的用户");

            user.PhonePasswordResendCount = 0;
            this.Update(user);
            this.UnitOfWork.SaveChanges();
        }
    }
}