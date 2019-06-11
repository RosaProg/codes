using PCHI.DataAccessLibrary.Context;
using PCHI.Model.Questionnaire;
using PCHI.Model.Questionnaire.Styling.Definition.Elements;
using PCHI.Model.Questionnaire.Styling.Definition.ItemGroupOptions;
using PCHI.Model.Questionnaire.Styling.Definition.Sections;
using PCHI.Model.Questionnaire.Styling.Presentation;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace PCHI.DataAccessLibrary.AccessHandelers
{
    /// <summary>
    /// Handles the data access for Questionnaire Formats
    /// </summary>
    public class QuestionnaireFormatAccessHandler
    {
        /// <summary>
        /// The Main Database context to use
        /// </summary>
        private MainDatabaseContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionnaireFormatAccessHandler"/> class
        /// </summary>
        /// <param name="context">The <see cref="MainDatabaseContext"/> instance to use</param>
        internal QuestionnaireFormatAccessHandler(MainDatabaseContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adds or updates a full Questionnaire Format Container Definition and all children and references
        /// </summary>
        /// <param name="container">The container to add</param>
        public void AddOrUpdateFullDefinitionContainer(ContainerFormatDefinition container)
        {
            this.context.ContainerFormatDefinitions.Add(container);
            this.CheckContainerDefinitionStatus(container);
            this.context.SaveChanges();
        }

        /// <summary>
        /// Checks the status of the recently added ContainerFormatDefinition and it's children to ensure all are added/updated as they should be
        /// </summary>
        /// <param name="container">The container to add or update</param>
        private void CheckContainerDefinitionStatus(ContainerFormatDefinition container)
        {
            if (this.context.ContainerFormatDefinitions.Any(c => c.ContainerDefinitionName == container.ContainerDefinitionName))
            {
                this.context.Entry(container).State = EntityState.Modified;
            }

            foreach (ElementFormatDefinition elementFormat in container.ElementFormatDefinitions)
            {
                elementFormat.ContainerDefinitionName = container.ContainerDefinitionName;
                if (this.context.ElementFormatDefinitions.Any(e => e.ElementFormatDefinitionName == elementFormat.ElementFormatDefinitionName))
                {
                    this.context.Entry(elementFormat).State = EntityState.Modified;
                }

                if (elementFormat.GetType() == typeof(ItemFormatDefinition))
                {
                    ItemFormatDefinition item = (ItemFormatDefinition)elementFormat;
                    foreach (ItemGroupOptionsFormatDefinition optionsFormat in item.ItemGroupOptionsFormatDefinitions)
                    {
                        optionsFormat.ItemFormatDefinitionName = item.ElementFormatDefinitionName;
                        if (this.context.ItemGroupOptionsFormatDefinitions.Any(i => i.GroupOptionDefinitionName == optionsFormat.GroupOptionDefinitionName))
                        {
                            this.context.Entry(optionsFormat).State = EntityState.Modified;
                        }

                        List<ItemGroupOptionsForEachOptionDefinition> tmp = new List<ItemGroupOptionsForEachOptionDefinition>(optionsFormat.ForEachOption);
                        optionsFormat.ForEachOption = tmp;

                        // Delete the old options before adding the new ones                        
                        this.context.ItemGroupOptionsForEachOptionDefinitions.Where(o => o.ItemGroupOptionsFormatDefinition.GroupOptionDefinitionName == optionsFormat.GroupOptionDefinitionName).ToList().ForEach(o => this.context.Entry(o).State = EntityState.Deleted);

                        int forEachOrder = 0;
                        foreach (ItemGroupOptionsForEachOptionDefinition forEachOptionText in optionsFormat.ForEachOption)
                        {
                            forEachOptionText.OrderInItemGroupOptionsFormatDefinition = forEachOrder++;
                            forEachOptionText.ItemGroupOptionsFormatDefinition = optionsFormat;
                        }
                    }
                }
            }

            foreach (ContainerFormatDefinition child in container.ChildContainers)
            {
                child.ParentContainer = container;
                this.CheckContainerDefinitionStatus(child);
            }
        }

        /// <summary>
        /// Adds a full <see cref="Format"/> and all it's children to the database.
        /// Does NOT store the referenced definitions classes
        /// </summary>
        /// <param name="format">The <see cref="Format"/> to store</param>
        public void AddOrUpdateFullFormat(Format format)
        {
            this.context.Configuration.AutoDetectChangesEnabled = false;
            bool exists = false;

            if (this.context.QuestionnaireFormats.Any(f => f.Name == format.Name && (f.SupportedPlatform & format.SupportedPlatform) == format.SupportedPlatform))
            {
                exists = true;
                foreach (FormatContainer c in this.context.QuestionnaireFormatContainers.Where(c => c.QuestionnaireFormat.Name == format.Name && (c.QuestionnaireFormat.SupportedPlatform & format.SupportedPlatform) == format.SupportedPlatform))
                {
                    this.CascadeDeleteContainer(c);
                }
            }

            int orderInFormat = 0;
            foreach (FormatContainer container in format.Containers)
            {
                container.OrderInParent = orderInFormat++;
                container.QuestionnaireFormat = format;
                this.AddFormatContainersRecursive(container);
            }

            // Set all local definitions to Unchanged so they don't get added to the DB            
            this.context.ContainerFormatDefinitions.Local.ToList().ForEach(o => this.context.Entry(o).State = EntityState.Detached);
            this.context.ElementFormatDefinitions.Local.ToList().ForEach(o => this.context.Entry(o).State = EntityState.Detached);
            this.context.ItemGroupOptionsForEachOptionDefinitions.Local.ToList().ForEach(o => this.context.Entry(o).State = EntityState.Detached);
            this.context.ItemGroupOptionsFormatDefinitions.Local.ToList().ForEach(o => this.context.Entry(o).State = EntityState.Detached);

            if (exists)
            {
                this.context.QuestionnaireFormats.Attach(format);
            }
            else
            {
                this.context.QuestionnaireFormats.Add(format);
            }

            try
            {
                this.context.SaveChanges();
            }
            catch (Exception ex)
            {
                var x = ex;
                var a = x;
            }
        }

        /// <summary>
        /// Cascade deletes a container and marks it and it's children as deleted.
        /// Does NOT update the database but only markts them inside the current context.
        /// </summary>
        /// <param name="container">The root of the container series to delete</param>
        private void CascadeDeleteContainer(FormatContainer container)
        {
            foreach (FormatContainer child in this.context.QuestionnaireFormatContainers.Where(c => c.Parent.Id == container.Id))
            {
                this.CascadeDeleteContainer(child);
            }

            this.context.Entry(container).State = EntityState.Deleted;
        }

        /// <summary>
        /// Adds a format container and it's children (but not Definition References) to the database recursively.
        /// Does not call context.SaveChanges()
        /// </summary>
        /// <param name="container">The container to add</param>
        private void AddFormatContainersRecursive(FormatContainer container)
        {
            container.ContainerFormatDefinition_Name = container.ContainerFormatDefinition == null ? container.ContainerFormatDefinition_Name : container.ContainerFormatDefinition.ContainerDefinitionName;
            container.ContainerFormatDefinition = null;

            if (container.GetType() == typeof(TextFormatContainer))
            {
                TextFormatContainer tf = (TextFormatContainer)container;
                tf.TextFormatDefinition_Name = tf.TextFormatDefinition.ElementFormatDefinitionName == null ? tf.TextFormatDefinition_Name : tf.TextFormatDefinition.ElementFormatDefinitionName;
                tf.TextFormatDefinition = null;

                int orderInSection = 0;
                foreach (FormatContainerElement element in tf.Elements)
                {
                    element.OrderInSection = orderInSection++;
                    element.FormatContainer = tf;
                    this.context.QuestionnaireFormatContainerElements.Add(element);
                }
            }
            else if (container.GetType() == typeof(ItemFormatContainer))
            {
                ItemFormatContainer ifc = (ItemFormatContainer)container;

                ifc.ItemFormatDefinition_Name = ifc.ItemFormatDefinition.ElementFormatDefinitionName == null ? ifc.ItemFormatDefinition_Name : ifc.ItemFormatDefinition.ElementFormatDefinitionName;
                ifc.ItemFormatDefinition = null;

                foreach (ItemGroupFormat igf in ifc.ItemGroupFormats)
                {
                    igf.ItemFormatContainer = ifc;

                    igf.ItemGroupOptionsFormatDefinition_Name = igf.ItemGroupOptionsFormatDefinition == null ? igf.ItemGroupOptionsFormatDefinition_Name : igf.ItemGroupOptionsFormatDefinition.GroupOptionDefinitionName;
                    igf.ItemGroupOptionsFormatDefinition = null;

                    this.context.ItemGroupFormats.Add(igf);
                }

                int orderInSection = 0;
                foreach (FormatContainerElement item in ifc.Elements)
                {
                    item.OrderInSection = orderInSection++;
                    item.FormatContainer = ifc;
                    this.context.QuestionnaireFormatContainerElements.Add(item);
                }
            }

            int orderInParent = 0;
            foreach (FormatContainer child in container.Children)
            {
                child.Parent = container;
                child.OrderInParent = orderInParent++;
                this.AddFormatContainersRecursive(child);
            }

            this.context.QuestionnaireFormatContainers.Add(container);
        }

        /// <summary>
        /// Returns a full format with the given name for a Questionnaire
        /// </summary>
        /// <param name="formatName">The name of the format</param>
        /// <param name="platform">The platform to load the format for</param>
        /// <returns>The full format</returns>
        public Format GetFullFormatByName(string formatName, Platform platform)
        {
            Format format = this.context.QuestionnaireFormats.Where(f => f.Name == formatName && (f.SupportedPlatform & platform) == platform).Include(f => f.Containers).SingleOrDefault();
            List<FormatContainer> containers = new List<FormatContainer>();
            format.Containers = format.Containers.OrderBy(c => c.OrderInParent).ToList();
            foreach (FormatContainer container in format.Containers)
            {
                containers.Add(this.GetContainer(container.Id));
            }

            format.Containers = containers;
            return format;
        }

        /// <summary>
        /// Loads the Format Container with the given Id recursively
        /// </summary>
        /// <param name="formatContainerId">The Id of the format container to load</param>
        /// <returns>The requested format container</returns>
        private FormatContainer GetContainer(int formatContainerId)
        {
            FormatContainer root = this.context.QuestionnaireFormatContainers.Where(c => c.Id == formatContainerId).Include(c => c.ContainerFormatDefinition).Include(c => c.Children).SingleOrDefault();
            List<FormatContainer> children = root.Children.ToList();
            root.Children = root.Children.OrderBy(c => c.OrderInParent).ToList();
            for (int i = 0; i < children.Count; i++)
            {
                children[i] = this.GetContainer(children[i].Id);
            }

            root.Children = children;
            root.Elements = this.context.QuestionnaireFormatContainers.Where(c => c.Id == root.Id).Select(c => c.Elements).Single();
            root.Elements = root.Elements.OrderBy(e => e.OrderInSection).ToList();
            if (root.GetType() == typeof(TextFormatContainer))
            {
                TextFormatContainer textFormatRoot = (TextFormatContainer)root;
                textFormatRoot.TextFormatDefinition = this.context.QuestionnaireFormatContainers.OfType<TextFormatContainer>().Where(c => c.Id == textFormatRoot.Id).Select(c => c.TextFormatDefinition).SingleOrDefault();
            }
            else if (root.GetType() == typeof(ItemFormatContainer))
            {
                ItemFormatContainer ifRoot = (ItemFormatContainer)root;
                ifRoot.ItemFormatDefinition = this.context.QuestionnaireFormatContainers.OfType<ItemFormatContainer>().Where(c => c.Id == ifRoot.Id).Select(c => c.ItemFormatDefinition).SingleOrDefault();
                ifRoot.ItemGroupFormats = this.context.QuestionnaireFormatContainers.OfType<ItemFormatContainer>().Where(c => c.Id == ifRoot.Id).Select(c => c.ItemGroupFormats).Single();
                foreach (ItemGroupFormat igf in ifRoot.ItemGroupFormats)
                {
                    igf.ItemGroupOptionsFormatDefinition = this.context.ItemGroupFormats.Where(f => f.Id == igf.Id).Select(f => f.ItemGroupOptionsFormatDefinition).Single();
                    igf.ItemGroupOptionsFormatDefinition.ForEachOption = this.context.ItemGroupOptionsForEachOptionDefinitions.Where(o => o.ItemGroupOptionsFormatDefinition.GroupOptionDefinitionName == igf.ItemGroupOptionsFormatDefinition.GroupOptionDefinitionName).OrderBy(o => o.OrderInItemGroupOptionsFormatDefinition).ToList();
                }
            }

            return root;
        }

        /// <summary>
        /// Gets a list of all Formats in the database
        /// </summary>
        /// <returns>A list of all the formats</returns>
        public List<Format> GetAllQuestionnaireFormats()
        {
            return this.context.QuestionnaireFormats.ToList();
        }
    }
}
