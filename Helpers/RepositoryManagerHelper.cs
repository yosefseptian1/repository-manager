using System.Collections.Concurrent;
using System.Text.Json;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using RepositoryManager.Models;

namespace RepositoryManager.Helpers;

public class RepositoryManagerHelper
{
    private readonly AppDbContext _context;
    public RepositoryManagerHelper(AppDbContext context)
    {
        this._context = context;
    }

    public async Task Register(string itemName, string itemContent, short repoType)
    {
        if (itemName == null)
        {
            throw new ArgumentNullException(nameof(itemName));
        }
        if (itemContent == null)
        {
            throw new ArgumentNullException(nameof(itemContent));
        }
        if (repoType == null)
        {
            throw new ArgumentOutOfRangeException(nameof(repoType), "repository Type must be filled!");
        }
        
        var checkRepoType = await this._context.TypeRepository.FirstOrDefaultAsync(u => u.id_type == repoType);

        // ===== repo type ===== //
        //1. JSON
        //2. XML
        // ===== repo type ===== //

        if (repoType == 1)
        {
            ValidateJson(itemContent);
        }
        else
        {
            ValidateXml(itemContent);
        }

        var newRepository = new Repository
        {
            name_repository = itemName,
            id_type = repoType
        };
        
        _context.Repository.Add(newRepository);
        var result = await _context.SaveChangesAsync();

        if (result == 0)
        {
            throw new Exception("Failed to register repository.");
        }
        Console.WriteLine("Repository successfully added!");
    }

    public string Retrieve(string itemName)
    {
        var checkRepoName = _context.Repository.FirstOrDefault(u => u.name_repository == itemName);
        if (checkRepoName == null)
        {
            throw new Exception("Repository does not exist!.");
        }
        return $"Repository {checkRepoName.name_repository} found!";
    }

    public int GetType(string itemName)
    {
        var checkRepoName = _context.Repository.FirstOrDefault(u => u.name_repository == itemName);
        if (checkRepoName == null)
        {
            throw new Exception("Repository does not exist!.");
        }
        return checkRepoName.id_type;
    }
    
    public void Deregister(string itemName)
    {
        var checkRepoName = _context.Repository.FirstOrDefault(u => u.name_repository == itemName);
        if (checkRepoName == null)
        {
            throw new Exception ("Repository does not exist!");
        }
        _context.Repository.Remove(checkRepoName);
        _context.SaveChangesAsync();
        
        Console.WriteLine("Remove repository successfully!");
    }
    
    private static void ValidateJson(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
        }
        catch (JsonException ex)
        {
            throw new ArgumentException("Invalid JSON content.", ex);
        }
    }

    private static void ValidateXml(string xml)
    {
        try
        {
            var settings = new XmlReaderSettings
            {
                ConformanceLevel = ConformanceLevel.Document,
                DtdProcessing = DtdProcessing.Prohibit,
                IgnoreComments = true,
                IgnoreWhitespace = true
            };

            using var stringReader = new System.IO.StringReader(xml);
            using var reader = XmlReader.Create(stringReader, settings);
            while (reader.Read()) { }
        }
        catch (XmlException ex)
        {
            throw new ArgumentException("Invalid XML content.", ex);
        }
    }
}