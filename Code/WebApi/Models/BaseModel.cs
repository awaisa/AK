using System;

namespace WebApiCore.Models
{
    public abstract class BaseModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? ModifiedOn { get; set; }
    }
}