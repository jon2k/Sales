using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Sales.Services
{
    public class UpdateService : IUpdateService
    {
        private readonly IBotService _botService;
        private readonly ILogger<UpdateService> _logger;

        public UpdateService(IBotService botService, ILogger<UpdateService> logger)
        {
            _botService = botService;
            _logger = logger;
        }

        public async Task EchoAsync(Update update)
        {
            switch (update.Type)
            {
                case UpdateType.CallbackQuery:
                    BotOnCallbackQueryReceived(update.CallbackQuery);
                    break;
                case UpdateType.Message:
                    BotOnMessageReceived(update.Message);
                    break;
                case UpdateType.EditedMessage:
                    BotOnMessageReceived(update.Message);
                    break;
                case UpdateType.InlineQuery:
                    BotOnInlineQueryReceived(update.InlineQuery);
                    break;
                case UpdateType.ChosenInlineResult:
                    BotOnChosenInlineResultReceived(update.ChosenInlineResult);
                    break;
            }



            //  _logger.LogInformation("Received Message from {0}", message.Chat.Id);


            async void BotOnMessageReceived(Message message)
            {
                switch (message.Type)
                {
                    case MessageType.Text:
                        // Echo each Message
                        await _botService.Client.SendTextMessageAsync(message.Chat.Id, message.Text);

                        //
                        var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                    // first row
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Удалить", "Удалено"),
                        InlineKeyboardButton.WithCallbackData("1.2", "Удалено"),
                    }

                });
                        await _botService.Client.SendTextMessageAsync(
                            chatId: message.Chat.Id,
                            text: "Choose",
                            replyMarkup: inlineKeyboard
                        );
                        //
                        break;


                    case MessageType.Photo:
                        // Download Photo
                        var fileId = message.Photo.LastOrDefault()?.FileId;
                        var file = await _botService.Client.GetFileAsync(fileId);

                        var filename = file.FileId + "." + file.FilePath.Split('.').Last();
                        using (var saveImageStream = System.IO.File.Open(filename, FileMode.Create))
                        {
                            await _botService.Client.DownloadFileAsync(file.FilePath, saveImageStream);
                        }

                        await _botService.Client.SendTextMessageAsync(message.Chat.Id, "Thx for the Pics");
                        break;
                }
            }
            async void BotOnCallbackQueryReceived(CallbackQuery callbackQuery)
            {
                await _botService.Client.AnswerCallbackQueryAsync(
               callbackQuery.Id,
               $"Received {callbackQuery.Data}"
           );
                InlineQueryResultBase[] results = {
                // displayed result
                new InlineQueryResultArticle(
                    id: "3",
                    title: "TgBots",
                    inputMessageContent: new InputTextMessageContent(
                        "hello"
                    )
                )
            };


                await _botService.Client.AnswerInlineQueryAsync(
              callbackQuery.Id, results

          );

                await _botService.Client.SendTextMessageAsync(
                    callbackQuery.Message.Chat.Id,
                    $"Received {callbackQuery.Data}"
                );
            }
            void BotOnInlineQueryReceived(InlineQuery update2)
            {

            }
            void BotOnChosenInlineResultReceived(ChosenInlineResult update3)
            {

            }

        }

    }
}
