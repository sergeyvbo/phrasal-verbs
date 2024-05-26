using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using phrasal_verbs.Models;

namespace phrasal_verbs.Services;

public class PhrasalVerbService
{
    private readonly List<PhrasalVerb> _phrasalVerbs;

    public PhrasalVerbService()
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        };
        using (var reader = new StreamReader("data.csv"))
        using (var csv = new CsvReader(reader, config))
        {
            _phrasalVerbs = csv.GetRecords<PhrasalVerb>().ToList();
        }
    }

    public (PhrasalVerb phrasalVerb, List<string> meanings) GetRandomPhrasalVerbWithMeanings()
    {
        var random = new Random();
        var phrasalVerb = _phrasalVerbs[random.Next(_phrasalVerbs.Count)];

        var allMeanings = _phrasalVerbs.Select(pv => pv.Meaning).ToList();
        var randomMeanings = allMeanings.Where(m => m != phrasalVerb.Meaning).OrderBy(x => random.Next()).Take(2).ToList();
        randomMeanings.Add(phrasalVerb.Meaning);
        randomMeanings = randomMeanings.OrderBy(x => random.Next()).ToList();

        return (phrasalVerb, randomMeanings);
    }

    public PhrasalVerb CheckMeaning(string verb, string selectedMeaning)
    {
        var phrasalVerb = _phrasalVerbs.FirstOrDefault(pv => pv.Verb == verb);
        return phrasalVerb;
    }
}