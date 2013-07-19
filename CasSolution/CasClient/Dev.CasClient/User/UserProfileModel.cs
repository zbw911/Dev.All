namespace Dev.CasClient.User
{
    /// <summary>
    /// </summary>
    public class UserProfileModel
    {
        #region Instance Properties

        /// <summary>
        /// </summary>
        public string Avator { get; set; }

        /// <summary>
        /// </summary>
        public int? City { get; set; }

        /// <summary>
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// </summary>
        public bool? IsConfirmEmail { get; set; }

        /// <summary>
        /// </summary>
        public bool? IsConfirmPhone { get; set; }

        /// <summary>
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// </summary>
        public int? Province { get; set; }

        /// <summary>
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        /// </summary>
        public int? Sex { get; set; }

        /// <summary>
        /// </summary>
        public decimal Uid { get; set; }

        /// <summary>
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// </summary>
        public string UserName { get; set; }

        #endregion

        //public string PhonePasswordResetToken { get; set; }
        //public Nullable<System.DateTime> PhonePasswordResetTokenExpirationDate { get; set; }
        //public Nullable<int> PhonePasswordResendCount { get; set; }
        //public Nullable<System.DateTime> LastPhonePasswordResetTokenTime { get; set; }
        //public virtual ICollection<webpages_UsersInRoles> webpages_UsersInRoles { get; set; }
    }
}