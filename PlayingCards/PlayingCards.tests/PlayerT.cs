// -----------------------------------------------
//     Author: Ramon Bollen
//      File: PlayingCards.tests.PlayerT.cs
// Created on: 20220722
// -----------------------------------------------

using PlayingCards.Models;

namespace PlayingCards.tests;

public class PlayingCardsT
{
    [Fact]
    public void TestHighestCard()
    {
        Player player = new(string.Empty);

        player.GiveCards("2H 3D 5S 9C KD 8C");

        player.Hand.HighestCard.Value.Should().Be(Card.ValueType.King);
        player.Hand.HighestCard.Suit.Should().Be(Card.SuitType.Diamonds);
    }

    [Theory]
    [InlineData("2H 3D 5S 9C KD", "PlayerA", "2C 3H 4S 8C AH", "PlayerB", "PlayerB", 1)]
    [InlineData("2H 4S 4C 2D 4H", "PlayerA", "2S 8S AS QS 3S", "PlayerB", "PlayerA", 1)]
    [InlineData("2H 3D 5S 9C QD", "PlayerA", "2C 3H 4S 8C KH", "PlayerB", "PlayerB", 1)]
    [InlineData("2H 3D 5S 9C KD", "PlayerA", "2D 3H 5C 9S KH", "PlayerB", "",        2)]
    public void HighestHandForPlayer(string cardsPlayerA, string namePlayerA, string cardsPlayerB, string namePlayerB, string nameOfWinningPlayer, int amountOfWinningPlayers)
    {
        Player player1 = new(namePlayerA);
        Player player2 = new(namePlayerB);

        player1.GiveCards(cardsPlayerA);
        player2.GiveCards(cardsPlayerB);

        Game game = new();
        game.AddPlayer(player1);
        game.AddPlayer(player2);

        game.Result().Count.Should().Be(amountOfWinningPlayers);
        if (amountOfWinningPlayers == 1) game.Result()[0].player.PlayerName.Should().Be(nameOfWinningPlayer);
    }
}