using DSPrima.UserSessionStore.Model;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DSPrima.UserSessionStore.Context
{
    /// <summary>
    /// A DbContext instance that is used as the database store for the SessionStoreData
    /// </summary>
    public class SessionContext : DbContext
    {
        /// <summary>
        /// Gets the Internal Object Context instance
        /// There are quite a few tricks for the EntityFramework that depend on the ObjectContext being available. This should allow their use.
        /// </summary>
        public ObjectContext ObjectContext { get { return ((IObjectContextAdapter)this).ObjectContext; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionContext"/> class
        /// </summary>
        public SessionContext()
            : base("DSPrima.SessionContext")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        /// <summary>
        /// Gets or sets the session store
        /// </summary>
        public DbSet<SessionStoreData> SessionStore { get; set; }

        /// <summary>
        ///  This method is called when the model for a derived context has been initialized,
        ///  but before the model has been locked down and used to initialize the context.
        ///  The default implementation of this method does nothing, but it can be overridden
        ///  in a derived class such that the model can be further configured before it
        ///  is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
