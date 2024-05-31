using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OpiumOsaka
{
    public class quizLogic
    {
        public static async Task fetchData(optionType type)
        {
            while (true)
            {
                using (HttpClient? client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage? response = await client.GetAsync("https://opentdb.com/api.php?amount=1&category=20&difficulty=hard&type=multiple");
                        if (response.IsSuccessStatusCode)
                        {
                            string? responseBody = await response.Content.ReadAsStringAsync();
                            TriviaQuestionResponse? triviaResponse = JsonConvert.DeserializeObject<TriviaQuestionResponse>(responseBody);


                            if (triviaResponse != null && triviaResponse.Results.Count > 0)
                            {
                                TriviaQuestion? question = triviaResponse.Results[0];
                                Console.WriteLine("Question: " + question.Question);

                                List<string>? answers = new List<string>();
                                if(type == optionType.text)
                                {
                                    Console.WriteLine("Here are some options for you peasants [Answer is case sensetive, you won't be spared either way]");
                                }
                                for (int i = 0; i < question.IncorrectAnswers.Count; i++)
                                {
                                    answers.Add(question.IncorrectAnswers[i]);
                                }
                                answers.Add(question.CorrectAnswer);

                                answers = answers.OrderBy(x => Random.Shared.Next()).ToList();

                                for (int i = 0; i < answers.Count; i++)
                                {
                                    Console.WriteLine($"{i} {answers[i]}");
                                }

                                Console.WriteLine("Enter your Answer, Remember wrong answer = data sell");


                                if (type == optionType.text)
                                {
                                    string? m_answer = Console.ReadLine();
                                    if (m_answer == question.CorrectAnswer)
                                    {
                                        Console.WriteLine("Correct, You're spared.");
                                        Console.WriteLine("Fetching more data...");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Unfortunately, your answer {m_answer} does not equate to {question.CorrectAnswer}");
                                        Console.WriteLine("Would you like another chance? Y/N");
                                        string? res = Console.ReadLine();
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
                                else if (type == optionType.numeric)
                                {
                                    string? m_answer = Console.ReadLine();
                                    try
                                    {
                                        int s_option = int.Parse(m_answer);
                                        if (answers[s_option] == question.CorrectAnswer)
                                        {
                                            Console.WriteLine("Correct, You're spared.");
                                            Console.WriteLine("Fetching more data...");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Unfortunately, your answer {answers[s_option]} does not equate to {question.CorrectAnswer}");
                                            Console.WriteLine("Would you like another chance? Y/N");
                                            string? res = Console.ReadLine();
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
                                    catch (FormatException)
                                    {
                                        Console.WriteLine($"Unable to take {m_answer} as an option");
                                        break;
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
