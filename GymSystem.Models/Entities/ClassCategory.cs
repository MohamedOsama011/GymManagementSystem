using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.Models.Entities
{
    public class ClassCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<GymClass> GymClasses { get; set; } = new List<GymClass>();
    }
}
