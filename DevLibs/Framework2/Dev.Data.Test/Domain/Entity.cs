namespace Dev.Data.Test.Domain
{
    public abstract class Entity
    {
        #region Public Properties

        public virtual int Id { get; set; }

        #endregion

        #region Public Methods and Operators

        public virtual bool IsTransient()
        {
            return this.Id == default(int);
        }

        #endregion
    }
}