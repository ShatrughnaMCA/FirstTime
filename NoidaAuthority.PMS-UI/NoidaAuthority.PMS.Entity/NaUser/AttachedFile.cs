using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class AttachedFile
    {
        public int AttachedFileId { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public string FileType { get; set; }
        public int CustomerId { get; set; }
        public virtual string NACustomer { get; set; }
    }
    public class DocumentDetail
    {
        public int RegistrationID { get; set; }
        public string DocumentType { get; set; }
        public string DocumentName { get; set; }
        public string PathName { get; set; }
    }
}
