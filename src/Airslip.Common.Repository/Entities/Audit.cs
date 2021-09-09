using Airslip.Common.Repository.Interfaces;
using System;

namespace Airslip.Common.Repository.Entities
{
    public class BasicAuditInformation : IEntityNoId
    {
        public string? CreatedByUserId { get; set; }
        
        public DateTime DateCreated { get; set; }
        
        public string? UpdatedByUserId { get; set; }
        
        public DateTime? DateUpdated { get; set; }
        
        public string? DeletedByUserId { get; set; }
        
        public DateTime? DateDeleted { get; set; }
    }
}