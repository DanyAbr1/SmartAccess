using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartAccess.Droid.Models
{
    public class SesionLock
    {
        public string InstallId { get; set; }
        public string ApplicationId { get; set; }
        public string UserId { get; set; }
        public bool VInstallId { get; set; }
        public bool VPassword { get; set; }
        public bool VEmail { get; set; }
        public bool VPhone { get; set; }
        public bool HasInstallId { get; set; }
        public bool HasPassword { get; set; }
        public bool HasEmail { get; set; }
        public bool HasPhone { get; set; }
        public bool IsLockedOut { get; set; }
        public string Captcha { get; set; }
        public List<object> Email { get; set; }
        public List<object> Phone { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string TemporaryAccountCreationPasswordLink { get; set; }
        public int Iat { get; set; }
        public int Exp { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
