using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAttrition.ML.Objects
{
    public class EmploymentHistoryPrediction//Contains only the prediction value of how many months the employee is projected  to be at his or her job
    {
        [ColumnName("Score")]
        public float DurationInMonths;

    }
}
