using System;
using System.Collections.Generic;
using System.Text;

namespace CodeMeasurement.Measurements.StorageObjects
{
    class ProjectInfo
    {
        public List<GeneralMetric> generalMetricList;
        public int projectId, sourceId;
        public string name, email, source;
        public DateTime creationDate, lastUpdateDate;

        public ProjectInfo(int projectId, int sourceId, DateTime creationDate, DateTime lastUpdateDate, string email, string name)
        {
            generalMetricList = new List<GeneralMetric>();
            this.projectId = projectId;
            this.sourceId = sourceId;
            this.creationDate = creationDate;
            this.lastUpdateDate = lastUpdateDate;
            this.email = email;
            this.name = name;
        }
    }
}
