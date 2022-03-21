using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaBjj.Services
{
    public interface ITemplateSender
    {
        Task SendTemplateAsync(string email, string templateKey,string name);

    }
}
