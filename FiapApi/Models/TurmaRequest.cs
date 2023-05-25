using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace FiapApi.Models
{
    public class TurmaRequest
    {
        
        public int curso_id { get; set; }
        public int ano { get; set; }
        public string? turma { get; set; }
    }
    
}
