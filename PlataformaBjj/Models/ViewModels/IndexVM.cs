using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaBjj.Models.ViewModels
{
    public class IndexVM
    {
        public IEnumerable<NewsItem> News { get; set; }
        public IEnumerable<Phrases> Phrases { get; set; }
    }
}
