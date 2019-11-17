using BusinessObject;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace BusinessLogic
{
    public class ProjectBL
    {
        private IHostingEnvironment _hostingEnvironment;
        private static string categoryFilePath;
        private static string projectFilePath;

        /// <summary>
        /// Project Business Logic Constuctor
        /// </summary>
        /// <param name="environment"></param>
        public ProjectBL(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
            categoryFilePath = _hostingEnvironment.WebRootPath + "//Category//CategoryTable.xml";
            projectFilePath = _hostingEnvironment.WebRootPath + "//Project//ProjectTable.xml";
        }

        /// <summary>
        /// Create New or update existing project
        /// </summary>
        /// <param name="projectObject"></param>
        /// <returns></returns>
        public int SaveUpdateProject(ProjectObject projectObject)
        {
            if (projectObject.ProjectId > 0)
            {
                UpdateProject(projectObject);
            }
            else if (projectObject.ProjectId == 0)
            {
                CreateXML(projectObject);
            }
            return 1;
        }

        /// <summary>
        /// Update Existing Project
        /// </summary>
        /// <param name="projectObject"></param>
        public void UpdateProject(ProjectObject projectObject)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(projectFilePath);
                var items = (from item in xmlDoc.Descendants("Project") select item).ToList();
                XElement selected = items.Where(p => p.Element("ProjectId").Value == projectObject.ProjectId.ToString()).FirstOrDefault();
                selected.Remove();
                xmlDoc.Save(projectFilePath);
                xmlDoc.Element("Projects").Add(new XElement("Project",
                    new XElement("ProjectId", projectObject.ProjectId)
                    , new XElement("Description", projectObject.Description)
                    , new XElement("Impact", projectObject.Impact)
                    , new XElement("Causes", projectObject.Causes)
                    , new XElement("CentreforPuralAction", projectObject.CentreforPuralAction)
                    , new XElement("Requirements", projectObject.Requirements)
                    , new XElement("SkillsNeeded", projectObject.SkillsNeeded)
                    , new XElement("Duration", projectObject.Duration)
                    , new XElement("TimeCommitment", projectObject.TimeCommitment)
                    , new XElement("TimePeriod", projectObject.TimePeriod)
                    , new XElement("CategoryName", projectObject.CategoryName)));
                xmlDoc.Save((projectFilePath));
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite(ex.ToString());
            }


        }

        /// <summary>
        /// Delete Existing Project
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public int DeleteProject(int ProjectId)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load((projectFilePath));
                var items = (from item in xmlDoc.Descendants("Project") select item).ToList();
                XElement selected = items.Where(p => p.Element("ProjectId").Value == ProjectId.ToString()).FirstOrDefault();
                selected.Remove();
                xmlDoc.Save(projectFilePath);
                return 1;
            }

            catch (Exception ex)
            {
                LogWriter.LogWrite(ex.ToString());
                return 0;
            }

        }

        /// <summary>
        /// Create New XML
        /// </summary>
        /// <param name="projectObject"></param>
        private void CreateXML(ProjectObject projectObject)
        {
            try
            {
                //Checking if there is any existing project then finding the id and save accordingly
                XmlDocument oXmlDocument = new XmlDocument();
                oXmlDocument.Load(projectFilePath);
                XmlNodeList nodelist = oXmlDocument.GetElementsByTagName("Project");
                var x = oXmlDocument.GetElementsByTagName("ProjectId");
                int Max = 0;
                foreach (XmlElement item in x)
                {
                    int EId = Convert.ToInt32(item.InnerText.ToString());
                    if (EId > Max)
                    {
                        Max = EId;
                    }
                }
                Max = Max + 1;
                XDocument xmlDoc = XDocument.Load(projectFilePath);
                xmlDoc.Element("Projects").Add(new XElement("Project"
                    , new XElement("ProjectId", Max)
                    , new XElement("Description", projectObject.Description)
                    , new XElement("Impact", projectObject.Impact)
                    , new XElement("Causes", projectObject.Causes)
                    , new XElement("CentreforPuralAction", projectObject.CentreforPuralAction)
                    , new XElement("Requirements", projectObject.Requirements)
                    , new XElement("SkillsNeeded", projectObject.SkillsNeeded)
                    , new XElement("Duration", projectObject.Duration)
                    , new XElement("TimeCommitment", projectObject.TimeCommitment)
                    , new XElement("TimePeriod", projectObject.TimePeriod)
                    , new XElement("CategoryName", projectObject.CategoryName)));
                xmlDoc.Save(projectFilePath);

            }
            catch (Exception ex)
            {
                LogWriter.LogWrite(ex.ToString());
            }


        }

        /// <summary>
        /// Get All Projects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProjectObject> GetAllProjects()
        {
            List<ProjectObject> listProjectObject = new List<ProjectObject>();

            try
            {
                if (!File.Exists(projectFilePath))
                {
                    var newdoc = new XDocument();
                    newdoc.Add(new XElement("Projects"));
                    newdoc.Save(projectFilePath);

                }
                else
                {
                    using (StreamReader bodyReader = new StreamReader(projectFilePath))
                    {
                        string bodyString = bodyReader.ReadToEnd();
                        int length = bodyString.Length;

                        if (length > 0)
                        {
                            DataSet ds = new DataSet();
                            ds.ReadXml(projectFilePath);
                            DataView dvPrograms;
                            if (ds.Tables.Count > 0)
                            {
                                dvPrograms = ds.Tables[0].DefaultView;
                                dvPrograms.Sort = "ProjectId";
                                foreach (DataRowView dr in dvPrograms)
                                {
                                    ProjectObject model = new ProjectObject();
                                    model.ProjectId = Convert.ToInt32(dr["ProjectId"]);
                                    model.Description = Convert.ToString(dr["Description"]);
                                    model.Impact = Convert.ToString(dr["Impact"]);
                                    model.Causes = Convert.ToString(dr["Causes"]);
                                    model.CentreforPuralAction = Convert.ToString(dr["CentreforPuralAction"]);
                                    model.Requirements = Convert.ToString(dr["Requirements"]);
                                    model.SkillsNeeded = Convert.ToString(dr["SkillsNeeded"]);
                                    model.Duration = Convert.ToString(dr["Duration"]);
                                    model.TimeCommitment = Convert.ToString(dr["TimeCommitment"]);
                                    model.TimePeriod = Convert.ToString(dr["TimePeriod"]);
                                    model.CategoryName = Convert.ToString(dr["CategoryName"]);
                                    listProjectObject.Add(model);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite(ex.ToString());
            }



            return listProjectObject;
        }

        /// <summary>
        /// Edit Project Details
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public ProjectObject ProjectEdit(int ProjectId)
        {
            ProjectObject projectObject = new ProjectObject();
            try
            {
                XDocument oXmlDocument = XDocument.Load(projectFilePath);
                var items = (from item in oXmlDocument.Descendants("Project")
                             where Convert.ToInt32(item.Element("ProjectId").Value) == ProjectId
                             select new ProjectObject
                             {
                                 ProjectId = Convert.ToInt32(item.Element("ProjectId").Value),
                                 Description = item.Element("Description").Value,
                                 Impact = item.Element("Impact").Value,
                                 Causes = item.Element("Causes").Value,
                                 CentreforPuralAction = item.Element("CentreforPuralAction").Value,
                                 Requirements = item.Element("Requirements").Value,
                                 SkillsNeeded = item.Element("SkillsNeeded").Value,
                                 Duration = item.Element("Duration").Value,
                                 TimeCommitment = item.Element("TimeCommitment").Value,
                                 TimePeriod = item.Element("TimePeriod").Value,
                                 CategoryName = item.Element("CategoryName").Value,
                             }).SingleOrDefault();
                if (items != null)
                {
                    projectObject.ProjectId = items.ProjectId;
                    projectObject.Description = items.Description;
                    projectObject.Impact = items.Impact;
                    projectObject.Causes = items.Causes;
                    projectObject.CentreforPuralAction = items.CentreforPuralAction;
                    projectObject.Requirements = items.Requirements;
                    projectObject.SkillsNeeded = items.SkillsNeeded;
                    projectObject.Duration = items.Duration;
                    projectObject.TimeCommitment = items.TimeCommitment;
                    projectObject.TimePeriod = items.TimePeriod;
                    projectObject.CategoryName = items.CategoryName;
                }
            }
            catch(Exception ex)
            {
                LogWriter.LogWrite(ex.ToString());
            }
            
            
            return projectObject;
        }
    }
}
