using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EPNG_ApiCommon.Entities
{
    public class Entity : IEntity
    {
        [Key]
        public int Id { get; set; }
    }

    public interface IEntity
    {
        int Id { get; set; }
    }
}
