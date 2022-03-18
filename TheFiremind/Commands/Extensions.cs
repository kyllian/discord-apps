using Discord;
using TheFiremind.Models;

namespace TheFiremind.Commands;

internal static class Extensions
{
    internal static Embed[] BuildEmbeds(this IScryfallCard card, Command command, ScryfallRuling[]? rulings = null) => command switch
    {
        Command.Rulings => rulings is null ? throw new ArgumentNullException(nameof(rulings)) : card.BuildOracleEmbeds(rulings),
        Command.Oracle => card.BuildOracleEmbeds(),
        Command.Card => card.BuildEmbeds(),
        _ => throw new ArgumentOutOfRangeException(nameof(command)),
    };

    internal static Embed[] BuildEmbeds(this IScryfallCard card, ScryfallRuling[] rulings) => card.BuildOracleEmbeds(rulings);

    internal static Embed[] BuildEmbeds(this IScryfallCard card)
    {
        List<Embed> embeds = new();
        var builder = card.GetBaseEmbedBuilder();
        switch (card.FaceCount)
        {
            case 1:
                embeds.Add(builder.BuildSingleFace(card));
                break;
            case 2:
                embeds.Add(builder.BuildFrontFace(card));
                embeds.Add(builder.BuildBackFace(card));
                break;
            default:
                break;
        }

        return embeds.ToArray();
    }

    private static Embed[] BuildOracleEmbeds(this IScryfallCard card, ScryfallRuling[]? rulings = null)
    {
        List<Embed> embeds = new();
        var builder = card.GetBaseEmbedBuilder();

        switch (card.FaceCount)
        {
            case 1:
                embeds.Add(builder.AddFields(card, rulings).BuildSingleFace(card));
                break;
            case 2:
                embeds.Add(builder.AddFields(card, rulings).BuildFrontFace(card));
                embeds.Add(builder.BuildBackFace(card));
                break;
            default:
                break;
        }

        return embeds.ToArray();
    }

    private static Embed BuildSingleFace(this EmbedBuilder builder, IScryfallCard card) => builder.WithImageUrl(card.Image_Uris!.Png).Build();
    private static Embed BuildFrontFace(this EmbedBuilder builder, IScryfallCard card) => builder.WithImageUrl(card.Card_Faces!.First().Image_Uris!.Png).Build();
    private static Embed BuildBackFace(this EmbedBuilder builder, IScryfallCard card) => builder.WithImageUrl(card.Card_Faces!.Last().Image_Uris!.Png).Build();

    /// <summary>
    /// Sharing the Url between two embeds combines the embeds into one in the Discord client
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    private static EmbedBuilder GetBaseEmbedBuilder(this IScryfallCard card) => new()
    {
        Title = card.Name,
        Url = card.Scryfall_Uri
    };

    private static EmbedBuilder AddFields(this EmbedBuilder builder, IScryfallCard card)
    {
        switch (card.FaceCount)
        {
            case 1:
                return builder.AddField("Mana Cost", card.Mana_Cost).AddField("Oracle Text", card.Oracle_Text);
            case 2:
                var front = card.Card_Faces!.First();
                var back = card.Card_Faces!.Last();

                static string validate(string? value) => string.IsNullOrWhiteSpace(value) ? "None" : value;

                return builder.AddField("Front Mana Value", validate(front.Mana_Cost), inline: true)
                    .AddField("Back Mana Value", validate(back.Mana_Cost), inline: true)
                    .AddField("Front Oracle Text", validate(front.Oracle_Text), inline: false)
                    .AddField("Back Oracle Text", validate(back.Oracle_Text), inline: true);
            default:
                throw new ArgumentOutOfRangeException(nameof(card), $"IScryfallCard property FaceCount was out of range: {card.FaceCount}.");
        }
    }

    private static EmbedBuilder AddFields(this EmbedBuilder builder, IScryfallCard card, ScryfallRuling[]? rulings)
    {
        if (rulings is null)
        {
            return builder.AddFields(card);
        }

        foreach (var ruling in rulings)
        {
            builder.AddField($"{ruling.Source}: {ruling.Date:d}", ruling.Comment);
        }

        return rulings.Any() ? builder : builder.WithDescription($"No rulings for [{card.Name}]");
    }
}

internal enum Command
{
    Rulings,
    Oracle,
    Card
}