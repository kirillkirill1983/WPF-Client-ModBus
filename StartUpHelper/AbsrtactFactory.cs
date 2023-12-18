using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp4.StartUpHelper
{
    public class AbsrtactFactory<T> : IAbsrtactFactory<T>
    {
        private readonly Func<T> _factory;
        public AbsrtactFactory(Func<T> factory)
        {
            _factory = factory;
        }

        public T Create()
        {
            return _factory();
        }
    }
}
