namespace Domain.MainBoundedContext.UserProfile.Repository
{
    using System.Collections.Generic;

    using Dev.Data.Infras;

    using Domain.Entities.Models;

    public interface IUserProfileRepository : IRepository<UserProfile>
    {

        string GetPhoneByUserId(int userid);

        void CreatePhonePasswordResetToken(int userid, string code, int tokenExpirationInMinutesFromNow = 1440);
        void ResetTokenCount(string userName);
    }
}
