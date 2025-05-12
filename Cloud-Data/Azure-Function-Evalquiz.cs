//using edusync.Models;
//using Microsoft.IdentityModel.Protocols;
//using Newtonsoft.Json;
//using System.Net;

//[Function("EvaluateQuiz")]
//public async Task<HttpResponseData> Run(
//    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "evaluate")] HttpRequestData req,
//    FunctionContext executionContext)
//{
//    var logger = executionContext.GetLogger("EvaluateQuiz");

//    try
//    {
//        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
//        var submission = JsonConvert.DeserializeObject<QuizSubmissionDto>(requestBody);

//        if (submission == null || submission.QuizId == Guid.Empty || submission.Answers == null)
//        {
//            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
//            await badResponse.WriteStringAsync("Invalid submission.");
//            return badResponse;
//        }

//        using var dbContext = new EduSyncDbContext(); // or inject via DI

//        var questions = await dbContext.Questions
//            .Where(q => q.QuizId == submission.QuizId)
//            .ToListAsync();

//        int score = 0;

//        foreach (var answer in submission.Answers)
//        {
//            var question = questions.FirstOrDefault(q => q.Id == answer.QuestionId);
//            if (question != null && question.CorrectOptionId == answer.SelectedOption)
//            {
//                score++;
//            }
//        }

//        var result = new Result
//        {
//            Id = Guid.NewGuid(),
//            UserId = submission.UserId,
//            QuizId = submission.QuizId,
//            SubmittedAt = submission.SubmittedAt,
//            Score = score
//        };

//        dbContext.Results.Add(result);
//        await dbContext.SaveChangesAsync();

//        var response = req.CreateResponse(HttpStatusCode.OK);
//        await response.WriteAsJsonAsync(new { result.Score });
//        return response;
//    }
//    catch (Exception ex)
//    {
//        logger.LogError(ex, "Error evaluating quiz.");
//        var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
//        await errorResponse.WriteStringAsync("Error processing quiz submission.");
//        return errorResponse;
//    }
//}
//public class QuizSubmissionDto
//{
//    public string UserId { get; set; }
//    public Guid QuizId { get; set; }
//    public DateTime SubmittedAt { get; set; }
//    public List<AnswerDto> Answers { get; set; }
//}

//public class AnswerDto
//{
//    public int QuestionId { get; set; }
//    public int SelectedOption { get; set; }
//}

