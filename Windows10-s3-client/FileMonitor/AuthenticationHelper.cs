using Amazon;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMonitor
{
    public class AuthenticationHelper
    {
        //http://blog.jacobmarks.com/2016/12/amazon-cognito-user-pool-admin.html
        private Configuration _configuration;

        public AuthenticationHelper(Configuration configuration)
        {
            _configuration = configuration;
        }
    }
}
