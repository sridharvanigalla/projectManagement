
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using BusinessObject;
using Microsoft.AspNetCore.Hosting;
namespace BusinessLogic
{
    public class CategoryBL
    {
        private IHostingEnvironment _hostingEnvironment;
        private static string filePath;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="environment"></param>
        public CategoryBL(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
            filePath = _hostingEnvironment.WebRootPath + "//Category//CategoryTable.xml";
        }

        /// <summary>
        /// Create new or update existing Category
        /// </summary>
        /// <param name="categoryObject"></param>
        /// <returns></returns>
        public int SaveUpdateCategory(CategoryObject categoryObject)
        {
            int result = 0;
            if (categoryObject.CategoryId > 0)
            {
                UpdateCategory(categoryObject);
            }
            else if (categoryObject.CategoryId == 0)
            {
                result = CreateXML(categoryObject);
            }
            return result;
        }

        /// <summary>
        /// Update Category
        /// </summary>
        /// <param name="categoryObject"></param>
        public void UpdateCategory(CategoryObject categoryObject)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(filePath);
                var items = (from item in xmlDoc.Descendants("Category") select item).ToList();
                XElement selected = items.Where(p => p.Element("CategoryId").Value == categoryObject.CategoryId.ToString()).FirstOrDefault();
                selected.Remove();
                xmlDoc.Save(filePath);
                xmlDoc.Element("Categories").Add(new XElement("Category", new XElement("CategoryId", categoryObject.CategoryId), new XElement("CategoryName", categoryObject.CategoryName), new XElement("IsDeleted", 0)));
                xmlDoc.Save(filePath);
            }
            catch(Exception ex)
            {
                LogWriter.LogWrite(ex.ToString());
            }
            
        }

        /// <summary>
        /// Delete Category
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public int DeleteCategory(int CategoryId)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(filePath);
                var items = (from item in xmlDoc.Descendants("Category") select item).ToList();
                XElement selected = items.Where(p => p.Element("CategoryId").Value == CategoryId.ToString()).FirstOrDefault();
                selected.Remove();
                xmlDoc.Save(filePath);
                return 1;
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite(ex.ToString());
                return 0;
            }

        }

        /// <summary>
        /// Create new xml if not exisiting
        /// </summary>
        /// <param name="categoryObject"></param>
        private int CreateXML(CategoryObject categoryObject)
        {
            try
            {
                var path = _hostingEnvironment.WebRootPath;

                XmlDocument oXmlDocument = new XmlDocument();
                oXmlDocument.Load(filePath);
                XmlNodeList nodelist = oXmlDocument.GetElementsByTagName("Category");
                var x = oXmlDocument.GetElementsByTagName("CategoryId");
                var categoryNamesNode = oXmlDocument.GetElementsByTagName("CategoryName");

                foreach (XmlElement item in categoryNamesNode)
                {
                    if(item.InnerText.ToString().ToLower().Equals(categoryObject.CategoryName.ToLower()))
                    {
                        return 0;
                    }
                }

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
                XDocument xmlDoc = XDocument.Load(filePath);
                xmlDoc.Element("Categories").Add(new XElement("Category", new XElement("CategoryId", Max), new XElement("CategoryName", categoryObject.CategoryName), new XElement("IsDeleted", 0)));
                xmlDoc.Save(filePath);
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite(ex.ToString());
                return 0;
            }

            return 1;
        }

        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CategoryObject> GetAllCategory()
        {
            List<CategoryObject> listCategoryObject = new List<CategoryObject>();
            try
            {
                if (!File.Exists(filePath))
                {
                    var newdoc = new XDocument();
                    newdoc.Add(new XElement("Categories"));
                    newdoc.Save(filePath);
                }
                else
                {
                    using (StreamReader bodyReader = new StreamReader(filePath))
                    {
                        string bodyString = bodyReader.ReadToEnd();
                        int length = bodyString.Length;

                        if (length > 0)
                        {
                            DataSet ds = new DataSet();
                            ds.ReadXml(filePath);
                            DataView dvPrograms;
                            if (ds.Tables.Count > 0)
                            {
                                dvPrograms = ds.Tables[0].DefaultView;
                                dvPrograms.Sort = "CategoryId";
                                foreach (DataRowView dr in dvPrograms)
                                {
                                    CategoryObject model = new CategoryObject();
                                    model.CategoryId = Convert.ToInt32(dr["CategoryId"]);
                                    model.CategoryName = Convert.ToString(dr["CategoryName"]);
                                    listCategoryObject.Add(model);
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

            return listCategoryObject;
        }

        /// <summary>
        /// Edit Category
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public CategoryObject CategoryEdit(int CategoryId)
        {
            CategoryObject categoryObject = new CategoryObject();
            try
            {
                XDocument oXmlDocument = XDocument.Load(filePath);
                var items = (from item in oXmlDocument.Descendants("Category")
                             where Convert.ToInt32(item.Element("CategoryId").Value) == CategoryId
                             select new CategoryObject
                             {
                                 CategoryId = Convert.ToInt32(item.Element("CategoryId").Value),
                                 CategoryName = item.Element("CategoryName").Value,
                             }).SingleOrDefault();
                if (items != null)
                {
                    categoryObject.CategoryId = items.CategoryId;
                    categoryObject.CategoryName = items.CategoryName;

                }
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite(ex.ToString());
            }

            return categoryObject;
        }

    }
}
