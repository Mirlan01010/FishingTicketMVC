using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.Responses
{
    public class ApiResponse
    {
        public string? Message { get; set; }
        public bool Success { get; set; } = true;
    }
}
