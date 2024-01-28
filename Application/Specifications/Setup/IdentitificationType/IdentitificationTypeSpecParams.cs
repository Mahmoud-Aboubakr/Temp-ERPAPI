using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications.Setup.IdentitificationType
{
    public class IdentitificationTypeSpecParams
    {
        private const int _maxPageSize = 50;

        private int _pageSize = 5;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = _pageSize > _maxPageSize ? _maxPageSize : value; }
        }

        public int PageIndex { get; set; } = 1;

        private string _search = string.Empty;
        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
    }
}
