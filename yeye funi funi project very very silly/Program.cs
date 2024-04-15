using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json;
namespace OpiumOsaka
{
    public class TriviaQuestionResponse
    {
        public List<TriviaQuestion>? Results { get; set; }
    }

    public class TriviaQuestion
    {
        public string? Question { get; set; }
        [JsonProperty("correct_answer")]
        public string? CorrectAnswer { get; set; }
        [JsonProperty("incorrect_answers")]
        public List<string>? IncorrectAnswers { get; set; }
    }

    class Program
    {
        async static Task Main(string[] args)
        {
            Program program= new Program();
            Console.WriteLine("Welcome to Nitrous Oxide Gang You will face consequences for this brainrot  Press any key to continue");
            Console.ReadKey();
            string key = "Opium osaka";
            Console.WriteLine("You must guess the right answer, if you don't we will sell your data to the chinese government!");
            Console.ReadKey();
            Console.WriteLine("Fetching questions...");
            await GetQuestions();

        }

        static async Task GetQuestions()
        {
            while (true) // Run infinitely until the user chooses to stop
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync("https://opentdb.com/api.php?amount=1&category=20&difficulty=hard&type=multiple");
                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            TriviaQuestionResponse triviaResponse = JsonConvert.DeserializeObject<TriviaQuestionResponse>(responseBody);

                            if (triviaResponse != null && triviaResponse.Results.Count > 0)
                            {
                                TriviaQuestion question = triviaResponse.Results[0];
                                Console.WriteLine("Question: " + question.Question);

                                List<string> answers = new List<string>();
                                Console.WriteLine("Here are some options for you peasants [Answer is case sensetive, you won't be spared either way]");
                                for (int i = 0; i < question.IncorrectAnswers.Count; i++)
                                {
                                    answers.Add(question.IncorrectAnswers[i]);
                                }
                                answers.Add(question.CorrectAnswer);

                                answers = answers.OrderBy(x => Random.Shared.Next()).ToList();

                                foreach (string answer in answers)
                                {
                                    Console.WriteLine($"-{answer}");
                                }

                                Console.WriteLine("Enter your Answer, Remember wrong answer = data sell");
                                string m_answer = Console.ReadLine();
                                if (m_answer == question.CorrectAnswer)
                                {
                                    Console.WriteLine("Correct, You're spared.");
                                    Console.WriteLine("Fetching more data...");
                                }
                                else
                                {
                                    Console.WriteLine($"Unfortunately, your answer {m_answer} does not equate to {question.CorrectAnswer}");
                                    Console.WriteLine("Would you like another chance? Y/N");
                                    string res = Console.ReadLine();
                                    switch (res.ToUpper()) // Convert to upper case for case insensitivity
                                    {
                                        case "Y":
                                            Console.WriteLine("Fetching Data...");
                                            continue; 
                                        case "N":
                                            Console.WriteLine("RIP, CCP will find you");
                                            return; // Exit the function
                                        default:
                                            Console.WriteLine("Invalid option, Selling your data");
                                            return; // Exit the function
                                    }
                                }

                            }
                            else
                            {
                                Console.WriteLine("No trivia question found in the response.");
                            }

                        }
                        else
                        {
                            Console.WriteLine("Failed to get response, status code: " + response.StatusCode);
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"You're safe for now big boy {e.Message} has halted the operation");
                    }
                }
            }
        }

    }
}