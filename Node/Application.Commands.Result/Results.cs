using System;
using System.Collections.Generic;
using Application.Common.Interfaces;
using Domain.Publicity;
using SmallTransit;

namespace Application.Commands.Result
{
    public class Results
    {
        private readonly Dictionary<string, List<Space>> results;
        private readonly IHostInfo _hostInfo;
        private int currentIteration;
  

        public event EventHandler ReceptionDonneesTerminer;

        public Results(Dictionary<string, List<Space>> results, IHostInfo hostInfo)
        {
            this.results = results ?? throw new ArgumentNullException(nameof(results));
            _hostInfo = hostInfo ?? throw new ArgumentNullException(nameof(hostInfo));
            currentIteration = 0;
        }
        /* Prends le nombre d'itération
        public void GetNumberOfIteration()
        {
            int numberOfIteration = _hostInfo.NbOfIteration;
        }
        */
        /*public async Task RunContinuousProcessingAsync()
        {
            while (iterationRemains)
            {
                await ProcessIterationAsync();
            }

            OnReceptionDonneesTerminer();
            Console.WriteLine("Itérations terminées");
           
        }*/
        /*public bool numberOfIterations()
        {
            int numberOfIteration = _hostInfo.NbOfIteration;

            if (currentIteration < numberOfIteration)
            {
            
                currentIteration++;
                return true;
            }
            /*else
            {
              
                

                iterationRemains = false;
                DisplayResults(results);
            }
            return false;
        }*/

        public void DisplayResults(Dictionary<string, List<Space>> finalResults)
        {
            foreach (var kvp in finalResults)
            {
                string groupId = kvp.Key;
                List<Space> spaces = kvp.Value;

                Console.WriteLine($"Group ID: {groupId}");

            
                foreach (var space in spaces)
                {
                    Console.WriteLine($"  Space ID: {space.Id}, Width: {space.Width}, Price: {space.Price}");
                }

                Console.WriteLine(); 
            }
        }
/*
        public void AddResults(string groupId, List<Space> spaces)
        {
            if (results.ContainsKey(groupId))
            {
                results[groupId].AddRange(spaces);
            }
            else
            {
                results[groupId] = spaces;
            }

          
            OnReceptionDonneesTerminer();
        }*/
        public Dictionary<string, List<Space>> GetResults()
        {
            return results;
        }

/*
        protected void OnReceptionDonneesTerminer()
        {
            ReceptionDonneesTerminer?.Invoke(this, EventArgs.Empty);
        }*/
        public bool HasMoreIterations()
        {
            int numberOfIteration = _hostInfo.NbOfIteration;
            if (currentIteration < numberOfIteration)
            {
                currentIteration++;
                return true;
            }

            return false;
        }


    }
}
