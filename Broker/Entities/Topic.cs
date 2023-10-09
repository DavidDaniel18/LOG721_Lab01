using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Topic : ITopic
    {
        public string Name { get; } = "";

        public string Path { get; } = "";
    }
}
