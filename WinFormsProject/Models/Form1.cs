using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsProject.Models
{
    public class LoanModel
    {
        public string LoanId { get; set; }
        public string BorrowerName { get; set; }
        public string LoanAmount { get; set; }
    }

    public class PipelineLoanItem
    {
        public string id { get; set; }
        public List<PipelineField> fields { get; set; }


        public string GetFieldValue(string fieldName)
        {
            return fields?.Find(f => string.Equals(f.name, fieldName, StringComparison.OrdinalIgnoreCase))?.value;
        }
    }

    public class PipelineField
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class DocumentModel
    {
        public string DocumentId { get; set; }
        public string DocumentName { get; set; }
    }

    public class AttachmentModel
    {
        public bool IsSelected { get; set; } = false;
        public string AttachmentId { get; set; }
        public string AttachmentName { get; set; }
        public string FileType { get; set; }
    }
}
