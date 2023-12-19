using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Interfaces
{
    public interface ICloseable
    {
        Action Close { get; set; }
    }
}
