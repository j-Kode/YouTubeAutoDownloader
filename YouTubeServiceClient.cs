using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;

namespace ConsoleApplication2
{
    public class YouTubeServiceClient
    {
        private static YouTubeServiceClient instance;
        public static YouTubeServiceClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new YouTubeServiceClient();
                }
                return instance;
            }
        }
        public async Task<YouTubeService> GetYouTubeService(string userEmail)
        {
            UserCredential credential;
            using (var stream = new FileStream("C:\\test\\stuff.json", FileMode.Open, FileAccess.Read))
            {
                dsAuthorizationBroker.RedirectUri = "http://localhost:64987/authorize/";

                credential = await dsAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                new[]
                {
                    YouTubeService.Scope.Youtube,
                    YouTubeService.Scope.Youtubepartner,
                    YouTubeService.Scope.YoutubeUpload,
                    YouTubeService.Scope.YoutubepartnerChannelAudit,
                    YouTubeService.Scope.YoutubeReadonly
                },
                userEmail,
                CancellationToken.None,
                new FileDataStore(this.GetType().ToString()));
            }
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });
            return youtubeService;
        }
        public class dsAuthorizationBroker : GoogleWebAuthorizationBroker
        {
            public static string RedirectUri;

            public new static async Task<UserCredential> AuthorizeAsync(
                ClientSecrets clientSecrets,
                IEnumerable<string> scopes,
                string user,
                CancellationToken taskCancellationToken,
                IDataStore dataStore = null)
            {
                var initializer = new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = clientSecrets,
                };
                return await AuthorizeAsyncCore(initializer, scopes, user,
                    taskCancellationToken, dataStore).ConfigureAwait(false);
            }

            private static async Task<UserCredential> AuthorizeAsyncCore(
                GoogleAuthorizationCodeFlow.Initializer initializer,
                IEnumerable<string> scopes,
                string user,
                CancellationToken taskCancellationToken,
                IDataStore dataStore)
            {
                initializer.Scopes = scopes;
                initializer.DataStore = dataStore ?? new FileDataStore(Folder);
                var flow = new dsAuthorizationCodeFlow(initializer);
                return await new AuthorizationCodeInstalledApp(flow,
                    new LocalServerCodeReceiver())
                    .AuthorizeAsync(user, taskCancellationToken).ConfigureAwait(false);
            }
        }
        public class dsAuthorizationCodeFlow : GoogleAuthorizationCodeFlow
        {
            public dsAuthorizationCodeFlow(Initializer initializer)
                : base(initializer) { }

            public override AuthorizationCodeRequestUrl
                           CreateAuthorizationCodeRequest(string redirectUri)
            {
                return base.CreateAuthorizationCodeRequest(dsAuthorizationBroker.RedirectUri);
            }
        }

    }
}
