using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace FiveDFileNumberSearchLib
{
    public class WellInfo
    {
        public WellInfo()
        {
            PlanVersionList = new List<PlanVersionInfo>();
        }

        public string WellName { get; set; }
        public string FileNumber { get; set; }

        public WellInfo ParentWell { get; set; }

        public List<PlanVersionInfo> PlanVersionList { get; set; }
    }

    public class ModelInfo
    {
        public string ModelPath { get; set; }

        public DateTime LastModified()
        {
            if (File.Exists(ModelPath))
            {
                return File.GetLastWriteTime(ModelPath);
            }
            return DateTime.MinValue;
        }
    }

    public class PlanVersionInfo
    {
        public string PlanName { get; set; }
        public string VersionName { get; set; }
        public DateTime CreationDate { get; set; }
        public string Comment { get; set; }

        public bool IsDefinitivePlan { get; set; }

        public bool IsCurrentPlan { get; set; }
    }

    public class FieldParser
    {
        private readonly List<WellInfo> _wellInfo = new List<WellInfo>();

        public FieldParser(string fieldXmlFilePath)
        {
            using (var reader = XmlReader.Create(fieldXmlFilePath))
            {
                ParseField(reader);
            }
        }

        private void ParseField(XmlReader reader)
        {
            while (reader.ReadToFollowing("Well"))
            {
                ParseWell(reader.ReadSubtree());
            }
        }

        public IEnumerable<WellInfo> AllWellInfo => _wellInfo;


        private void ParsePlans(XmlReader rdr, WellInfo wi)
        {
            while (rdr.ReadToFollowing("Plan"))
            {
                ParsePlan(rdr.ReadSubtree(),wi);
            }
            if (wi.PlanVersionList.Count > 0)
            {
                var lastPlanVersion = wi.PlanVersionList[wi.PlanVersionList.Count - 1];
                lastPlanVersion.IsCurrentPlan = true;
            }
        }

        private void ParsePlan(XmlReader rdr, WellInfo wi)
        {
            string planName = string.Empty;
            while (rdr.Read())
            {
                if (rdr.IsStartElement())
                {
                    if (rdr.Name == "PlanInfo")
                    {
                        planName = rdr.GetAttribute("planName");
                    }
                    if (rdr.Name == "Version")
                    {
                        ParsePlanVersion(rdr.ReadSubtree(), wi, planName);
                    }
                }
            }
        }

        private void ParsePlanVersion(XmlReader rdr, WellInfo wi, string planName)
        {
            while (rdr.Read())
            {
                if (rdr.IsStartElement())
                {
                    if (rdr.Name == "VersionInfo")
                    {
                        PlanVersionInfo pvi = new PlanVersionInfo
                        {
                            PlanName = planName,
                            VersionName = rdr.GetAttribute("versionName"),
                            Comment = rdr.GetAttribute("comment"),
                            IsCurrentPlan = false
                        };

                        DateTime.TryParse(rdr.GetAttribute("versionDateCreated"), out var creationDate);
                        pvi.CreationDate = creationDate;

                        bool.TryParse(rdr.GetAttribute("isDefinitivePlan"), out bool isDefinitivePlan);
                        pvi.IsDefinitivePlan = isDefinitivePlan;

                        wi.PlanVersionList.Add(pvi);
                    }
                }
            }
        }

        private void ParseChildWellList(XmlReader rdr, WellInfo parentWell)
        {
            while (rdr.ReadToFollowing("Well"))
            {
                ParseWell(rdr.ReadSubtree(), parentWell);
            }
        }

        private void ParseWell(XmlReader rdr, WellInfo parentWell = null)
        {
            WellInfo wellInfo = new WellInfo();
            _wellInfo.Add(wellInfo);
            wellInfo.ParentWell = parentWell;
            while (rdr.Read())
            {
                if (rdr.IsStartElement())
                {
                    if (rdr.Name == "WellMisc")
                    {
                        wellInfo.WellName = rdr.GetAttribute("wellName");
                        wellInfo.FileNumber = rdr.GetAttribute("fileNumber");
                    }
                    if (rdr.Name == "PlanList")
                    {
                        if (!rdr.IsEmptyElement)
                        {
                            ParsePlans(rdr.ReadSubtree(),wellInfo);
                        }
                    }
                    if (rdr.Name == "ChildWells")
                    {
                        if (!rdr.IsEmptyElement)
                        {
                            ParseChildWellList(rdr.ReadSubtree(), wellInfo);
                        }
                    }
                }
            }
        }
    }
}
