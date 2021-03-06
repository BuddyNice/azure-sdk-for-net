﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Azure.Core.Testing;
using Azure.Search.Documents.Models;
using NUnit.Framework;

#region Snippet:Azure_Search_Tests_Samples_Readme_Namespace
using Azure;
using Azure.Search.Documents;
#endregion Snippet:Azure_Search_Tests_Samples_Readme_Namespace

namespace Azure.Search.Documents.Tests.Samples
{
    /// <summary>
    /// Samples used to generate the README.md snippets.
    /// </summary>
    public class Readme : SearchTestBase
    {
        public Readme(bool async, SearchClientOptions.ServiceVersion serviceVersion)
            : base(async, serviceVersion, null /* RecordedTestMode.Record /* to re-record */)
        {
        }

        [Test]
        [SyncOnly]
        public void Authenticate()
        {
            #region Snippet:Azure_Search_Tests_Samples_Readme_Authenticate
            // Get the service endpoint and API key from the environment
            Uri endpoint = new Uri(Environment.GetEnvironmentVariable("SEARCH_ENDPOINT"));
            string key = Environment.GetEnvironmentVariable("SEARCH_API_KEY");

            // Create a client
            AzureKeyCredential credential = new AzureKeyCredential(key);
            SearchServiceClient client = new SearchServiceClient(endpoint, credential);
            #endregion Snippet:Azure_Search_Tests_Samples_Readme_Authenticate
        }

        [Test]
        [SyncOnly]
        public void FirstQuery()
        {
            #region Snippet:Azure_Search_Tests_Samples_Readme_FirstQuery
            // We'll connect to the Azure Cognitive Search public sandbox and send a
            // query to its "nycjobs" index built from a public dataset of available jobs
            // in New York.
            string serviceName = "azs-playground";
            string indexName = "nycjobs";
            string apiKey = "252044BE3886FE4A8E3BAA4F595114BB";

            // Create a SearchIndexClient to send queries
            Uri serviceEndpoint = new Uri($"https://{serviceName}.search.windows.net/");
            AzureKeyCredential credential = new AzureKeyCredential(apiKey);
            SearchIndexClient client = new SearchIndexClient(serviceEndpoint, indexName, credential);

            // Let's get the top 5 jobs related to Microsoft
            SearchResults<SearchDocument> response = client.Search("Microsoft", new SearchOptions { Size = 5 });
            foreach (SearchResult<SearchDocument> result in response.GetResults())
            {
                // Print out the title and job description (we'll see below how to
                // use C# objects to make accessing these fields much easier)
                string title = (string)result.Document["business_title"];
                string description = (string)result.Document["job_description"];
                Console.WriteLine($"{title}\n{description}\n");
            }
            #endregion Snippet:Azure_Search_Tests_Samples_Readme_FirstQuery
        }

        [Test]
        [SyncOnly]
        public async Task CreateAndQuery()
        {
            await using SearchResources resources = await SearchResources.GetSharedHotelsIndexAsync(this);
            Environment.SetEnvironmentVariable("SEARCH_ENDPOINT", resources.Endpoint.ToString());
            Environment.SetEnvironmentVariable("SEARCH_API_KEY", resources.PrimaryApiKey);
            string indexName = resources.IndexName;

            #region Snippet:Azure_Search_Tests_Samples_Readme_Client
            // Get the service endpoint and API key from the environment
            Uri endpoint = new Uri(Environment.GetEnvironmentVariable("SEARCH_ENDPOINT"));
            string key = Environment.GetEnvironmentVariable("SEARCH_API_KEY");
            //@@ string indexName = "hotels";

            // Create a client
            AzureKeyCredential credential = new AzureKeyCredential(key);
            SearchIndexClient client = new SearchIndexClient(endpoint, indexName, credential);
            /*@@*/ client = InstrumentClient(new SearchIndexClient(endpoint, indexName, credential, GetSearchClientOptions()));
            #endregion Snippet:Azure_Search_Tests_Samples_Readme_Client

            #region Snippet:Azure_Search_Tests_Samples_Readme_Dict
            SearchResults<SearchDocument> response = client.Search("luxury");
            foreach (SearchResult<SearchDocument> result in response.GetResults())
            {
                SearchDocument doc = result.Document;
                string id = (string)doc["hotelId"];
                string name = (string)doc["hotelName"];
                Console.WriteLine("{id}: {name}");
            }
            #endregion Snippet:Azure_Search_Tests_Samples_Readme_Dict

            #region Snippet:Azure_Search_Tests_Samples_Readme_Dynamic
            //@@ SearchResults<SearchDocument> response = client.Search("luxury");
            /*@@*/ response = client.Search("luxury");
            foreach (SearchResult<SearchDocument> result in response.GetResults())
            {
                dynamic doc = result.Document;
                string id = doc.hotelId;
                string name = doc.hotelName;
                Console.WriteLine("{id}: {name}");
            }
            #endregion Snippet:Azure_Search_Tests_Samples_Readme_Dynamic
        }

        #region Snippet:Azure_Search_Tests_Samples_Readme_StaticType
        public class Hotel
        {
            [JsonPropertyName("hotelId")]
            public string Id { get; set; }

            [JsonPropertyName("hotelName")]
            public string Name { get; set; }
        }
        #endregion Snippet:Azure_Search_Tests_Samples_Readme_StaticType

        [Test]
        [SyncOnly]
        public async Task QueryStatic()
        {
            await using SearchResources resources = await SearchResources.GetSharedHotelsIndexAsync(this);
            SearchIndexClient client = resources.GetQueryClient();

            #region Snippet:Azure_Search_Tests_Samples_Readme_StaticQuery
            SearchResults<Hotel> response = client.Search<Hotel>("luxury");
            foreach (SearchResult<Hotel> result in response.GetResults())
            {
                Hotel doc = result.Document;
                Console.WriteLine($"{doc.Id}: {doc.Name}");
            }
            #endregion Snippet:Azure_Search_Tests_Samples_Readme_StaticQuery

            #region Snippet:Azure_Search_Tests_Samples_Readme_StaticQueryAsync
            //@@SearchResults<Hotel> response = await client.SearchAsync<Hotel>("luxury");
            /*@@*/ response = await client.SearchAsync<Hotel>("luxury");
            await foreach (SearchResult<Hotel> result in response.GetResultsAsync())
            {
                Hotel doc = result.Document;
                Console.WriteLine($"{doc.Id}: {doc.Name}");
            }
            #endregion Snippet:Azure_Search_Tests_Samples_Readme_StaticQueryAsync
        }

        [Test]
        [SyncOnly]
        public async Task Options()
        {
            await using SearchResources resources = await SearchResources.GetSharedHotelsIndexAsync(this);
            SearchIndexClient client = resources.GetQueryClient();

            #region Snippet:Azure_Search_Tests_Samples_Readme_Options
            int stars = 4;
            SearchOptions options = new SearchOptions
            {
                // Filter to only ratings greater than or equal our preference
                Filter = SearchFilter.Create($"rating ge {stars}"),
                Size = 5, // Take only 5 results
                OrderBy = new[] { "rating desc" } // Sort by rating from high to low
            };
            SearchResults<Hotel> response = client.Search<Hotel>("luxury", options);
            // ...
            #endregion Snippet:Azure_Search_Tests_Samples_Readme_Options
        }

        [Test]
        [SyncOnly]
        public async Task Index()
        {
            await using SearchResources resources = await SearchResources.CreateWithEmptyHotelsIndexAsync(this);
            SearchIndexClient client = resources.GetQueryClient();
            try
            {
                #region Snippet:Azure_Search_Tests_Samples_Readme_Index
                IndexDocumentsBatch<Hotel> batch = IndexDocumentsBatch.Create(
                IndexDocumentsAction.Upload(new Hotel { Id = "783", Name = "Upload Inn" }),
                IndexDocumentsAction.Merge(new Hotel { Id = "12", Name = "Renovated Ranch" }));

                IndexDocumentsOptions options = new IndexDocumentsOptions { ThrowOnAnyError = true };
                client.IndexDocuments(batch, options);
                #endregion Snippet:Azure_Search_Tests_Samples_Readme_Index
            }
            catch (RequestFailedException)
            {
                // Ignore the non-existent merge failure
            }
        }

        [Test]
        [SyncOnly]
        public async Task Troubleshooting()
        {
            await using SearchResources resources = await SearchResources.GetSharedHotelsIndexAsync(this);
            SearchIndexClient client = resources.GetQueryClient();
            LookupHotel();

            // We want the sample to have a return but the unit test doesn't
            // like that so we move it into a block scoped function
            Response<Hotel> LookupHotel()
            {
                #region Snippet:Azure_Search_Tests_Samples_Readme_Troubleshooting
                try
                {
                    return client.GetDocument<Hotel>("12345");
                }
                catch (RequestFailedException ex) when (ex.Status == 404)
                {
                    Console.WriteLine("We couldn't find the hotel you are looking for!");
                    Console.WriteLine("Please try selecting another.");
                    return null;
                }
                #endregion Snippet:Azure_Search_Tests_Samples_Readme_Troubleshooting
            }
        }

    }
}
