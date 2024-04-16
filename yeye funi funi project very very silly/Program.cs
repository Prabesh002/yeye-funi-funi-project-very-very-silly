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


    public enum optionType
    {
        numeric,
        text
    }

    class Program
    {
        async static Task Main(string[] args)
        {
            Program program= new Program();
            Console.WriteLine("Welcome to Nitrous Oxide Gang You will face consequences for this brainrot  Press any key to continue");
            Console.ReadKey();
            string? key;
            Console.WriteLine($"Please select your option type. Select numeric for {optionType.numeric} and text for {optionType.text}");
            key = Console.ReadLine();
            key.ToLower();
            Console.WriteLine("You must guess the right answer, if you don't we will sell your data to the chinese government! [Emter to continue]");
            Console.ReadKey();
            Console.WriteLine("Fetching questions...");
            await GetQuestions(key);

        }

        static async Task GetQuestions(string option)
        {
            quizLogic quizLogic = new quizLogic();
            switch (option)
            {
                case "numeric":
                    await quizLogic.fetchData(optionType.numeric);

                 break;

                case "text":
                    await quizLogic.fetchData(optionType.text);
                    break;

                default:
                    Console.WriteLine("Invalid option type. The cartel members are definitely not happy");
                 return;
            }
        }

    }
}