using System.Globalization;
using Discord;
using Discord.Commands;

namespace ChattyMeDiscordBot.Modules;

[Group("statistics")]
public class StatisticsModule : ModuleBase<SocketCommandContext>
{
    [Group("messages")]
    public class MessagesStatisticsModule : ModuleBase<SocketCommandContext>
    {
        [Command("today")]
        public async Task GetAmountOfMessagesInCurrentChannelTodayAsync()
        {
            const int maxMessagesCount = 500;
            var today = DateTime.Now.Date;

            await CountMessagesUntilDateAsync(today, maxMessagesCount);
        }

        [Command("week")]
        public async Task GetAmountOfMessagesInCurrentChannelLastWeekAsync()
        {
            const int maxMessagesCount = 1000;
            var lastWeek = DateTime.Now.AddDays(-7).Date;

            await CountMessagesUntilDateAsync(lastWeek, maxMessagesCount);
        }

        private async Task CountMessagesUntilDateAsync(DateTime toDate, int maxMessagesCount)
        {
            var dateStringRepresentation = toDate.ToString("d", CultureInfo.GetCultureInfo("en-US"));
            var lastMessage = (await Context.Channel.GetMessagesAsync(1).FlattenAsync()).FirstOrDefault();
            if (lastMessage == null || lastMessage.CreatedAt.Date < toDate)
            {
                await Context.Channel.SendMessageAsync(
                    $"No messages were published to this channel since {dateStringRepresentation}.");
                return;
            }

            var messagesCount = 0;
            var intendedCancellation = false;
            var lastMessageId = lastMessage.Id;
            var source = new CancellationTokenSource();
            try
            {
                await foreach (var messageBatch in Context.Channel.GetMessagesAsync(
                                   lastMessageId, Direction.Before, maxMessagesCount,
                                   options: new RequestOptions {CancelToken = source.Token}))
                {
                    foreach (var message in messageBatch)
                    {
                        if (message.CreatedAt.Date < toDate)
                        {
                            intendedCancellation = true;
                            source.Cancel();
                            break;
                        }

                        messagesCount++;
                    }
                }
            }
            catch (Exception exception)
            {
                if (exception is not (OperationCanceledException or TimeoutException) || !intendedCancellation)
                {
                    throw;
                }
            }

            await Context.Channel.SendMessageAsync(messagesCount == maxMessagesCount
                ? $"More than {messagesCount} were published to this channel since {dateStringRepresentation}."
                : $"{messagesCount} messages were published to this channel since {dateStringRepresentation}.");
        }
    }
}