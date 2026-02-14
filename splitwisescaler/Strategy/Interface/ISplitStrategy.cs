using splitwisescaler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace splitwisescaler.Strategy.Interface
{
    public interface ISplitStrategy
    {
        Dictionary<User, decimal> Split(decimal amount, List<User> participants, params object[] parameters);
    }
}
