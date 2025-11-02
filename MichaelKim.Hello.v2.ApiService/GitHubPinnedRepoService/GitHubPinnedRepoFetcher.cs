using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

/*
https://aspnano.com/web-scraping-in-c-asp-net/
*/

public class PinnedRepo {
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Link { get; set; } = "";
}

public class GitHubPinnedRepoFetcher {
    private readonly HttpClient _httpClient;

    public GitHubPinnedRepoFetcher() {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("MyAspireGitHubRepoFetcher/1.0");
    }

    public async Task<List<PinnedRepo>> FetchPinnedReposAsync(string githubUsername) {
        string baseUrl = "https://github.com";
        string profileUrl = $"{baseUrl}/{githubUsername}";

        // raw html file
        string html = await _httpClient.GetStringAsync(profileUrl);
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // find "div" sections that contain 'pinned-item-list-item' -> find all pinned repo sections
        HtmlNodeCollection pinnedRepoCards = doc.DocumentNode.SelectNodes("//div[contains(@class, 'pinned-item-list-item')]");
        var repos = new List<PinnedRepo>();

        if (pinnedRepoCards != null) {
            foreach (var card in pinnedRepoCards) {
                // find "a" sections that contain '/michaeljhkim/' -> find link to pinned repo
                // <a ></a> is the link in HTML format 
                HtmlNode? linkNode = card.SelectSingleNode(".//a[contains(@href, '/michaeljhkim/')]");
                string name = linkNode?.InnerText.Trim() ?? "";
                string? href = linkNode?.GetAttributeValue("href", "").Trim();

                // find "p" sections that contain 'pinned-item-desc' -> find pinned repo description
                HtmlNode? descNode = card.SelectSingleNode(".//p[contains(@class, 'pinned-item-desc')]");
                string description = descNode?.InnerText.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(href)) {
                    continue;
                }
                repos.Add(
                    new PinnedRepo {
                        Name = name,
                        Description = description,
                        Link = $"{baseUrl}{href}"
                    }
                );
            }
        }

        return repos;
    }
}
