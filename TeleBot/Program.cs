using Telegram.Bot.Extensions;
using System;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

//VARIABLES

internal class Program

{
    private static TelegramBotClient botClient;
    //Bot Token

    //Time
    int year;
    int month;
    int day;
    int hour;
    int minute;
    int second;

    //Messages and user info
    long chatId = 0;
    string messageText;
    int messageId;
    string firstName;
    string lastName;
    long id;
    Message sentMessage;

    private static void Main(string[] args)

    {
        botClient = new TelegramBotClient("8031586249:AAHTljA4bDigFDonOW4RRgmop7xDzhf-RJw");


        //----------------------------//

        /* //Read time and save variables
         year = int.Parse(DateTime.UtcNow.Year.ToString());
         month = int.Parse(DateTime.UtcNow.Year.ToString());
         day = int.Parse(DateTime.UtcNow.Year.ToString());
         hour = int.Parse(DateTime.UtcNow.Year.ToString());
         minute = int.Parse(DateTime.UtcNow.Year.ToString());
         second = int.Parse(DateTime.UtcNow.Year.ToString());
         Console.WriteLine("Data: " + year + "/" + month + "/" + day);
         Console.WriteLine("Time: " + hour + ":" + minute + ":" + second);*/
        Task telegramRun = TelegramRun();
        Thread.Sleep(-1);

    }
    private static async Task TelegramRun()
    {
        // cts token
        using var cts = new CancellationTokenSource();

        // Bot StratReceiving, không chặn luồng người gọi. Việc nhận được thực hiện trên ThreadPool
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { },// receive all the updates types
            ThrowPendingUpdates = true
        };
        botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken: cts.Token);

        var me = await botClient.GetMeAsync();

        //write on console a hello message by bot
        Console.WriteLine($"\nHello! I'm {me.Username} and i'm your Bot!");

        // Send cancellation request to stop bot and close console 
        Console.ReadKey();
        cts.Cancel();

        //Answer of the bot to the input.
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            //Only process Message updates: https://core.telegram.org/bots/api
            if (update.Type != UpdateType.Message)
                return;
            //Only process text messages
            if (update.Message!.Type != MessageType.Text)
                return;

            //set variables
            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;
            var messageId = update.Message.MessageId;
            var firstName = update.Message.From.FirstName;
            var lastName = update.Message.From.LastName;
            var id = update.Message.From.Id;
            var year = update.Message.Date.Year;
            var month = update.Message.Date.Month;
            var day = update.Message.Date.Day;
            var minute = update.Message.Date.Minute;
            var second = update.Message.Date.Second;

            /* //when receive a message show data and time on console.
             Console.WriteLine("Data message --> " + year + "/" + month + "/" + day + "-" + ":" + second);
             //show the message, the chat id and the user info on console.
             Console.WriteLine($"Received a '{messageText}' message in chat {chatId} from user:\n" + firstName + " -" + lastName + "-" + "- " + id + " -");
 */
            //Insert this if to solve a bug, if you haven't problems you can removed it.
            var hour = update.Message.Date.Hour;
            if (messageText != null && int.Parse(day.ToString()) >= day && int.Parse(hour.ToString()) >= hour && int.Parse
            (minute.ToString()) >= minute && int.Parse(second.ToString()) >= second - 10)
            {
                //if message is Hello .. bot answer Hello.
                if (messageText == "Hello" || messageText == "hello")
                {
                    // Echo received message text
                    var sentMessage = await botClient.SendTextMessageAsync(
                       chatId: chatId,
                       text: "Hello" + firstName + " " + lastName + "",
                       //replyMarkup: replyKeyboardMarkup,
                       cancellationToken: cancellationToken);
                    return;
                }
                if (messageText == "yeubot" || messageText == "yeubot")
                {
                    var sentMessage = await botClient.SendTextMessageAsync(
                       chatId: chatId,
                       text: "Camon" + firstName + " " + lastName + "",
                       cancellationToken: cancellationToken);
                    return;
                }
            }
        }

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                => $"Telegram Api Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(ErrorMessage);
            botClient.SendTextMessageAsync(1684623466, ErrorMessage);
            return Task.CompletedTask;
        }

    }

}
