using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Linq;

namespace Cheng.Comon.ComIdentityServer4
{
    //    <PackageReference Include="IdentityServer4" Version="2.1.0" />

    /// <summary>
    /// 认证的服务类，提供了用户，第一步授权
    /// </summary>
    public class Config
    {

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },

                // resource owner password grant client
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AccessTokenLifetime = 5,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },

                //自定义方式
                 new Client()
        {
            ClientId =OAuthConfig.UserApi.ClientId,
            AllowedGrantTypes = new List<string>()
            {
                GrantTypes.ResourceOwnerPassword.FirstOrDefault(),//Resource Owner Password模式
                GrantTypeConstants.ResourceWeixinOpen,//新增的自定义微信客户端的授权模式
            },
            ClientSecrets = {new Secret(OAuthConfig.UserApi.Secret.Sha256()) },
            AllowedScopes= {OAuthConfig.UserApi.ApiName},
            AccessTokenLifetime = OAuthConfig.ExpireIn,
        }



            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
        }
    }

    public class OAuthConfig
    {
        /// <summary>
        /// 过期秒数
        /// </summary>
        public const int ExpireIn = 36000;

        /// <summary>
        /// 用户Api相关
        /// </summary>
        public static class UserApi
        {
            public static string ApiName = "user_api";

            public static string ClientId = "user_clientid";

            public static string Secret = "user_secret";
        }
    }
}
