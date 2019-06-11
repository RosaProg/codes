using PCHI.DataAccessLibrary.Context;
using PCHI.Model.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCHI.DataAccessLibrary.AccessHandelers
{
    /// <summary>
    /// Handles restoring and retrieving of Tags
    /// </summary>
    public class TagAccessHandler
    {
        /// <summary>
        /// The Main Database context to use
        /// </summary>
        private MainDatabaseContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagAccessHandler"/> class
        /// </summary>
        /// <param name="context">The <see cref="MainDatabaseContext"/> instance to use</param>
        internal TagAccessHandler(MainDatabaseContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adds a tag to the database
        /// </summary>
        /// <param name="tag">The tag to add</param>
        public void AddTag(Tag tag)
        {
            if (!this.context.Tags.Any(t => t.TagName == tag.TagName && t.Value == tag.Value))
            {
                this.context.Tags.Add(tag);
                this.context.SaveChanges();
            }
        }

        /// <summary>
        /// Removes a tag from the database
        /// </summary>
        /// <param name="tag">The tag to remove</param>
        public void RemoveTag(Tag tag)
        {
            this.context.Entry(tag).State = System.Data.Entity.EntityState.Deleted;
            this.context.SaveChanges();
        }

        /// <summary>
        /// Gets a tag from the database
        /// </summary>
        /// <param name="name">Name of the tag to be retrieved</param>        
        /// <returns>The Tag found or null</returns>
        public List<Tag> GetTags(string name)
        {
            return this.context.Tags.Where(tg => tg.TagName == name).ToList();
        }
    }
}
