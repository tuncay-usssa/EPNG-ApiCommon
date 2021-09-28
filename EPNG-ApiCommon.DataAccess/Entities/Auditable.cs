using System;
using System.Collections.Generic;
using System.Text;

namespace EPNG_ApiCommon.Entities
{
    public class Auditable : RootEntity, IAuditable
    {
        public Auditable()
        {
            IsActive = true;
        }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
    public interface IAuditable
    {
        bool IsActive { get; set; }
        DateTime CreatedDate { get; set; }
        string CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        string ModifiedBy { get; set; }
    }
}
