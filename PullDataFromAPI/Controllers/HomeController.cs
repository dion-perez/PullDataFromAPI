using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PullDataFromAPI.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;

namespace PullDataFromAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var jsonCandidatesFromApi = GetCandidates();
            var candidatesButOld = MakeCandidatesSad(jsonCandidatesFromApi);
            var orderedColours = OrderColours(jsonCandidatesFromApi);

            DataViewModel dataViewModel = new DataViewModel
            {
                Colours = orderedColours,
                Candidates = candidatesButOld
            };

            return View(dataViewModel);
        }

        public List<Candidate> GetCandidates()
        {
            try
            {
                List<Candidate> candidates = new List<Candidate>();

                HttpClient client = new HttpClient();
                HttpResponseMessage response = client.GetAsync("https://recruitment.highfieldqualifications.com/api/test").Result;

                var contentResponse = response.Content.ReadAsStringAsync().Result;
                var jsonCandidates = JsonConvert.DeserializeObject<List<Candidate>>(contentResponse);

                if (jsonCandidates != null)
                    return jsonCandidates;

                return new List<Candidate>();
            }
            catch(Exception e)
            {
                var exception = e.InnerException;
                return new List<Candidate>();
            }
        }

        public List<Candidate> MakeCandidatesSad(List<Candidate> candidates)
        {
            foreach (var candidate in candidates)
            {
                candidate.dob = candidate.dob.AddYears(20);
            }

            return candidates;
        }

        public List<string> OrderColours(List<Candidate> candidates)
        {
            // Create new list of just colours then order by frequency, alphabetically
            List<string> colours =
                candidates.Select(candidate => candidate.favouriteColour)
                          .OrderBy(favColour => favColour.Trim())
                          .ThenBy(favColour => favColour.Count())
                          .ToList();

            return colours;
        }
    }
}