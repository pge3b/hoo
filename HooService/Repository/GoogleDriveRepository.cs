﻿using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;

namespace HooService.Repository
{
    public class GoogleDriveRepository : ISourceRepositor
    {

        private DriveService driveService { get; }

        public GoogleDriveRepository(IConfiguration configuration)
        {
            Console.WriteLine(configuration.GetValue<string>("AccessToken"));

            var tokenResponse = new TokenResponse
            {
                AccessToken = configuration.GetValue<string>("AccessToken"),
                RefreshToken = configuration.GetValue<string>("RefreshToken"),
            };

            var applicationName = configuration.GetValue<string>("ApplicationName");
            var username = configuration.GetValue<string>("Username");

            var apiCodeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = configuration.GetValue<string>("ClientId"),
                    ClientSecret = configuration.GetValue<string>("ClientSecret")
                },
                Scopes = new[] { DriveService.ScopeConstants.Drive },
                DataStore = new FileDataStore(applicationName)
            });


            var credential = new UserCredential(apiCodeFlow, username, tokenResponse);

            driveService = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = applicationName
            });
        }

        public IEnumerable<Google.Apis.Drive.v3.Data.File> GetFiles(string folder)
        {
            var fileList = driveService.Files.List();
            fileList.Q = $"mimeType!='application/vnd.google-apps.folder' and '{folder}' in parents";
            fileList.Fields = "nextPageToken, files(id, name, size, mimeType)";

            var result = new List<Google.Apis.Drive.v3.Data.File>();
            string pageToken = null;
            do
            {
                fileList.PageToken = pageToken;
                var filesResult = fileList.Execute();
                var files = filesResult.Files;
                pageToken = filesResult.NextPageToken;
                result.AddRange(files);
            } while (pageToken != null);

            return result;
        }
    }
}
